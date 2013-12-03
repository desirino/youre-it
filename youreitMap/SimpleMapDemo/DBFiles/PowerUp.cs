using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class PowerupData
	{

		public PowerupData(int ID, string Name,int Edition, string ImgUrl, Double Price )
		{
			this.ID = ID;
			this.Name = Name;
			this.ImgUrl = ImgUrl;
			this.Edition = Edition;
			this.Price = Price;
		}

		public int ID { get; set; }
		public string Name { get; set;}
		public string ImgUrl { get; set;}
		public int Edition { get; set; }
		public Double Price { get; set; }

	}

	public class Powerups
	{
		public static SqliteConnection connection;


		public static List<PowerupData>  DoSomeDataAccess ()
		{
			List<PowerupData> powerupList = new List<PowerupData>(); 

			string dbName = "youreit.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Powerups]";
				var r = contents.ExecuteReader ();

				while (r.Read ())
					powerupList.Add(new PowerupData(
						Convert.ToInt32(r["ID"]), r ["Name"].ToString(),
						Convert.ToInt32(r ["Edition"]), r["ImgUrl"].ToString(), Convert.ToInt32(r ["Price"])
					));
			}
			connection.Close ();

			return powerupList;
		}


	}
}