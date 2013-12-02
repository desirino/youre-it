using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Support.V4.Widget;

//using Styles;
//using Styles.Enum;
using DrawerSample;

namespace youreitUIv2
{
	[Activity (Label = "You're It", MainLauncher = true)]
	public class MainActivity : Activity
	{
		//drawer stuff
		private DrawerLayout _drawer;
		private MyActionBarDrawerToggle _drawerToggle;
		private ListView _drawerList;

		private string _title;
		private string _drawerTitle;
		private string[] _menuTitles;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//NativeCSS.StyleWithCSS("styles.css", new Uri("http://ugrad.bitdegree.ca/~andrewbrough/youreit/styles.css"), RemoteContentRefreshPeriod.EverySecond);

			//drawer stuff
			_title = _drawerTitle = "Menu";
			_menuTitles = Resources.GetStringArray(Resource.Array.MenuItemNames);
			_drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			_drawerList = FindViewById<ListView>(Resource.Id.left_drawer);

			_drawer.SetDrawerShadow(Resource.Drawable.Icon, (int)GravityFlags.Start);

			_drawerList.Adapter = new ArrayAdapter<string>(this,
			                                               Resource.Layout.DrawerListItem, _menuTitles);
			_drawerList.ItemClick += (sender, args) => SelectItem(args.Position);


			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			//DrawerToggle is the animation that happens with the indicator next to the
			//ActionBar icon. You can choose not to use this.
			_drawerToggle = new MyActionBarDrawerToggle(this, _drawer,
			                                            Resource.Drawable.Icon,
			                                            Resource.String.hello,
			                                            Resource.String.hello);

			//You can alternatively use _drawer.DrawerClosed here
			_drawerToggle.DrawerClosed += delegate
			{
				ActionBar.Title = _title;
				InvalidateOptionsMenu();
			};

			//You can alternatively use _drawer.DrawerOpened here
			_drawerToggle.DrawerOpened += delegate
			{
				ActionBar.Title = _drawerTitle;
				InvalidateOptionsMenu();
			};

			_drawer.SetDrawerListener(_drawerToggle);

			if (null == bundle)
				SelectItem(0);
		}

		//more drawer stuff
		private void SelectItem(int position)
		{
			Console.WriteLine (position);

			switch(position){
			case 0:
				var mapfragment = new MapFragment ();
				var maparguments = new Bundle ();
				maparguments.PutInt ("stuff", position);
				mapfragment.Arguments = maparguments;

				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, mapfragment)
						.Commit ();

				_drawerList.SetItemChecked (position, true);
				ActionBar.Title = _title = _menuTitles [position];
				_drawer.CloseDrawer (_drawerList);
				break;
			case 1:
				var proffragment = new ProfileFragment ();
				var profarguments = new Bundle ();
				profarguments.PutInt ("stuff", position);
				proffragment.Arguments = profarguments;

				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, proffragment)
						.Commit ();

				_drawerList.SetItemChecked (position, true);
				ActionBar.Title = _title = _menuTitles [position];
				_drawer.CloseDrawer (_drawerList);
				break;
			case 2:
				var profifragment = new ProfileFragment ();
				var profiarguments = new Bundle ();
				profiarguments.PutInt ("stuff", position);
				profifragment.Arguments = profiarguments;

				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, profifragment)
						.Commit ();

				_drawerList.SetItemChecked (position, true);
				ActionBar.Title = _title = _menuTitles [position];
				_drawer.CloseDrawer (_drawerList);
				break;
			case 3:
				var powerfragment = new PowerupsFragment ();
				var powerarguments = new Bundle ();
				powerarguments.PutInt ("stuff", position);
				powerfragment.Arguments = powerarguments;

				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, powerfragment)
						.Commit ();

				_drawerList.SetItemChecked (position, true);
				ActionBar.Title = _title = _menuTitles [position];
				_drawer.CloseDrawer (_drawerList);
				break;
			case 4:
				var hotspfragment = new HotspotsFragment ();
				var hotarguments = new Bundle ();
				hotarguments.PutInt ("stuff", position);
				hotspfragment.Arguments = hotarguments;

				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, hotspfragment)
						.Commit ();

				_drawerList.SetItemChecked (position, true);
				ActionBar.Title = _title = _menuTitles [position];
				_drawer.CloseDrawer (_drawerList);
				break;
			case 5:
				var settingsfragment = new SettingsFragment ();
				var setarguments = new Bundle ();
				setarguments.PutInt ("stuff", position);
				settingsfragment.Arguments = setarguments;

				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, settingsfragment)
						.Commit ();

				_drawerList.SetItemChecked (position, true);
				ActionBar.Title = _title = _menuTitles [position];
				_drawer.CloseDrawer (_drawerList);
				break;
			}


		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);
			_drawerToggle.SyncState();
		}

		public override void OnConfigurationChanged(Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			_drawerToggle.OnConfigurationChanged(newConfig);
		}





		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			//add menu button items
			menu.Add (0, 0, 0, "Settings");
			//add ActionItems
			MenuInflater.Inflate (Resource.Menu.ActionItems, menu);

			//return true;
			//drawer stuff
			return base.OnCreateOptionsMenu(menu);
		}

		//drawer stuff
		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			var drawerOpen = _drawer.IsDrawerOpen(Resource.Id.left_drawer);
			menu.FindItem(Resource.Id.menu_map).SetVisible(!drawerOpen);
			menu.FindItem(Resource.Id.menu_profile).SetVisible(!drawerOpen);
			return base.OnPrepareOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			//drawer stuff
			if (_drawerToggle.OnOptionsItemSelected(item))
				return true;

			//handle menu button item selection
			Android.Widget.Toast.MakeText (this, 
			                               "Selected Item: " + 
			                               item.TitleFormatted, 
			                               Android.Widget.ToastLength.Short).Show();

//			switch (item.ItemId) {
//				case Resource.Id.menu_map:
//				StartActivity (typeof(MainActivity));
//				//OpenMap ();
//				return true;
//				case Resource.Id.menu_profile:
//				StartActivity (typeof(ProfileActivity));
//				//OpenProfile ();
//				return true;
//			}

			Console.Write ("------------");
			return true;
		}


	}
}


