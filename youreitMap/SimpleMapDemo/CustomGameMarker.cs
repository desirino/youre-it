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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;

namespace youreit
{
	public class CustomGameMarker
	{
		public CustomGameMarker(string markerID, string Type, int TypeID, Boolean Click)
		{
			this.markerID = markerID;
			this.Type = Type;
			this.TypeID = TypeID;
			this.Click = Click;
		}

		public string markerID{ get; set; }
		public string Type{ get; set; }
		public int TypeID{ get; set; } 
		public Boolean Click{ get; set; } 

	}
}

