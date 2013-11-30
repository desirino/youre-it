using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Style;
using Style.Enums;

namespace YoureItUI
{
	[Activity (Label = "You're It", MainLauncher = true)]
	public class MainActivity : Activity //SherlockActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//NativeCSS.StyleWithCSS("styles.css", new Uri("http://ugrad.bitdegree.ca/~andrewbrough/youreit/styles.css"), RemoteContentRefreshPeriod.EverySecond);

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add (0, 1, 1, Resource.String.Map);
			//menu.Add (0, 2, 2, Resource.String.Profile);
			MenuInflater.Inflate (Resource.Menu.ActionItems, menu);
			//Console.Write("----------" + menu);
			return true;
		}

		public bool onActionItemsClick(ActionMode mode, IMenuItem item)
		{
			Android.Widget.Toast.MakeText (this, 
			                               "Selected Item: " + 
			                               item.TitleFormatted, 
			                               Android.Widget.ToastLength.Short).Show();

			return true;

//			switch (item.ItemId) {
//			case Resource.Id.menu_map:
//				StartActivity (typeof(MainActivity));
//				//OpenMap ();
//				return true;
//			case Resource.Id.menu_profile:
//				Console.WriteLine ("-----------------");
//				StartActivity (typeof(ProfileActivity));
//				//OpenProfile ();
//				return true;
//			}
//			return false;
		}

		public bool onOptionsItemSelected(IMenuItem item)
		{
			Android.Widget.Toast.MakeText (this, 
			                               "Selected Item: " + 
			                               item.TitleFormatted, 
			                               Android.Widget.ToastLength.Short).Show();

			return true;

//			switch (item.ItemId) {
//			case Resource.Id.menu_map:
//				StartActivity (typeof(MainActivity));
//				//OpenMap ();
//				return true;
//			case Resource.Id.menu_profile:
//				Console.Write ("-------------");
//				StartActivity (typeof(ProfileActivity));
//				//OpenProfile ();
//				return true;
//			}
//			return true;
		}

		public void OpenMap()
		{
			StartActivity (typeof(MainActivity));
		}

		public void OpenProfile()
		{
			StartActivity (typeof(ProfileActivity));
		}


	}
}