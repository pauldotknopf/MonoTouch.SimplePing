using System;
using System.Drawing;
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.SimplePing
{
	[BaseType (typeof (NSObject))]
	public partial interface SimplePing 
	{
		//- (id)initWithHostName:(NSString *)hostName address:(NSData *)hostAddress
		[Export ("initWithHostName:address:")]
		IntPtr Constructor ([NullAllowed]string hostName, [NullAllowed]NSData hostAddress);

		//+ (SimplePing *)simplePingWithHostName:(NSString *)hostName
		[Static, Export("simplePingWithHostName:")]
		SimplePing WithHostName(string hostName);

		//+ (SimplePing *)simplePingWithHostAddress:(NSData *)hostAddress
		[Static, Export("simplePingWithHostAddress:")]
		SimplePing WithHostAddress (NSData hostAddress);

		//@property (nonatomic, weak,   readwrite) id<SimplePingDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		SimplePingDelegate Delegate { get; set; }

		//- (void)start;
		[Export("start")]
		void Start();

		[Export("stop")]
		void Stop();

		//- (void)sendPingWithData:(NSData *)data;
		[Export("sendPingWithData:")]
		void SendPingWithData([NullAllowed]NSData data);

		//@property (nonatomic, copy,   readonly ) NSString *             hostName;
		[Export("hostName", ArgumentSemantic.Copy)]
		string HostName { get; }
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface SimplePingDelegate 
	{
		//- (void)simplePing:(SimplePing *)pinger didStartWithAddress:(NSData *)address;
		[Export("simplePing:didStartWithAddress:")]
		void DidStartWithAddress (SimplePing pinger, NSData address);

		//- (void)simplePing:(SimplePing *)pinger didFailWithError:(NSError *)error;
		[Export("simplePing:didFailWithError:")]
		void DidFailWithError (SimplePing pinger, NSError error);

		//- (void)simplePing:(SimplePing *)pinger didSendPacket:(NSData *)packet;
		[Export("simplePing:didSendPacket:")]
		void DidSendPacket(SimplePing pinger, NSData packet);

		//- (void)simplePing:(SimplePing *)pinger didFailToSendPacket:(NSData *)packet error:(NSError *)error;
		[Export("simplePing:didFailToSendPacket:error:")]
		void DidFailToSendPacket (SimplePing pinger, NSData packet, NSError error);

		//- (void)simplePing:(SimplePing *)pinger didReceivePingResponsePacket:(NSData *)packet;
		[Export("simplePing:didReceivePingResponsePacket:")]
		void DidRecievePingResponsePacket (SimplePing pinger, NSData packet);

		//- (void)simplePing:(SimplePing *)pinger didReceiveUnexpectedPacket:(NSData *)packet;
		[Export("simplePing:didReceiveUnexpectedPacket:")]
		void DidReceiveUnexpectedPacket (SimplePing pinger, NSData packet);
	}

//	public delegate void SimplePingResponseDelegate(NSNumber success);
//	[BaseType (typeof (NSObject))]
//	public partial interface SimplePingHelper : SimplePingDelegate 
//	{
//
//		[Static, Export ("ping:target:sel:")]
//		void Ping (string address, NSObject target, SimplePingResponseDelegate sel);
//	}
}

