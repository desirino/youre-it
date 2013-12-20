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

namespace youreit
{
	class CustomizeFragment : Fragment
	{

		public override View OnCreateView(LayoutInflater inflater, ViewGroup p1, Bundle p2)
		{
			var view = inflater.Inflate(Resource.Layout.CustomizeScreen, p1, false);
			List<CustomizationData> customizationList = Customizations.DoSomeDataAccess ();

	
			PersonalInfoData myData = PersonalInfo.DoSomeDataAccess ();
			string[] myCustomizations = myData.Customization.Split (',');

			var ownedCustomizations = view.FindViewById<LinearLayout>(Resource.Id.customization_container1);
			var allCustomizations = view.FindViewById<LinearLayout>(Resource.Id.customization_container2);

			//Add buttons for all customizations
			foreach (CustomizationData c in customizationList) {
				bool makeButton = false;

				foreach (string id in myCustomizations) {
					//Console.WriteLine ("----------------------------id: " + id + ", c.ID: " + c.ID);
					if (c.ID == Convert.ToInt32(id)) {
						Console.WriteLine ("----------------------------id: " + id + ", c.ID: " + c.ID);
						makeButton = true;
					}
				}
				if (!makeButton) {
					ImageButton btn = new ImageButton (this.Activity);

					string[] imgName = c.ImgURL.Split ('.');
					Console.WriteLine ("--------------------" + imgName [0].ToLower () + Resources.GetIdentifier (imgName [0].ToLower (), "drawable", this.Activity.PackageName));
					btn.SetBackgroundResource (Resources.GetIdentifier (imgName [0].ToLower (), "drawable", this.Activity.PackageName));

					//button event
					btn.Click += (sender, e) => {
						//check if user has enough points to buy customization, if so equip and subtract points, else toast "not enough points"
						if (myData.Points > c.Price) {
							PersonalInfo.UpdateUser (myData, null, myData.Points, null, Convert.ToString (c.ID));
							Android.Widget.Toast.MakeText (this.Activity, "Customization " + c.Name + " was bought", Android.Widget.ToastLength.Short).Show ();
						} else {
							Android.Widget.Toast.MakeText (this.Activity, "Not enough points", Android.Widget.ToastLength.Short).Show ();
						}
					};

					allCustomizations.AddView (btn);
				}
			}

			//add buttons for owned items.
			for (int i = 0; i<myCustomizations.Length; i++) {

				int location = Convert.ToInt32(myCustomizations [i]);
				CustomizationData c = customizationList.ElementAt (location);
				//Console.WriteLine (myCustomizations[i]);

				//button stuff
				ImageButton btn = new ImageButton (this.Activity);
				string[] imgName = c.ImgURL.Split('.');
				Console.WriteLine ("--------------------" + imgName[0].ToLower() + Resources.GetIdentifier (imgName[0].ToLower(), "drawable", this.Activity.PackageName));
				btn.SetBackgroundResource (Resources.GetIdentifier (imgName[0].ToLower(), "drawable", this.Activity.PackageName));

				btn.Click += (sender, e) => {
					customizationList.Add(c);
					string s = "";
					foreach(CustomizationData a in customizationList){
						s += "," + Convert.ToString(a.ID);
					}
					Console.WriteLine(s);
					PersonalInfo.UpdateUser(myData,s,myData.Points,null,null);
					Android.Widget.Toast.MakeText (this.Activity, "Customization changed to: " + c.Name, Android.Widget.ToastLength.Short).Show();

				};

				ownedCustomizations.AddView (btn);
			}

			Console.WriteLine ("------------CUSTOMIZE FRAGMENT LOADED------------");
			return view;

		}


	}
}
