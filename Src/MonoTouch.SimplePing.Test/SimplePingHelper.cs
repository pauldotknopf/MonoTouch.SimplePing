using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;

namespace MonoTouch.SimplePing.Test
{
	public class SimplePingHelper : NSObject, IDisposable
	{
		SimplePing _pinger;
		bool _completed = true;
		int _timeout;
		string _address;
		private object _lock = new object ();

		public SimplePingHelper (string address, int timeout)
		{
			_address = address.ToString();
			_timeout = timeout;
			if (_timeout <= 0)
				_timeout = 1000;
			_pinger = SimplePing.WithHostName (_address);
			_pinger.WeakDelegate = this;
		}

		~SimplePingHelper()
		{
			Dispose ();
		}

		#region SimplePingDelegate

		[Export ("simplePing:didFailToSendPacket:error:")]
		public virtual void DidFailToSendPacket (SimplePing pinger, NSData packet, NSError error)
		{
			Fail ();
		}

		[Export ("simplePing:didFailWithError:")]
		public virtual void DidFailWithError (SimplePing pinger, NSError error)
		{
			Fail ();
		}

		[Export ("simplePing:didReceiveUnexpectedPacket:")]
		public virtual void DidReceiveUnexpectedPacket (SimplePing pinger, NSData packet)
		{
			Fail ();
		}

		[Export ("simplePing:didReceivePingResponsePacket:")]
		public virtual void DidRecievePingResponsePacket (SimplePing pinger, NSData packet)
		{
			Pass ();
		}

		[Export ("simplePing:didSendPacket:")]
		public virtual void DidSendPacket (SimplePing pinger, NSData packet)
		{
		}

		[Export ("simplePing:didStartWithAddress:")]
		public virtual void DidStartWithAddress (SimplePing pinger, NSData address)
		{
			pinger.SendPingWithData (null);
		}

		#endregion

		#region Properties

		public int Timeout
		{
			get{ return _timeout; }
		}

		#endregion

		#region Methods

		public static void Ping(string hostName, int timeout, Action success, Action failure)
		{
			var helper = new SimplePingHelper (hostName, timeout);
			helper.Succedded += (object sender, EventArgs e) => {
				if(success != null)
					success();
				(sender as SimplePingHelper).Release();
				(sender as SimplePingHelper).Dispose();
			};
			helper.Failed += (object sender, EventArgs e) => {
				if(failure != null)
					failure();
				(sender as SimplePingHelper).Release();
				(sender as SimplePingHelper).Dispose();
			};
			helper.Retain ();
			helper.Go ();
		}

		public void Go()
		{
			if (!_completed)
				throw new InvalidOperationException ("You must wait until the current ping is completed.");
			_completed = false;
			_pinger.Start ();
			// start the timeout
			Task.Factory.StartNew (state => {
				System.Threading.Thread.Sleep((state as SimplePingHelper).Timeout);
				(state as SimplePingHelper).Fail();
			}, this);
		}

		public void Stop()
		{
			Fail ();
		}

		private void Fail()
		{
			lock (_lock) {
				if (_completed)
					return;
				_pinger.Stop ();
				_completed = true;
			}
			RaiseFailed ();
		}

		private void Pass()
		{
			lock (_lock) {
				if (_completed)
					return;
				_pinger.Stop ();
				_completed = true;
			}
			RaiseSuccedded ();
		}

		public void Dispose ()
		{
			Dispose (true);
			Succedded = null;
			Failed = null;
			_address = null;
			if (_pinger != null) {
				_pinger.Stop ();
				_pinger.Dispose ();
				_pinger = null;
			}
		}

		#endregion

		#region Events

		public event EventHandler Succedded;

		public void RaiseSuccedded()
		{
			var handler = Succedded;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		public event EventHandler Failed;

		public void RaiseFailed()
		{
			var handler = Failed;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

        #endregion
	}
}

