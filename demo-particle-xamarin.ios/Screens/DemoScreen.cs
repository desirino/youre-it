using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

using ExitGames.Client.DemoParticle;
using ExitGames.Client.Photon.LoadBalancing;


namespace DemoParticle.Xamarin.iOS
{
	public partial class DemoScreen : UIViewController
	{
		private Demo demo;

		// Grid view 
		public UIImageView GridView { get; private set; }
		public int GridViewSize { get; private set; }
		private const int GridViewPadding = 20;
		private RectangleF gridViewBounds;

		public string[] ConnectionParams { get; private set; }

		public DemoScreen (string[] connectionParams) : base ("DemoScreen", null)
		{
			demo = new Demo(connectionParams, Repaint);
		}


		private void Repaint()
		{
			InvokeOnMainThread(delegate { DrawDemoGrid();});
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

			this.btnUp.Hidden = true;
			this.btnDown.Hidden = true;		
			this.btnLeft.Hidden = true;
			this.btnRight.Hidden = true;

			this.NavigationItem.SetHidesBackButton(true, true);

			this.btnAddClient.TouchUpInside += (sender, e) => {
				this.demo.AddClient();
			}; 

			this.btnRemoveClient.TouchUpInside += (sender, e) => {
				this.demo.RemoveClient();
			}; 

			this.btnChangeGridSize.TouchUpInside += (sender, e) => {
				this.demo.ChangeGridSize();

			}; 

			this.btnChangeColor.TouchUpInside += (sender, e) => {
				this.demo.ChangeLocalPlayerColor();
			}; 

			this.switchAutomove.ValueChanged += (sender, e) => {
				this.demo.LocalGameLogic.MoveInterval.IsEnabled = this.switchAutomove.On;
				this.btnUp.Hidden = this.switchAutomove.On;
				this.btnDown.Hidden = this.switchAutomove.On;		
				this.btnLeft.Hidden = this.switchAutomove.On;
				this.btnRight.Hidden = this.switchAutomove.On;

				this.demo.UpdateVisuals(true);
			};

			this.switchInterestManagement.ValueChanged += (sender, e) => {
					
				this.demo.InterestGroupsOnOff();
			};

			this.switchShowClientName.ValueChanged += (sender, e) => {
				this.demo.ShowUserInfo(this.switchShowClientName.On);
			};

			this.btnUp.TouchUpInside += (sender, e) => {
				this.demo.MoveLocalPlayerDown();
			};

			this.btnDown.TouchUpInside += (sender, e) => {
				this.demo.MoveLocalPlayerUp();
			};

			this.btnLeft.TouchUpInside += (sender, e) => {
				this.demo.MoveLocalPlayerLeft();
			};
			
			this.btnRight.TouchUpInside += (sender, e) => {
				this.demo.MoveLocalPlayerRight();
			};
		}


