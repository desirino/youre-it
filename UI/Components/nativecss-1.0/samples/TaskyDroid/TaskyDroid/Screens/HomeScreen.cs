using System.Collections.Generic;
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.BL;
using Style;
using Style.Enums;
using Java.Net;
using System.IO;
namespace TaskyAndroid.Screens
{

	[Activity (Label = "Tasky", MainLauncher = true, Icon="@drawable/launcher")]			
	public class HomeScreen : Activity
	{
		protected Adapters.TaskListAdapter _taskList;
		protected IList<Task> _tasks;
		protected Button _addTaskButton = null;
		protected ListView _taskListView = null;
		static protected Application app;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			// set our layout to be the home screen
			this.SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			this._taskListView = FindViewById<ListView> (Resource.Id.lstTasks);
			this._addTaskButton = FindViewById<Button> (Resource.Id.btnAddTask);
			app = this.Application;
			// wire up add task button handler
			if(this._addTaskButton != null)
			{
				this._addTaskButton.Click += (sender, e) => {
					this.StartActivity(typeof(TaskDetailsScreen));
				};
			}
			
			// wire up task click handler
			if(this._taskListView != null)
			{
				this._taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("TaskID", this._tasks[e.Position].ID);
					this.StartActivity (taskDetails);
				};
			}

			// enable this to see logging
			//NativeCSS.SetDebugLogging(true);

			// To live edit, cd to styles.css and run "python -m SimpleHTTPServer"
			NativeCSS.StyleWithCSS("styles.css",
			                       new Uri("http://10.0.2.2:8000/styles.css"), 
			                       RemoteContentRefreshPeriod.EveryFiveSeconds);
		
		}
		protected override void OnResume ()
		{
			base.OnResume ();

			this._tasks = Tasky.BL.Managers.TaskManager.GetTasks();
			
			// create our adapter
			this._taskList = new Adapters.TaskListAdapter(this, this._tasks);

			//Hook up our adapter to our ListView
			this._taskListView.Adapter = this._taskList;
		}
		
	}
}

