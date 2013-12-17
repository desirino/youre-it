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
			var view = inflater.Inflate(Resource.Layout.CustomizationItem, p1, false);

			List<CustomizationData> customizationList = Customizations.DoSomeDataAccess ();
			LinearLayout customization1 = view.FindViewById<LinearLayout> (Resource.Id.customization_container1);

			var textView = view.FindViewById<LinearLayout>(Resource.Id.customization_container1);

			foreach (CustomizationData c in customizationList) {

				Button btn = new Button (this.Activity);
				btn.Text = c.Name;
				textView.AddView (btn);
			}

			

			Console.WriteLine ("------------CUSTOMIZE FRAGMENT LOADED------------");
			return view;

		}


	}
}