		private void DrawDemoGrid()
		{
			if (!(this.demo.LocalGameLogic.LocalRoom != null && this.demo.LocalGameLogic.State == ClientState.Joined)) return;
			// Set the dimensions of the grid
			int GridViewSize = (int) this.View.Frame.Width - GridViewPadding * 2;
			gridViewBounds = new RectangleF(GridViewPadding, GridViewPadding, GridViewSize, GridViewSize);

			// Prepare the context
			int bytesPerRow = (int) this.View.Frame.Width * 4;
			byte[] contextBuffer = new byte[bytesPerRow * (int) this.View.Frame.Width];
			CGBitmapContext context = new CGBitmapContext(contextBuffer, (int) this.View.Frame.Width, (int) this.View.Frame.Width, 8, bytesPerRow, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.PremultipliedFirst);

			// Draw a background of the grid
			if (this.demo.LocalGameLogic.UseInterestGroups)
			{
				context.SetFillColor(0.5f, 0.5f, 0.5f, 1.0f);
				context.FillRect(new RectangleF(GridViewPadding, GridViewPadding, GridViewSize / 2, GridViewSize / 2));
				context.FillRect(new RectangleF(GridViewSize / 2 + GridViewPadding, GridViewSize / 2 + GridViewPadding, GridViewSize / 2, GridViewSize / 2));
				context.SetFillColor(0.8f, 0.8f, 0.8f, 1.0f);
				context.FillRect(new RectangleF(GridViewSize / 2 + GridViewPadding, GridViewPadding, GridViewSize / 2, GridViewSize / 2));
				context.FillRect(new RectangleF(GridViewPadding, GridViewSize / 2 + GridViewPadding, GridViewSize / 2, GridViewSize / 2)); 
			}
			else
			{
				context.SetFillColor(0.8f, 0.8f, 0.8f, 1.0f);
				context.FillRect(new RectangleF(GridViewPadding, GridViewPadding, GridViewSize, GridViewSize));
			}

			// Draw cells
			context.SetFillColor(1.0f, 1.0f, 1.0f, 1.0f);
			context.SetLineWidth(2.0f); 
			float rectSize = (float) GridViewSize / this.demo.LocalGameLogic.GridSize;

			using (CGPath path = new CGPath())
			{
				for (int i = 0; i <= this.demo.LocalGameLogic.GridSize; i++)
				{
					float x = i * rectSize;
					path.AddLines(new PointF[] {
						new PointF(x + GridViewPadding, GridViewPadding), 
						new PointF(x + GridViewPadding, GridViewPadding + GridViewSize),
					});
					path.CloseSubpath();
					context.AddPath(path);
					context.DrawPath(CGPathDrawingMode.FillStroke);

					path.AddLines(new PointF[] {
						new PointF(GridViewPadding, x + GridViewPadding), 
						new PointF(GridViewSize + GridViewPadding, x + GridViewPadding),
					});
					path.CloseSubpath();
					context.AddPath(path);
					context.DrawPath(CGPathDrawingMode.FillStroke);
				}
			}

			// Set the size, position and add to current Demo View the UIImageView component on which the Playground will be drawn
			if (GridView != null)
				this.View.Subviews[this.View.Subviews.Length-1].RemoveFromSuperview();

			lock (this.demo.LocalGameLogic.LocalRoom.Players)
			{
				foreach (ParticlePlayer p in this.demo.LocalGameLogic.LocalRoom.Players.Values)
				{
					float x = p.PosX * rectSize + GridViewPadding;
					float y = GridViewSize - p.PosY * rectSize + GridViewPadding - rectSize;
					float alpha = 1.0f;
					if (!p.IsLocal && p.UpdateAge > 500)
					{
						alpha = (p.UpdateAge > 1000) ? 0.2f : 0.8f;
					}

					UIColor convertedColor = DemoScreen.IntToColor(p.Color, alpha);
					float red;
					float green;
					float blue;
					convertedColor.GetRGBA(out red, out green, out blue, out alpha);
					context.SetFillColor(red, green, blue, alpha);
					context.FillRect(new RectangleF(x, y, rectSize, rectSize));
					
					if (this.demo.UserInfoIsVisible)
					{
						context.SelectFont("Arial", 16f, CGTextEncoding.MacRoman);
						context.SetFillColor(1.0f, 1.0f, 1.0f, 1.0f);
						context.SetTextDrawingMode(CGTextDrawingMode.Fill);
						context.ShowTextAtPoint(x + rectSize / 2, y + rectSize / 2, p.Name);	
					}
				}
			}

			GridView = new UIImageView(gridViewBounds);
			this.View.AddSubview(GridView);
			GridView.Image = UIImage.FromImage(context.ToImage());
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

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

		public static UIColor IntToColor(int i, float alpha)
		{
			float red = (byte) (i >> 16);
			float green = (byte) (i >> 8);
			float blue = (byte) i;
			
			return UIColor.FromRGBA((float) (red / 255), (float) (green / 255), (float) (blue / 255),  alpha);
		}
	}
}

