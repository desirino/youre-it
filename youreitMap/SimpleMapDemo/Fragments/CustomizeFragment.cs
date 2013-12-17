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

		public override View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
		{
			var rootView = p0.Inflate(Resource.Layout.CustomizeScreen, p1, false);

			List<CustomizationData> customizationList = Customizations.DoSomeDataAccess ();
//			ArrayAdapter adapter = new ArrayAdapter (rootView, Resource.Layout.CustomizationItem, customizationList);
			LinearLayout customization1 = rootView.FindViewById<LinearLayout> (Resource.Id.customization_container1);

			foreach (CustomizationData c in customizationList) {
				Console.WriteLine (c);
//				Button b = new Button (this);
//				b.SetBackgroundResource (Resource.Drawable.cafe);
//				customization1.AddView (b);
//				customization1 = (LinearLayout)View.Inflate (View, Resource.Layout.CustomizationItem, null);

			}

			Console.WriteLine ("------------CUSTOMIZE FRAGMENT LOADED------------");
			return rootView;

		}


	}
}

