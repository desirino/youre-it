namespace youreit
{
    using System.Collections.Generic;

    using Android.App;
    using Android.Content;
    using Android.Gms.Common;
    using Android.OS;
    using Android.Util;
    using Android.Views;
    using Android.Widget;

    using AndroidUri = Android.Net.Uri;

	// **********  ADDED FOR MAP SCREEN
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using System;
	// **********  
	
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static readonly int InstallGooglePlayServicesId = 1000;
        public static readonly string Tag = "You're It Map Testing";

		//private SampleActivity activity;
		private bool _isGooglePlayServicesInstalled;

		// **********  ADDED FOR MAP SCREEN
		private static readonly LatLng[] HotspotLocations = new[]
		{
			new LatLng(45.393435, -75.683029),
			new LatLng(45.401843, -75.710067),
			new LatLng(45.416081, -75.688838),
			new LatLng(45.420254, -75.649044),
			new LatLng(45.421869, -75.698430),
			new LatLng(45.413712, -75.710656),
			new LatLng(45.421073, -75.695847)
		};
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
            _isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();
			//InitializeListView();

			// **********  ADDED FOR MAP SCREEN

			SetContentView(Resource.Layout.MapWithOverlayLayout);

			InitMapFragment();
			SetupMapIfNeeded();
			// **********  

			//activity = new SampleActivity(Resource.String.activity_label_mapwithoverlays, Resource.String.activity_description_mapwithoverlays, typeof(MapWithOverlaysActivity));

			//activity.Start(this);
        }
		// **********  ADDED FOR MAP SCREEN

		protected override void OnPause()
		{
			base.OnPause();

			// Pause the GPS - we won't have to worry about showing the 
			// location.
			_map.MyLocationEnabled = false;

			_map.MarkerClick -= MapOnMarkerClick;

			_map.InfoWindowClick += HandleInfoWindowClick;
		}

		protected override void OnResume()
		{
			base.OnResume();
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
			for (int i = 0; i < HotspotLocations.Length; i++)
			{
				BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.cafe1);
				MarkerOptions markerOptions = new MarkerOptions ()
					.SetPosition (HotspotLocations [i])
					.InvokeIcon (icon)
					.InfoWindowAnchor(200,100)
					.SetSnippet(String.Format("Starbucks #{0}.", i))
					.SetTitle(String.Format("Starbucks {0}", i));
				_map.AddMarker(markerOptions);
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
			/*if (marker.Id.Equals(_gotoMauiMarkerId))
			{
				PositionPolarBearGroundOverlay(InMaui);
				_map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(InMaui, 13));
				_gotoMauiMarkerId = null;
				_polarBearMarker.Remove();
				_polarBearMarker = null;
			}
			else
			{*/
				Toast.MakeText(this, String.Format("Starbucks {0}", marker.Id), ToastLength.Short).Show();
			//}
		}

		private bool SetupMapIfNeeded()
		{
			if (_map == null)
			{
				_map = _mapFragment.Map;
				if (_map != null)
				{
					AddHotspotMarkersToMap();
					CameraPosition cameraPosition = new CameraPosition.Builder()
						.Target(new LatLng(45.393435, -75.683029))      // Sets the center of the map to Mountain View
						.Zoom(14)                   // Sets the zoom
						//.Bearing(90)                // Sets the orientation of the camera to east
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
            int queryResult = GooglePlayServicesUtil.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info(Tag, "Google Play Services is installed on this device.");
                return true;
            }

            if (GooglePlayServicesUtil.IsUserRecoverableError(queryResult))
            {
                string errorString = GooglePlayServicesUtil.GetErrorString(queryResult);
                Log.Error(Tag, "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
                Dialog errorDialog = GooglePlayServicesUtil.GetErrorDialog(queryResult, this, InstallGooglePlayServicesId);
                ErrorDialogFragment dialogFrag = new ErrorDialogFragment(errorDialog);

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }
            return false;
        }
    }
}
