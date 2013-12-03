using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class HotspotData
	{

		public HotspotData(int ID, string Name, int Category, Double Latitude, Double Longitude, int Reward, string Time)
		{
			this.ID = ID;
			this.Name = Name;
			this.Category = Category;
			this.Latitude = Latitude;
			this.Longitude = Longitude;
			this.Reward = Reward;
			this.Time = Time;
		}

		public int ID { get; set; }
		public string Name { get; set;}
		public int Category { get; set; }
		public int Reward { get; set;} 
		public string Time { get; set; }
		public Double Price { get; set;}
		public Double Latitude { get; set; }
		public Double Longitude { get; set; }
	}

	public class Hotspots
	{
		public static SqliteConnection connection;


		public static List<HotspotData>  DoSomeDataAccess ()
		{
			List<HotspotData> hotspotList = new List<HotspotData>(); 

			string dbName = "youreit.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);
	

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Hotspots]";
				var r = contents.ExecuteReader ();

				while (r.Read ()) {
					Console.Write (r);
					hotspotList.Add (new HotspotData (
						Convert.ToInt32 (r ["ID"]), r ["Name"].ToString (), Convert.ToInt32 (r ["Category"]),
						Convert.ToDouble (r ["Latitude"]), Convert.ToDouble (r ["Longitude"]), 
						Convert.ToInt32 (r ["Reward"]), r ["Time"].ToString ()
					));
				}
			}
			connection.Close ();

			return hotspotList;
		}


	}
}