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
		public string CurrentCustomization {get; set;} 
		public string Hotspots { get; set; }
		public int Tags { get; set; }
	}

	public class storedPersonalInfoData {

		public storedPersonalInfoData(int ID, string Name, Double Latitude, Double Longitude, int TaggedCount, int Points, string Powerups, string Customization,  string currentCustomization, string Hotspots, int tags)
			{
				this.ID = ID;
				this.Username = Name;
				this.Latitude = Latitude;
				this.Longitude = Longitude;
				this.TaggedCount = TaggedCount;
				this.Points = Points;
				this.PowerUps = Powerups;
				this.Customization = Customization;
				this.CurrentCustomization = CurrentCustomization;
				this.Hotspots = Hotspots;
				this.Tags = tags;
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
			public string CurrentCustomization {get; set;} 
			public string Hotspots { get; set; }
			public int Tags {get; set;}
		}


	public class PersonalInfo
	{
		public static SqliteConnection connection;

		public static List<storedPersonalInfoData> accessAllStoredData () {

			List<storedPersonalInfoData> storedUsers = new List<storedPersonalInfoData>(); 
			string dbName = "personal.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.InitializeLifetimeService ();
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [User]";
				var r = contents.ExecuteReader ();

				while (r.Read ()) {
					Console.Write (r);
					storedUsers.Add (new storedPersonalInfoData (
						Convert.ToInt32 (r ["ID"]), r ["Username"].ToString (),
						Convert.ToDouble (r ["Latitude"]), Convert.ToDouble (r ["Longitude"]), 
						Convert.ToInt32 (r ["TaggedCount"]),Convert.ToInt32 (r ["Points"]),
						r ["Powerups"].ToString(), r ["Customizations"].ToString (),
						r ["CurrentCustomization"].ToString(), r ["Hotspots"].ToString (),
						Convert.ToInt32 (r["Tags"])
					));
				}
			}
			connection.Close ();
			return storedUsers;
		}

		public static bool checkifUserExists(List<storedPersonalInfoData> storedUsers, string Username) {

			foreach (storedPersonalInfoData d in storedUsers) {
				if (d.Username.ToLower() == Username.ToLower()) {
					return true;
				}
			}

			return false;
		}


		public static PersonalInfoData getCurrentUserData(string Username) {

			PersonalInfoData myData = new PersonalInfoData(); 
			string dbName = "personal.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.InitializeLifetimeService ();
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [User] WHERE Username='"+Username+"'";
				var r = contents.ExecuteReader ();

				while (r.Read ()) {

					if (Convert.ToInt32 (r ["ID"]) != null) {
						myData.ID = Convert.ToInt32 (r ["ID"]);
						myData.Username = r ["Username"].ToString ();
						myData.TaggedCount = Convert.ToInt32 (r ["TaggedCount"]);
						myData.Points = Convert.ToInt32 (r ["Points"]);
						myData.Longitude = Convert.ToDouble (r ["Longitude"]);
						myData.Latitude = Convert.ToDouble (r ["Latitude"]);
						myData.PowerUps = r ["Powerups"].ToString ();
						myData.Customization = r ["Customizations"].ToString();
						myData.Hotspots = r ["Hotspots"].ToString ();
						myData.CurrentCustomization = r ["CurrentCustomization"].ToString ();
						myData.Tags = Convert.ToInt32(r ["Tags"]);
					}
				}
			}
			connection.Close ();
			return myData;
		
		}
		public static bool UpdateUser(PersonalInfoData personal, string customization, int points, string powerup, string currentCustomization ) {

			if (customization == null)
				customization = personal.Customization;
			if (points == null)
				points = personal.Points;
			if (powerup == null)
				powerup = personal.PowerUps;
			if (currentCustomization == null)
				currentCustomization = personal.CurrentCustomization;


			string dbName = "personal.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {

				var commands = new[] {
					"UPDATE [USER] SET Customizations='"+ customization +"' WHERE ID=" + personal.ID,
					"UPDATE [USER] SET Powerups='"+ powerup +"'WHERE ID=" + personal.ID,
					"UPDATE [USER] SET Points='"+ points +"' WHERE ID=" + personal.ID,
					"UPDATE [USER] SET CurrentCustomization='" + currentCustomization+"'WHERE ID+" + personal.ID
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						c.ExecuteNonQuery ();
					}
				}
			}
			connection.Close ();
			return false;
		}

		public static bool createUser(string name ) {

		string dbName = "personal.sqlite";
		string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

		connection = new SqliteConnection ("Data Source=" + dbPath);
		connection.Open ();
		using (var contents = connection.CreateCommand ()) {
			
			Random random = new Random();
			int randomNumber = random.Next(0, 1000);

			var commands = new[] {
					"INSERT INTO [USER] ([ID], [Username]) VALUES ('"+randomNumber+"', '"+name+"')"
			};
			foreach (var command in commands) {
				using (var c = connection.CreateCommand ()) {
					c.CommandText = command;
					c.ExecuteNonQuery ();
				}
			}
		}
		connection.Close ();
		return false;
	}

}
}

