// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace DemoParticle.Xamarin.iOS
{
	[Register ("ConnectionScreen")]
	partial class ConnectionScreen
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnConnect { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField labelServerAddress { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField labelAppId { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField labelGameVersion { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnConnect != null) {
				btnConnect.Dispose ();
				btnConnect = null;
			}

			if (labelServerAddress != null) {
				labelServerAddress.Dispose ();
				labelServerAddress = null;
			}

			if (labelAppId != null) {
				labelAppId.Dispose ();
				labelAppId = null;
			}

			if (labelGameVersion != null) {
				labelGameVersion.Dispose ();
				labelGameVersion = null;
			}
		}
	}
}
