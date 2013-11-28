using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class PowerupData
	{

		public PowerupData(int ID, string Name, Double Longitude, Double Latitude, int Edition, Double Price)
		{
			this.ID = ID;
			this.Name = Name;
			this.Longitude = Longitude;
			this.Latitude = Latitude;
			this.Price = Price;
			this.Edition = Edition;
		}

		public int ID { get; set; }
		public string Name { get; set;}
		public Double Price { get; set;}
		public int Edition { get; set; }
		public Double Longitude { get; set; }
		public Double Latitude { get; set; }

	}

	public class Powerups
	{
		public static SqliteConnection connection;


		public static List<PowerupData>  DoSomeDataAccess ()
		{
			List<PowerupData> powerupList = new List<PowerupData>(); 

			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();

			// query the database to prove data was inserted!

			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Powerups]";
				var r = contents.ExecuteReader ();
				while (r.Read ())
					powerupList.Add(new PowerupData(
						Convert.ToInt32(r["ID"]), r ["Name"].ToString(),
						Convert.ToDouble(r["Longitude"]),Convert.ToDouble(r["Latitude"]), 
						Convert.ToInt32(r ["Edition"]), Convert.ToDouble(r["Price"])
					));
			}
			connection.Close ();

			return powerupList;
		}


	}
}