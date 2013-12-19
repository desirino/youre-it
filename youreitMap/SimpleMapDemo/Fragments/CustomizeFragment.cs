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

			var ownedCustomizations = view.FindViewById<LinearLayout>(Resource.Id.customization_container1);
			//var notOwnedCustomizations = view.FindViewById<LinearLayout>(Resource.Id.customization_container2);

			foreach (CustomizationData c in customizationList) {

				ImageButton btn = new ImageButton (this.Activity);
//				btn.SetMinimumWidth (20);
//				btn.SetMinimumHeight (20);
//				btn.SetMaxWidth (20);
//				btn.SetMaxHeight (20);
				char[] end = {'.','p','n','g'};
				string imgName = c.ImgURL.TrimEnd(end);
				//Console.WriteLine ("--------------------" + imgName + Resources.GetIdentifier (imgName, "drawable", this.Activity.PackageName));

				btn.SetBackgroundResource (Resources.GetIdentifier (imgName, "drawable", this.Activity.PackageName));
				ownedCustomizations.AddView (btn);
				//notOwnedCustomizations.AddView();
			}

			

			Console.WriteLine ("------------CUSTOMIZE FRAGMENT LOADED------------");
			return view;

		}


	}
}
