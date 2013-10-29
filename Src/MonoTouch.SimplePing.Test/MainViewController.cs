using System;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.SimplePing.Test
{
	public class MainViewController : DialogViewController
	{
		EntryElement _hostNameElement;
		StyledStringElement _pingButtonElement;

		public MainViewController ()
			: base(null)
		{
			_hostNameElement = new EntryElement ("Target", "google.com", "") { TextAlignment = UITextAlignment.Right };
			_pingButtonElement = new StyledStringElement ("Ping"){ Alignment = UITextAlignment.Center };
			_pingButtonElement.Tapped += OnPingButtonClicked;

			Root = new MonoTouch.Dialog.RootElement ("SimplePing") {
				new Section () { 
					_hostNameElement,
					_pingButtonElement,
				}
			};
		}

		void OnPingButtonClicked ()
		{
			SimplePingHelper.Ping (
				_hostNameElement.Value, 
				1000, 
				() => {
					NSThread.MainThread.BeginInvokeOnMainThread (new NSAction (() => {
						var alertView = new UIAlertView ("Response", "Success", null, null, new string[] { "Ok" });
						alertView.Show ();
					}));
				}, 
				() => {
					NSThread.MainThread.BeginInvokeOnMainThread (new NSAction (() => {
						var alertView = new UIAlertView ("Response", "Failure", null, null, new string[] { "Ok" });
						alertView.Show ();
					}));
				});
		}
	}
}

