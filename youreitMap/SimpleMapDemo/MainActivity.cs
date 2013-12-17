using System.Net;


namespace youreit
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;
	using Android;
    using Android.App;
    using Android.Content;
	using Android.Content.PM;
	using Android.Content.Res;
	using Android.Graphics;
    using Android.Gms.Common;
    using Android.OS;
    using Android.Util;
    using Android.Views;
    using Android.Widget;
	using Android.Support.V4.Widget;
	using Android.Locations;

	using DrawerSample;

	using Style;
	using Style.Enums;

    using AndroidUri = Android.Net.Uri;

	// **********  ADDED FOR MAP SCREEN
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using Android.Graphics;

		// **********  
	using Mono.Data.Sqlite;
	using Environment = System.Environment;

	using Style;
	
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
	
	public class MainActivity : Activity, ILocationListener
    {

		public static readonly int InstallGooglePlayServicesId = 1000;
        public static readonly string Tag = "You're It Map Testing";

		//private SampleActivity activity;
		private bool _isGooglePlayServicesInstalled;

		//Get Location 
		static readonly string LocationTag = "- - - - -  Location";
		Location _currentLocation;
		LocationManager _locMgr;
		string _locationProvider;
		public List<CustomGameMarker> markerList = new List<CustomGameMarker>();
	
		//Lists where all the data is stored 
		private List<HotspotData> hotspotList;
		private List<CustomizationData> customizationList;
		private List<PowerupData> powerupList;
		private List<UserData> userList;
		private PersonalInfoData myData; 

		private GoogleMap _map;
		private MapFragment _mapFragment;

		//For the Draw (Menu)
		private DrawerLayout _drawer;
		private MyActionBarDrawerToggle _drawerToggle;
		private ListView _drawerList;

		private string _title;
		private string _drawerTitle;
		private string[] _menuTitles;
		private CircleOptions circleOptions1 = new CircleOptions ();
		private CircleOptions circleOptions2 = new CircleOptions ();
		private CircleOptions circleOptions3 = new CircleOptions ();
		Circle circle1;
		Circle circle2;
		Circle circle3;

		// **********  


		// COULD REMOVE :D
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (resultCode)
            {
                case Result.Ok:
                    // Try again.
                    _isGooglePlayServicesInstalled = true;
                    break;

                default:
                    Log.Debug("MainActivity", "Unknown resultCode {0} for request {1}", resultCode, requestCode);
                    break;
            }
        }

		// NEED THIS :(
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			//DATAMANAGEMENT STUFF!

			string dbName = "youreit.sqlite";
			string dbPath = System.IO.Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);
	
			if (File.Exists (dbPath)) {
				File.Delete (dbPath);
			} 

			using (Stream source = Assets.Open(dbName))
			using (var dest = System.IO.File.Create (dbPath)) {
				source.CopyTo (dest);
			}

			//We dont want to delete the personal DB, so only create it if its not in the file system
			string myDbName = "personal.sqlite";
			string myDbPath = System.IO.Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), myDbName);

			if (!File.Exists (myDbPath)) {
				using (Stream source = Assets.Open(myDbName))
				using (var dest2 = System.IO.File.Create (myDbPath)) {
					source.CopyTo (dest2);
				}
			} 

			//Returns a list of hotspots (ID, Name, Longtitude, Latitude, Reward, Price, TimeDate)
			hotspotList = Hotspots.DoSomeDataAccess ();
			powerupList = Powerups.DoSomeDataAccess ();
			customizationList = Customizations.DoSomeDataAccess ();
			userList = User.DoSomeDataAccess ();
			myData = PersonalInfo.DoSomeDataAccess ();

			Console.WriteLine ("My current player is " + myData.Username +" and "+ myData.Customization);

			PersonalInfo.UpdateUser (myData, "green", 10000, "shield");
			myData = PersonalInfo.DoSomeDataAccess ();

			Console.WriteLine ("My current player is " + myData.Username +" and "+ myData.Customization);
			Console.WriteLine ("There are " +hotspotList.Count + " hotspots. There are" +  powerupList.Count + " powerups. There are" + customizationList.Count + "customizations. There are" + userList.Count + "users");

			_locMgr = GetSystemService (Context.LocationService) as LocationManager;

            _isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();

			SetContentView(Resource.Layout.MapWithOverlayLayout);

			_title = _drawerTitle = "Menu";
			_menuTitles = Resources.GetStringArray(Resource.Array.MenuItemNames);
			_drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			_drawerList = FindViewById<ListView>(Resource.Id.left_drawer);

			_drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);

			_drawerList.Adapter = new ArrayAdapter<string>(this,
				Resource.Layout.DrawerListItem, _menuTitles);
			_drawerList.ItemClick += (sender, args) => SelectItem(args.Position);


			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			//DrawerToggle is the animation that happens with the indicator next to the
			//ActionBar icon. You can choose not to use this.
			_drawerToggle = new MyActionBarDrawerToggle(this, _drawer,
				Resource.Drawable.ic_drawer_light,
				Resource.String.app_name,
				Resource.String.app_name);

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

			// **********  

			//activity = new SampleActivity(Resource.String.activity_label_mapwithoverlays, Resource.String.activity_description_mapwithoverlays, typeof(MapWithOverlaysActivity));

			//activity.Start(this);

			NativeCSS.StyleWithCSS("styles.css", new Uri("http://ugrad.bitdegree.ca/~andrewbrough/youreit/styles.css"), RemoteContentRefreshPeriod.EverySecond);
        }

		public void OnLocationChanged(Location location) {
			_currentLocation = location;
			Log.Debug(LocationTag, "{0}, {1}", location.Longitude, location.Latitude);

		}

		public void OnProviderDisabled(string provider) {
		}

		public void OnProviderEnabled(string provider) {
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras){
			Log.Debug(LocationTag, "{0}, {1}", provider, status);
		}

		protected override void OnPause() {
			base.OnPause();

			_map.MyLocationEnabled = false;
			_map.MarkerClick -= MapOnMarkerClick;
			_map.InfoWindowClick += HandleInfoWindowClick;

			_locMgr.RemoveUpdates(this);
			Log.Debug (LocationTag, "No longer listening for location updates.");

		}

		protected override void OnResume() {
			base.OnResume();
			_locMgr.RequestLocationUpdates (LocationManager.GpsProvider, 2000, 1, this);
			Log.Debug(LocationTag, "Listening for location updates using " + _locationProvider + ".");

			if (SetupMapIfNeeded())
			{
				_map.MyLocationEnabled = true;
				// Setup a handler for when the user clicks on a marker.
				_map.MarkerClick += MapOnMarkerClick;
			}
		}


		private void AddHotspotMarkersToMap()
		{
			BitmapDescriptor icon;
			CircleOptions circleOptions = new CircleOptions ();

			foreach (HotspotData hotspot in hotspotList) {

				switch (hotspot.Category)
				{
				case 1:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.cafe);
					break;
				case 2:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.resturant);
					break;
				case 3:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.bar);
					break;
				case 4:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.events);
					break;
				case 5:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.hotel);
					break;
				case 6:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.shop);
					break;
				case 7:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.tourism);
					break;
				default:
					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.events);
					break;
				}

				circleOptions.InvokeCenter(new LatLng (hotspot.Latitude, hotspot.Longitude));
				circleOptions.InvokeRadius (250);
				circleOptions.InvokeStrokeWidth (0);
				circleOptions.InvokeFillColor (Color.Argb(178,110,187,229));
				circleOptions.InvokeZIndex (1);
				_map.AddCircle(circleOptions);
				Marker marker;
				MarkerOptions markerOptions = new MarkerOptions ()
					.SetPosition (new LatLng (hotspot.Latitude, hotspot.Longitude))
					.InvokeIcon (icon)
					.Anchor(0.5f,0.5f)
					.InfoWindowAnchor(200,100)
					.SetSnippet(String.Format("Starbucks #{0}.", hotspot.ID))
					.SetTitle(String.Format("Starbucks {0}",  hotspot.ID));
				//_map.AddMarker(markerOptions);

				marker = _map.AddMarker(markerOptions);
				CustomGameMarker customMapMarker = new CustomGameMarker(marker.Id,"hotspot",hotspot.ID,true);

				markerList.Add(customMapMarker) ;

			}
		}

		private void AddUserMarkersToMap()
		{
			BitmapDescriptor icon;
			foreach (UserData m_user in userList) {
				string[] customization = m_user.Customization.Split (',');
				int location = Convert.ToInt32(customization [0]);
				CustomizationData custom = customizationList.ElementAt (location);


				string imgURL = string.Format("smallCustomizations/{0}",custom.ImgURL);
				Console.WriteLine ("------------------------------- "+imgURL);

				//string imgURL = "smallCustomizations/canada.jpg";
				//icon = BitmapDescriptorFactory.FromPath("cafe.png");
				icon = BitmapDescriptorFactory.FromAsset (imgURL);

				Marker marker;
				MarkerOptions userMarkers = new MarkerOptions ()
					.SetPosition (new LatLng (m_user.Latitude, m_user.Longitude))
					.InvokeIcon (icon)
					.Anchor(0.5f,0.5f)
					.InfoWindowAnchor(200,100)
					.SetSnippet(String.Format("User #{0}.", m_user.ID))
					.SetTitle(String.Format("User {0}",  m_user.ID));
				marker = _map.AddMarker(userMarkers);
				CustomGameMarker customMapMarker = new CustomGameMarker(marker.Id,"user",m_user.ID,true);

				markerList.Add(customMapMarker) ;

			}
		}

		private void HandleInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
		{
			CircleOptions circleOptions = new CircleOptions();
			circleOptions.InvokeRadius(100.0);
		}

		private void InitMapFragment()
		{
			_mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
			if (_mapFragment == null)
			{
					_map = null;
					GoogleMapOptions mapOptions = new GoogleMapOptions()
					.InvokeMapType(GoogleMap.MapTypeNormal)
					.InvokeZoomControlsEnabled(false)
					.InvokeCompassEnabled(true);

				_mapFragment = MapFragment.NewInstance(mapOptions);
				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, _mapFragment, "map")
					.Commit ();


			} 
		}

		private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			markerClickEventArgs.Handled = true;
			Marker marker = markerClickEventArgs.P0;
			foreach (CustomGameMarker customMarker1 in markerList) {

				if (marker.Id.Equals(customMarker1.markerID))
				{
					if (customMarker1.Type == "user") {
//						CircleOptions circleOptions = new CircleOptions ();
						Console.WriteLine ("---------------its a user");
						if (customMarker1.Click == true) {

							foreach (CustomGameMarker marker1 in markerList) {
								if (marker1.Click = false)
									marker1.Click = true;
							}
							if (circle1 != null) {
								circle1.Remove ();
								circle2.Remove ();
								circle3.Remove ();
							}
							circle1 = _map.AddCircle (new CircleOptions ()
								.InvokeCenter (new LatLng (userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude))
								.InvokeRadius (250)
								.InvokeStrokeWidth (0)
								.Visible (true)
								.InvokeFillColor (Color.Argb (102, 36, 120, 166)));
							circle2 = _map.AddCircle (new CircleOptions ()
								.InvokeCenter (new LatLng (userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude))
								.InvokeRadius (625)
								.InvokeStrokeWidth (0)
								.Visible (true)
								.InvokeFillColor (Color.Argb (102, 78, 156, 201)));
							circle3 = _map.AddCircle (new CircleOptions ()
								.InvokeCenter (new LatLng (userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude))
								.InvokeRadius (1000)
								.InvokeStrokeWidth (0)
								.Visible (true)
								.InvokeFillColor (Color.Argb (178, 110, 187, 229)));

							double distance = Measure (userList.ElementAt (Convert.ToInt32 (customMarker1.TypeID) - 1).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude, myData.Latitude, myData.Longitude);
							if (distance >= 1000) {//Console.WriteLine ("_______________You get NOTHING"+distance);
								Toast.MakeText (this, String.Format ("You Can't Tag This User"), ToastLength.Short).Show ();
							} else if (distance <= 1000 & distance >= 650) {
								Toast.MakeText (this, String.Format ("You Tag This User"), ToastLength.Short).Show ();
								PersonalInfo.UpdateUser (myData, null, myData.Points + 50, null);
							} else if (distance <= 650 & distance >= 250) {
								Toast.MakeText (this, String.Format ("You Tag This User"), ToastLength.Short).Show ();
								PersonalInfo.UpdateUser (myData, null, myData.Points + 100, null);
							} else if (distance <= 250) {
								PersonalInfo.UpdateUser (myData, null, myData.Points + 200, null);
								Toast.MakeText (this, String.Format ("You Tag This User"), ToastLength.Short).Show ();
							}
							customMarker1.Click = false;

						} else {
							customMarker1.Click = true;
							circle1.Remove();
							circle2.Remove();
							circle3.Remove();
						}
					}
				}
			}

			//	PositionPolarBearGroundOverlay(InMaui);
			//		_map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(InMaui, 13));

			//Toast.MakeText(this, String.Format("Starbucks {0}", marker.Id), ToastLength.Short).Show();

		}

		private bool SetupMapIfNeeded()
		{
			if (_map == null)
			{
				_map = _mapFragment.Map;
				if (_map != null)
				{
					AddHotspotMarkersToMap();
					AddUserMarkersToMap();
					CameraPosition cameraPosition = new CameraPosition.Builder()
						.Target(new LatLng(45.403208, -75.688433))      // Sets the center of the map to Mountain View
						.Zoom(14)                   // Sets the zoom
						//.Bearing(90)              // Sets the orientation of the camera to east
						.Build();  
					// Animate the move on the map so that it is showing the markers we added above.
					_map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition)); 

					return true;
				}
				return false;
			}
			return true;
		}


		private void SelectItem(int position){

			switch (position) {
			case 0:

				InitMapFragment ();
				SetupMapIfNeeded ();

	
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
				var profifragment = new CustomizeFragment ();
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

			switch(item.ItemId){
				case 2131099687:
					SelectItem (0);
					break;
				case 2131099688:
					SelectItem (1);
					break;
				case 0:
					SelectItem(5);
					break;
			}


			Android.Widget.Toast.MakeText (this, 
				"Selected Item: " + 
				item.ItemId, 
				Android.Widget.ToastLength.Short).Show();
			return true;
		}
        private bool TestIfGooglePlayServicesIsInstalled()
		{
			int queryResult = GooglePlayServicesUtil.IsGooglePlayServicesAvailable (this);
			if (queryResult == ConnectionResult.Success) {
				Log.Info (Tag, "Google Play Services is installed on this device.");
				return true;
			}

			if (GooglePlayServicesUtil.IsUserRecoverableError (queryResult)) {
				string errorString = GooglePlayServicesUtil.GetErrorString (queryResult);
				Log.Error (Tag, "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
				Dialog errorDialog = GooglePlayServicesUtil.GetErrorDialog (queryResult, this, InstallGooglePlayServicesId);
				ErrorDialogFragment dialogFrag = new ErrorDialogFragment (errorDialog);

				dialogFrag.Show (FragmentManager, "GooglePlayServicesDialog");
			}
			return false;
		}

		public double Measure(Double lat1, Double lon1, Double lat2, Double lon2) {
			var R = 6378.137; // Radius of earth in KM
			var dLat = (lat2 - lat1) * Math.PI / 180;
			var dLon = (lon2 - lon1) * Math.PI / 180;
			var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
			Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
			        Math.Sin(dLon/2) * Math.Sin(dLon/2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
			var d = R * c;
			return d * 1000; // meters

		}
    }
}
