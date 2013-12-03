using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace youreit
{

	public class CustomizationData
	{

		public CustomizationData(int ID, string Name, string ImgURL, Double Price)
		{
			this.ID = ID;
			this.Name = Name;
			this.ImgURL = ImgURL;
			this.Price = Price;
		}

		public int ID { get; set; }
		public string Name { get; set;}
		public string ImgURL { get; set;} 
		public Double Price { get; set;}
	}

	public class Customizations
	{
		public static SqliteConnection connection;


		public static List<CustomizationData>  DoSomeDataAccess ()
		{
			List<CustomizationData> cusomtizationList = new List<CustomizationData>(); 

			string dbName = "youreit.sqlite";
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);

			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();

			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * from [Customizations]";
				var r = contents.ExecuteReader ();
				while (r.Read ())
					cusomtizationList.Add(new CustomizationData(
						Convert.ToInt32(r["ID"]), r ["Name"].ToString(),
						r ["ImgURL"].ToString(), Convert.ToDouble(r["Price"])
					));
			}
			connection.Close ();

			return cusomtizationList;
		}


	}
}