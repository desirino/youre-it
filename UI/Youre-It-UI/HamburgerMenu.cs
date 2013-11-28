using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace YoureItUI
{
	[Activity (Theme = "@style/Theme.notitle", Label = "HamburgerMenu")]			
	public class HamburgerMenu : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.HamburgerMenu);

			// Create your application here
			CreateButtons ();

		}

		protected void CreateButtons()
		{
			//get button from layout
			var MapButton = FindViewById<Button> (Resource.Id.MapButton);
			//delegate function
			MapButton.Click += (sender, e) => {
				//StartActivity (typeof(HamburgerMenu));
			};

//			var ProfileButton = FindViewById<Button> (Resource.Id.ProfileButton);
//			ProfileButton.Click += (sender, e) => {
//				//StartActivity (typeof(HamburgerMenu));
//			};

		}
	}
}

