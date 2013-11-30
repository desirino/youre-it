namespace youreit
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;
    using Android.App;
    using Android.Content;
    using Android.Gms.Common;
    using Android.OS;
    using Android.Util;
    using Android.Views;
    using Android.Widget;
	using Android.Locations;

    using AndroidUri = Android.Net.Uri;

	// **********  ADDED FOR MAP SCREEN
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using Android.Graphics;

	// **********  

	/// <summary>
	/// ADDED FOR DB MANAGEMENT 
	/// </summary>
	using SQLite;
	using Environment = System.Environment;
	
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
		public CustomGameMarker customMapMarker = new CustomGameMarker("0","0",0,true);
	
		//Lists where all the data is stored 
		private List<HotspotData> hotspotList;
		private List<CustomizationData> customizationList;
		private List<PowerupData> powerupList;
		private List<UserData> userList;

		private GoogleMap _map;
		private MapFragment _mapFragment;
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
			SQLite3.Config (SQLite3.ConfigOption.Serialized);

			//Returns a list of hotspots (ID, Name, Longtitude, Latitude, Reward, Price, TimeDate)
			hotspotList = Hotspots.DoSomeDataAccess ();
			powerupList = Powerups.DoSomeDataAccess ();
			customizationList = Customizations.DoSomeDataAccess ();
			userList = User.DoSomeDataAccess ();
			Console.WriteLine ("---- " +hotspotList + "---" +  powerupList + "---" + customizationList + userList);
			//hotspotList.ForEach(item => Console.Write("\n------ " + item.Name));

			_locMgr = GetSystemService (Context.LocationService) as LocationManager;

            _isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();
			//InitializeListView();

			// **********  ADDED FOR MAP SCREEN

			SetContentView(Resource.Layout.MapWithOverlayLayout);

			//Start up location Manager to get long and lat.
			InitMapFragment();
			SetupMapIfNeeded();
			// **********  

			//activity = new SampleActivity(Resource.String.activity_label_mapwithoverlays, Resource.String.activity_description_mapwithoverlays, typeof(MapWithOverlaysActivity));

			//activity.Start(this);


        }

		// **********  ADDED FOR MAP SCREEN
		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			Log.Debug(LocationTag, "{0}, {1}", location.Longitude, location.Latitude);

		}

		public void OnProviderDisabled(string provider)
		{
		}

		public void OnProviderEnabled(string provider)
		{
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			Log.Debug(LocationTag, "{0}, {1}", provider, status);
		}

		protected override void OnPause()
		{
			base.OnPause();

			// Pause the GPS - we won't have to worry about showing the 
			// location.
			_map.MyLocationEnabled = false;

			_map.MarkerClick -= MapOnMarkerClick;

			_map.InfoWindowClick += HandleInfoWindowClick;

			_locMgr.RemoveUpdates(this);
			Log.Debug(LocationTag, "No longer listening for location updates.");


		}

		protected override void OnResume()
		{
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

		/// <summary>
		///   Add three markers to the map.
		/// </summary>
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
				customMapMarker.markerID = marker.Id;
				customMapMarker.Type = "hotspot";
				customMapMarker.TypeID = hotspot.ID;
				markerList.Add(customMapMarker) ;
			}
		}


		private void AddUserMarkersToMap()
		{
			BitmapDescriptor icon;

			foreach (UserData user in userList) {

					icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.cafe);
				//icon = BitmapDescriptorFactory.FromResource(customization.url);
				Marker marker;
				MarkerOptions userMarkers = new MarkerOptions ()
					.SetPosition (new LatLng (user.Latitude, user.Longitude))
					.InvokeIcon (icon)
					.Anchor(0.5f,0.5f)
					.InfoWindowAnchor(200,100)
					.SetSnippet(String.Format("User #{0}.", user.ID))
					.SetTitle(String.Format("User {0}",  user.ID));
				marker = _map.AddMarker(userMarkers);
				customMapMarker.markerID = marker.Id;
				customMapMarker.Type = "user";
				customMapMarker.TypeID = user.ID;
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
				GoogleMapOptions mapOptions = new GoogleMapOptions()
					.InvokeMapType(GoogleMap.MapTypeNormal)
					.InvokeZoomControlsEnabled(false)
					.InvokeCompassEnabled(true);

				FragmentTransaction fragTx = FragmentManager.BeginTransaction();
				_mapFragment = MapFragment.NewInstance(mapOptions);
				fragTx.Add(Resource.Id.mapWithOverlay, _mapFragment, "map");
				fragTx.Commit();
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
		

						CircleOptions circleOptions = new CircleOptions ();

						if (customMarker1.Click==true) {

							circleOptions.InvokeCenter (new LatLng (userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude));
							circleOptions.InvokeRadius (250);
							circleOptions.InvokeStrokeWidth (0);
							circleOptions.InvokeFillColor (Color.Argb (102, 36, 120, 166));
							circleOptions.InvokeZIndex (3);
							_map.AddCircle (circleOptions);
							circleOptions.InvokeCenter (new LatLng (userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude));
							circleOptions.InvokeRadius (625);
							circleOptions.InvokeStrokeWidth (0);
							circleOptions.InvokeFillColor (Color.Argb (102, 78, 156, 201));
							circleOptions.InvokeZIndex (2);
							_map.AddCircle (circleOptions);
							circleOptions.InvokeCenter (new LatLng (userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Latitude, userList.ElementAt ((Convert.ToInt32 (customMarker1.TypeID) - 1)).Longitude));
							circleOptions.InvokeRadius (1000);
							circleOptions.InvokeStrokeWidth (0);
							circleOptions.InvokeFillColor (Color.Argb (178, 110, 187, 229));
							circleOptions.InvokeZIndex (1);
							circleOptions.Visible (true);

							_map.AddCircle (circleOptions);

							customMarker1.Click = false;

						} else {
							circleOptions.Visible (false);
						
						}
					}
				}
			}

			//	PositionPolarBearGroundOverlay(InMaui);
			//		_map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(InMaui, 13));

				Toast.MakeText(this, String.Format("Starbucks {0}", marker.Id), ToastLength.Short).Show();

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
		// **********  


		/*
		// COULD REMOVE :D
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            if (position == 0)
            {
                AndroidUri geoUri = AndroidUri.Parse("geo:42.374260,-71.120824");
                Intent mapIntent = new Intent(Intent.ActionView, geoUri);
                StartActivity(mapIntent);
                return;
            }

            SampleActivity activity = _activities[position];
            activity.Start(this);
        }

*/
		
		/*
		// COULD REMOVE :D
        private void InitializeListView()
        {
            if (_isGooglePlayServicesInstalled)
            {
                _activities = new List<SampleActivity>
                      {
                          //new SampleActivity(Resource.String.mapsAppText, Resource.String.mapsAppTextDescription, null),
                          //new SampleActivity(Resource.String.activity_label_axml, Resource.String.activity_description_axml, typeof(BasicDemoActivity)),
                          new SampleActivity(Resource.String.activity_label_mapwithmarkers, Resource.String.activity_description_mapwithmarkers, typeof(MapWithMarkersActivity)),
                          new SampleActivity(Resource.String.activity_label_mapwithoverlays, Resource.String.activity_description_mapwithoverlays, typeof(MapWithOverlaysActivity))
                      };

                ListAdapter = new SimpleMapDemoActivityAdapter(this, _activities);
            }
            else
            {
                Log.Error("MainActivity", "Google Play Services is not installed");
                ListAdapter = new SimpleMapDemoActivityAdapter(this, null);
            }
        }
		*/

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
    }
}
