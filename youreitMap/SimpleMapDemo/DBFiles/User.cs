using SQLite;
using System;
using System.Data;
using System.IO;

namespace youreit
{
	[Table("User")]
	public class User
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string Username { get; set;} 
		public int Tagged { get; set;}
		public int Points { get; set;}
		public float Longitude { get; set; }
		public float Latitude { get; set; }

		//Stores ids of all of the following. 
		public string PowerUps { get; set;} 
		public string customizations { get; set; }
		public string Friends { get; set; }
		public string Events { get; set; }
	}
	
	public static class UserExample {

		/// <returns>
		/// Output of test query
		/// </returns>
		public static string DoSomeDataAccess () {

			var output = ""; 
			Console.WriteLine("---- Creating database, if it doesn't already exist");
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			var db = new SQLiteConnection (dbPath);
			db.CreateTable<User> ();

			if (db.Table<User> ().Count() == 0) {
				// only insert the data if it doesn't already exist
				var newUser = new User ();
				newUser.Username = "Jack";
				db.Insert (newUser); 

				newUser = new User ();
				newUser.Username = "Andrew";
				db.Insert (newUser); 

				newUser = new User ();
				newUser.Username = "Kevin";
				db.Insert (newUser);
			}

			Console.WriteLine ("----Reading data using Orm");
			var table = db.Table<User> ();
			foreach (var s in table) {
				output += "\n" + s.ID + " " + s.Username;
			}
			Console.WriteLine ("------" + output);
			return output;
		}

		public static string MoreComplexQuery () 
		{
			var output = "";
			output += "\nComplex query example: ";
			string dbPath = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			var db = new SQLiteConnection (dbPath);

			var query = db.Query<User> ("SELECT * FROM [User] WHERE Symbol = ?", "MSFT");
			foreach (var s in query) {
				output += "\n" + s.ID + " " + s.Username;
			}

			return output;
		}

		public static string Get () 
		{
			var output = "";
			output += "\nGet query example: ";
			string dbPath = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			var db = new SQLiteConnection (dbPath);

			var returned = db.Get<User>(3);

			return output;
		}

		public static string Delete () 
		{
			var output = "";
			output += "\nDelete query example: ";
			string dbPath = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.Personal), "youreit.db3");

			var db = new SQLiteConnection (dbPath);

			var rowcount = db.Delete(new User(){ID=3});

			return output;
		}


	}
}