using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace YoureItUI
{
	[Activity (Theme = "@style/Theme.notitle", Label = "Youre-It-UI", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//attaches functions to layout buttons
			CreateButtons ();


		}

		protected void CreateButtons()
		{
			//
			var HambugerButton = FindViewById<Button> (Resource.Id.HamburgerButton);
			//delegate function
			HambugerButton.Click += (sender, e) => {
				StartActivity (typeof(HamburgerMenu));
			};
		}
	}


}