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
namespace youreit
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
	public class LoginActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ActionBar.Hide ();

			SetContentView (Resource.Layout.LoginScreen);
			NativeCSS.StyleWithCSS("loginStyles.css", new Uri("http://ugrad.bitdegree.ca/~andrewbrough/youreit/loginStyles.css"), RemoteContentRefreshPeriod.EverySecond);

			var editText = FindViewById<EditText> (Resource.Id.editText);
			var joinGame = FindViewById<Button> (Resource.Id.joinGame);


			joinGame.Click += (sender, e) => {           
				var main = new Intent(this, typeof(MainActivity));
				main.PutExtra("PlayerName", editText.Text);
				StartActivity (main);
            };		
		}
	}
}

