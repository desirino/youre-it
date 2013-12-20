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
		
			PersonalInfoData myData = PersonalInfo.getCurrentUserData (((MainActivity)this.Activity).activeUsername);
			string[] myCustomizations = myData.Customization.Split (',');

			var ownedCustomizations = view.FindViewById<LinearLayout>(Resource.Id.customization_container1);
			var allCustomizations = view.FindViewById<LinearLayout>(Resource.Id.customization_container2);

			//Add buttons for all customizations
			foreach (CustomizationData c in customizationList) {
				bool makeButton = true;

				foreach (string id in myCustomizations) {
					if (c.ID-2 == Convert.ToInt32(id)) {
						Console.WriteLine ("BLOCK CUSTOMIZATION BUTTON----------------------------id: " + id + ", c.ID: " + c.ID);
						makeButton = false;
					}
				}
				if (makeButton) {
					LinearLayout container = new LinearLayout (this.Activity);
					container.SetGravity (GravityFlags.Center);
					container.Orientation = Android.Widget.Orientation.Vertical;

					ImageButton btn = new ImageButton (this.Activity);

					string[] imgName = c.ImgURL.Split ('.');
					btn.SetBackgroundResource (Resources.GetIdentifier (imgName [0].ToLower (), "drawable", this.Activity.PackageName));

					//price stuff
					TextView price = new TextView (this.Activity);
					price.Text = "(P)" + Convert.ToString(c.Price);
					price.Gravity = GravityFlags.Center;
					price.SetPadding (10, 10, 10, 10);
					price.SetTextSize (ComplexUnitType.Pt, 10);;

					//button event
					btn.Click += (sender, e) => {
						//check if user has enough points to buy customization, if so equip and subtract points, else toast "not enough points"
						if (myData.Points > c.Price) {
							string allCustomization =  myData.Customization + "," + Convert.ToString (c.ID-2);
							customizationList.Remove(c);
							PersonalInfo.UpdateUser (myData, allCustomization, myData.Points - (int)c.Price, null, Convert.ToString (c.ID-2));
							Android.Widget.Toast.MakeText (this.Activity, "Customization " + c.Name + " was bought", Android.Widget.ToastLength.Short).Show ();
							((MainActivity)this.Activity).SelectItem(2);
						} else {
							Android.Widget.Toast.MakeText (this.Activity, "Not enough points", Android.Widget.ToastLength.Short).Show ();
						}
					};

					container.AddView(btn);
					container.AddView (price);
					allCustomizations.AddView (container);
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
//				Console.WriteLine ("--------------------" + imgName[0].ToLower() + Resources.GetIdentifier (imgName[0].ToLower(), "drawable", this.Activity.PackageName));
				btn.SetBackgroundResource (Resources.GetIdentifier (imgName[0].ToLower(), "drawable", this.Activity.PackageName));

				btn.Click += (sender, e) => {

					PersonalInfo.UpdateUser(myData,null,myData.Points,null, Convert.ToString(c.ID));
					Android.Widget.Toast.MakeText (this.Activity, "Customization changed to: " + c.Name, Android.Widget.ToastLength.Short).Show();

				};

				ownedCustomizations.AddView (btn);
			}

			return view;

		}


	}
}
