using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class UserData
	{

		public UserData(int ID, string Username, int TaggedCount, int Points, Double Latitude, Double Longitude, string Powerups, string Customization, string Hotspots)
		{
			this.ID = ID;
			this.Username = Username;
			this.TaggedCount = TaggedCount;
			this.Points = Points;
			this.Longitude = Longitude;
			this.Latitude = Latitude;
			this.PowerUps = PowerUps;
			this.Customization = Customization;
			this.Hotspots = Hotspots;
		}

		public int ID { get; set; }
		public string Username { get; set;} 
		public int TaggedCount { get; set;}
		public int Points { get; set;}
		public Double Longitude { get; set; }
		public Double Latitude { get; set; }

		//Stores ids of all of the following. 
		public string PowerUps { get; set;} 
		public string Customization { get; set; }
		public string Hotspots { get; set; }
	}

	public class User
	{
		public static SqliteConnection connection;

		public static List<UserData>  DoSomeDataAccess ()
		{
			List<UserData> userList = new List<UserData>(); 
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Users]";
				var r = contents.ExecuteReader ();
				while (r.Read ())
					userList.Add(new UserData(
						Convert.ToInt32(r["ID"]), r ["Username"].ToString(),
						Convert.ToInt32(r["TaggedCount"]), Convert.ToInt32(r["Points"]),
						Convert.ToDouble(r["Latitude"]), Convert.ToDouble(r["Longitude"]),
						r ["Powerups"].ToString(), r ["Customization"].ToString(), r ["Hotspots"].ToString()

					));

			}
			connection.Close ();
			return userList;
		}

		public bool UpdateUser() {

			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Users]";
				var r = contents.ExecuteReader ();

			}
			connection.Close ();

			return false;
		}
	}
}
	
