using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class HotspotData
	{

		public HotspotData(int ID, string Name, Double Longitude, Double Latitude, string Reward, Double Price, string Time)
		{
			this.ID = ID;
			this.Name = Name;
			this.Longitude = Longitude;
			this.Latitude = Latitude;
			this.Reward = Reward;
			this.Price = Price;
			this.Time = Time;
		}

		public int ID { get; set; }
		public string Name { get; set;}
		public string Time { get; set;} 
		public Double Price { get; set;}
		public string Reward { get; set; }
		public Double Longitude { get; set; }
		public Double Latitude { get; set; }

	}

	public class Hotspots
	{
		public static SqliteConnection connection;


		public static List<HotspotData>  DoSomeDataAccess ()
		{
			List<HotspotData> hotspotList = new List<HotspotData>(); 

			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			bool exists = File.Exists (dbPath);

			if (!exists) {
				// Need to create the database and seed it with some data.
				Mono.Data.Sqlite.SqliteConnection.CreateFile (dbPath);
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();

				//TODO: Need to learn how to fill this from an external DB. (For a later day)
				var commands = new[] {
					"CREATE TABLE [Hotspots] (ID int, Name ntext, Longitude float, Latitude float, Reward string, Price float, Time DateTime );"
					,
					"INSERT INTO [Hotspots] ([ID], [Name], [Longitude], [Latitude], [Reward], [Price]) VALUES ('1', 'Starbucks', '49.1', '49.2', 'No reward', '0.1')"
					,
					"INSERT INTO [Hotspots] ([ID], [Name], [Longitude], [Latitude], [Reward], [Price]) VALUES ('2', 'Starbucks', '59.1', '44.2', 'No reward', '0.1')"
					,
					"INSERT INTO [Hotspots] ([ID], [Name], [Longitude], [Latitude], [Reward], [Price]) VALUES ('3', 'Starbucks', '69.1', '534.2', 'No reward', '0.1')"

					,"CREATE TABLE [Powerups] (ID int, Name ntext, Longitude float, Latitude float, Edition int, Price float );"
					,
					"INSERT INTO [Powerups] ([ID], [Name], [Longitude], [Latitude], [Edition], [Price]) VALUES ('1', 'Starbucks', '49.1', '49.2', '1', '0.1')"
					,
					"INSERT INTO [Powerups] ([ID], [Name], [Longitude], [Latitude], [Edition], [Price]) VALUES ('2', 'Starbucks', '59.1', '44.2', '1', '0.1')"
					,
					"INSERT INTO [Powerups] ([ID], [Name], [Longitude], [Latitude], [Edition], [Price]) VALUES ('3', 'Starbucks', '69.1', '534.2', '2', '0.1')"

					,"CREATE TABLE [Customizations] (ID int, Name ntext, ImgURL string, Price float);"
					,
					"INSERT INTO [Customizations] ([ID], [Name], [ImgURL], [Price]) VALUES ('1', 'Red', 'cusomtizations.png', '0.0')"
					,
					"INSERT INTO [Customizations] ([ID], [Name], [ImgURL], [Price]) VALUES ('2', 'Green', 'cusomtizations.png', '2.0')"
					,
					"INSERT INTO [Customizations] ([ID], [Name], [ImgURL], [Price]) VALUES ('3', 'Blue', 'cusomtizations.png', '1.0')"
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
					}
				}
			} else {
				connection = new SqliteConnection ("Data Source=" + dbPath);
				connection.Open ();
			}

			// query the database to prove data was inserted!

			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Hotspots]";
				var r = contents.ExecuteReader ();
				while (r.Read ())
					hotspotList.Add(new HotspotData(
						Convert.ToInt32(r["ID"]), r ["Name"].ToString(),
						Convert.ToDouble(r["Longitude"]),Convert.ToDouble(r["Latitude"]), 
						r ["Reward"].ToString(), Convert.ToDouble(r["Price"]),
						r["Time"].ToString()
						));
			}
			connection.Close ();

			return hotspotList;
		}


	}
}