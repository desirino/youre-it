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

namespace youreitUIv2
{
	class PowerupsFragment : Fragment
	{

		public override View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
		{
			var rootView = p0.Inflate(Resource.Layout.PowerupsScreen, p1, false);


			Console.WriteLine ("------------POWERUPS FRAGMENT------------");
			return rootView;

		}


	}
}

