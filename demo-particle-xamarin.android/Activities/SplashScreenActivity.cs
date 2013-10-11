// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH">
//   Exit Games GmbH, 2012
// </copyright>
// <summary>
//   The "Particle" demo is a load balanced and Photon Cloud compatible "coding" demo.
//   The focus is on showing how to use the Photon features without too much "game" code cluttering the view.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------
using System.Threading;
using Android.App;
using Android.OS;
using Android.Content.PM;

namespace DemoParticle.Xamarin.Android
{

    

    [Activity(Theme = "@style/Theme.Splash", Label = "Particle Demo", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Thread.Sleep(2000); 
            StartActivity(typeof(MainActivity));
        }
    }
}
