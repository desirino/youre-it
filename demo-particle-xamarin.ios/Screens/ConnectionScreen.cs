
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using ExitGames.Client.DemoParticle;
using ExitGames.Client.Photon.LoadBalancing;

namespace DemoParticle.Xamarin.iOS
{
	public partial class ConnectionScreen : UIViewController
	{
		private const string ServerAddress = "app.exitgamescloud.com:5055";
		private const string AppId = ""; // Enter your AppId here
		private const string GameVersion = "1.0";

		DemoScreen demoScreen;

		public ConnectionScreen () : base ("ConnectionScreen", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.labelServerAddress.Text = ServerAddress;
			this.labelAppId.Text = AppId;
			this.labelGameVersion.Text = GameVersion;

			this.btnConnect.TouchUpInside += (sender, e) => {

				if (demoScreen == null)
				{
					demoScreen = new DemoScreen(new string[] {
						this.labelServerAddress.Text, 
					    this.labelAppId.Text, 
					    this.labelGameVersion.Text
					});
				}

				this.NavigationController.PushViewController(this.demoScreen, true);
			};
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;

			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden (true, animated);
		}
		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);
			this.NavigationController.SetNavigationBarHidden (false, animated);
		}
	}
}

