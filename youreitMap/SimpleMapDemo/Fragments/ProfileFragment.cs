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

namespace youreit
{
	class ProfileFragment : Fragment
	{

		public override View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
		{
			var view = p0.Inflate(Resource.Layout.ProfileScreen, p1, false);

			var tagsButton = view.FindViewById<Button> (Resource.Id.button_Tags);
			var taggedButton =  view.FindViewById<Button> (Resource.Id.button_Tagged);
			var pointButton = view.FindViewById<Button> (Resource.Id.button_Points);
			var customize = view.FindViewById<Button> (Resource.Id.button_customize);

			PersonalInfoData myData = PersonalInfo.getCurrentUserData (((MainActivity)this.Activity).activeUsername);

			tagsButton.Text = myData.TaggedCount + "Tagged";
			//taggedButton.Text = myData.Tags + "Tags";
			pointButton.Text = "(P)" + myData.Points.ToString ();


			Console.WriteLine ("------------PROFILE FRAGMENT------------");
			return view;

		}


	}
}

