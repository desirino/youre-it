using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class PersonalInfoData
	{
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

	public class PersonalInfo
	{
		public static SqliteConnection connection;

		public static PersonalInfoData DoSomeDataAccess () {

			PersonalInfoData myData = new PersonalInfoData(); 
			string dbName = "personal.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.InitializeLifetimeService ();
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [User]";
				var r = contents.ExecuteReader ();

				while (r.Read ()) {
					myData.ID = Convert.ToInt32 (r ["ID"]);
					myData.Username = r ["Username"].ToString ();
					myData.TaggedCount = Convert.ToInt32 (r ["TaggedCount"]);
					myData.Points = Convert.ToInt32 (r ["Points"]);
					myData.Longitude = Convert.ToDouble (r ["Longitude"]);
					myData.Latitude = Convert.ToDouble (r ["Latitude"]);
					myData.PowerUps = r ["Powerups"].ToString ();
					myData.Customization = r ["Customizations"].ToString();
					myData.Hotspots = r ["Hotspots"].ToString ();

				}
			}
			connection.Close ();
			return myData;
		}

		public static bool UpdateUser(PersonalInfoData personal, string customization, int points, string powerup ) {
			string dbName = "personal.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {

				var commands = new[] {
					"UPDATE [USER] SET Customizations='"+ customization +"' WHERE ID=" + personal.ID,
					"UPDATE [USER] SET Powerups='"+ powerup +"'WHERE ID=" + personal.ID,
					"UPDATE [USER] SET Points='"+ points +"' WHERE ID=" + personal.ID
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
					}
				}
			}
			connection.Close ();
			return false;
		}
	}
}

