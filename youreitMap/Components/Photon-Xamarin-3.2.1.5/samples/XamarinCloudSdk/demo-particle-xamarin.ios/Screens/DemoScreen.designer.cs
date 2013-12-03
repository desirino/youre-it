// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace DemoParticle.Xamarin.iOS
{
	[Register ("DemoScreen")]
	partial class DemoScreen
	{
		[Outlet]
		MonoTouch.UIKit.UISwitch switchInterestManagement { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch switchAutomove { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnAddClient { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnRemoveClient { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnChangeGridSize { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnChangeColor { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch switchShowClientName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnUp { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnDown { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnRight { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (switchInterestManagement != null) {
				switchInterestManagement.Dispose ();
				switchInterestManagement = null;
			}

			if (switchAutomove != null) {
				switchAutomove.Dispose ();
				switchAutomove = null;
			}

			if (btnAddClient != null) {
				btnAddClient.Dispose ();
				btnAddClient = null;
			}

			if (btnRemoveClient != null) {
				btnRemoveClient.Dispose ();
				btnRemoveClient = null;
			}

			if (btnChangeGridSize != null) {
				btnChangeGridSize.Dispose ();
				btnChangeGridSize = null;
			}

			if (btnChangeColor != null) {
				btnChangeColor.Dispose ();
				btnChangeColor = null;
			}

			if (switchShowClientName != null) {
				switchShowClientName.Dispose ();
				switchShowClientName = null;
			}

			if (btnUp != null) {
				btnUp.Dispose ();
				btnUp = null;
			}

			if (btnDown != null) {
				btnDown.Dispose ();
				btnDown = null;
			}

			if (btnLeft != null) {
				btnLeft.Dispose ();
				btnLeft = null;
			}

			if (btnRight != null) {
				btnRight.Dispose ();
				btnRight = null;
			}
		}
	}
}
