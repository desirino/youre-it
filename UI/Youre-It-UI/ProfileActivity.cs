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

using Style;
using Style.Enums;

namespace YoureItUI
{
	[Activity (Label = "ProfileActivity")]			
	public class ProfileActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MenuScreen);
			//ActionBar.Hide ();

			NativeCSS.StyleWithCSS("styles.css", new Uri("http://ugrad.bitdegree.ca/~andrewbrough/youreit/styles.css"), RemoteContentRefreshPeriod.EverySecond);
		}
	}
}

