using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class UserData
	{

		public UserData(int ID, string Username, int TaggedCount, int Points, Double Longitude, Double Latitude, string Powerups, string Customization, string Hotspots)
		{
			this.ID = ID;
			this.Username = Username;
			this.TaggedCount = TaggedCount;
			this.Points = Points;
			this.Longitude = Longitude;
			this.Latitude = Latitude;
			this.PowerUps = PowerUps;
			this.Customizations = Customization;
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
		public string Customizations { get; set; }
		public string Hotspots { get; set; }
	}

	public class User
	{
		public static SqliteConnection connection;

		public static List<UserData>  DoSomeDataAccess ()
		{
			List<UserData> userList = new List<UserData>(); 

			string dbName = "youreit.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.InitializeLifetimeService ();
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [User]";
				var r = contents.ExecuteReader ();

				while (r.Read ()) {

					userList.Add (new UserData (
						Convert.ToInt32 (r ["ID"]), r ["Username"].ToString (),
						//Convert.ToInt32(r["Tagged"]), Convert.ToInt32(r["Points"]),
						Convert.ToInt32 (r ["Tagged"]), 100,
						Convert.ToDouble (r ["Longitude"]), Convert.ToDouble (r ["Latitude"]), 
						r ["Powerups"].ToString (), r ["Customizations"].ToString (), r ["Hotspots"].ToString ()
					));
				}
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
	
