// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH">
//   Exit Games GmbH, 2012
// </copyright>
// <summary>
//   The "Particle" demo is a load balanced and Photon Cloud compatible "coding" demo.
//   The focus is on showing how to use the Photon features without too much "game" code cluttering the view.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content.PM;
using ExitGames.Client.Photon.LoadBalancing;
using ExitGames.Client.DemoParticle;

namespace DemoParticle.Xamarin.Android
{
    [Activity(Label = "Particle Demo", MainLauncher = false, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private const string ServerAddress = "app.exitgamescloud.com:5055";
        private const string AppId = "";   // get your AppId from: https://cloud.exitgames.com/dashboard
        private const string GameVersion = "1.0";

        private string[] availableRooms;
        private Queue<GameLogic> backgroundGames;

        public Demo demo { get; private set; }

        MainView view;

        public bool ShowUserInfo { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            view = new MainView(this);
            this.SetContentView(view);
            
            if (AppId != "")
            {
                demo = new Demo(new string[] {ServerAddress, AppId, GameVersion}, delegate { view.PostInvalidate(); });
            }
        }

        /// <summary>
        /// This class handles drawing operations of the Activity
        /// </summary>
        private class MainView : View
        {
            MainActivity mainActivity;
            public MainView(Activity activity) : base(activity)
            {
                Focusable = true;
                mainActivity = (MainActivity)activity;
            }

            protected override void OnDraw(Canvas canvas)
            {
                base.OnDraw(canvas);

                canvas.DrawColor(Color.Black);

                Paint textPaint = new Paint();
                textPaint.SetStyle(Paint.Style.Fill);
                textPaint.Color = Color.White;

                if (AppId == "")
                {
                    canvas.DrawText("The AppId is not found. Please enter your AppId in MainActivity.cs", 10, 20, textPaint);
                }
                else
                {
                    if (mainActivity.demo.LocalGameLogic.LocalRoom != null && mainActivity.demo.LocalGameLogic.State == ClientState.Joined)
                    {
                        int gridSize = this.mainActivity.demo.LocalGameLogic.GridSize;

                        int rectWidth = this.Width / gridSize;
                        int rectHeight = this.Height / gridSize;

                        int x;
                        int y;

                        Paint backgroundPaint1 = new Paint();
                        backgroundPaint1.Color = Color.Gray;
                        backgroundPaint1.SetStyle(Paint.Style.Fill);

                        Paint backgroundPaint2 = new Paint();
                        backgroundPaint2.Color = Color.DarkGray;
                        backgroundPaint2.SetStyle(Paint.Style.Fill);

                        if (mainActivity.demo.LocalGameLogic.UseInterestGroups)
                        {
                            canvas.DrawRect(new Rect(0, 0, this.Width / 2, this.Height / 2), backgroundPaint1);
                            canvas.DrawRect(new Rect(this.Width / 2, 0, this.Width, this.Height / 2), backgroundPaint2);
                            canvas.DrawRect(new Rect(0, this.Height / 2, this.Width / 2, this.Height), backgroundPaint2);
                            canvas.DrawRect(new Rect(this.Width / 2, this.Height / 2, this.Width, this.Height), backgroundPaint1);
                        }
                        else
                        {
                            canvas.DrawRect(new Rect(0, 0, this.Width, this.Height), backgroundPaint1);
                        }

                        Paint clientPaint = new Paint();
                        clientPaint.SetStyle(Paint.Style.Fill);

                        clientPaint.Color = Color.Black;

                        for (int i = 0; i <= gridSize; i++)
                        {
                            x = i * rectWidth;
                            canvas.DrawLine(x, 0, x, this.Height, clientPaint);
                            y = i * rectHeight;
                            canvas.DrawLine(0, y, this.Width, y, clientPaint);
                        }


                        // Now draw the players (all of them)
                        lock (mainActivity.demo.LocalGameLogic.LocalRoom.Players)
                        {
                            foreach (ParticlePlayer p in mainActivity.demo.LocalGameLogic.LocalRoom.Players.Values)
                            {
                                x = p.PosX * rectWidth;
                                y = p.PosY * rectHeight;

                                byte alpha = 254;
                                if (!p.IsLocal && p.UpdateAge > 500)
                                {
                                    alpha = (p.UpdateAge > 1000) ? (byte)20 : (byte)80;
                                }

                                clientPaint.Color = IntToColor(p.Color, alpha);

                                canvas.DrawRect(new Rect(x + 1, y + 1, x + rectWidth, y + rectHeight), clientPaint);

                                clientPaint.Color = Color.White;

                                if (mainActivity.demo.UserInfoIsVisible)
                                {
                                    canvas.DrawText(p.Name, x, y + rectHeight / 2, clientPaint);
                                }
                            }
                        }
                    }
                    else
                    {
                        string connectionStatus = "";
                        switch (mainActivity.demo.LocalGameLogic.State)
                        {
                            case ClientState.ConnectingToMasterserver:
                                connectionStatus = "Connecting to Masterserver";
                                break;
                            case ClientState.ConnectedToMaster:
                                connectionStatus = "Connected to Masterserver";
                                break;
                            case ClientState.Joining:
                                connectionStatus = "Joining the game";
                                break;
                            case ClientState.DisconnectingFromMasterserver:
                                connectionStatus = "Disconnecting from Masterserver";
                                break;
                            case ClientState.ConnectingToGameserver:
                                connectionStatus = "Connecting to Gameserver";
                                break;
                            case ClientState.ConnectedToGameserver:
                                connectionStatus = "Connected to Gameserver";
                                break;
                        }
                        canvas.DrawText(connectionStatus + "...", 10, 20, textPaint);
                        Console.WriteLine(mainActivity.demo.LocalGameLogic.State);

                    }
                }
            }
        }

        /// <summary>
        /// Main menu elements
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 1, 1, Resource.String.InterestManagementMenuItem);
            menu.Add(0, 2, 2, Resource.String.ChangeGridSizeMenuItem);
            menu.Add(0, 3, 3, Resource.String.AddPlayerMenuItem);
            menu.Add(0, 4, 4, Resource.String.RemovePlayerMenuItem);
            menu.Add(0, 5, 5, Resource.String.ShowUserInfoMenuItem);
            menu.Add(0, 6, 6, Resource.String.ChangeColorMenuItem);
            menu.Add(0, 7, 7, Resource.String.AutomoveMenuItem);
            return true;
        }

        /// <summary>
        /// Menu item handlers
        /// </summary>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 1:
                    this.demo.InterestGroupsOnOff();
                    string msg = this.demo.LocalGameLogic.UseInterestGroups ? "on" : "off";
                    Toast.MakeText(this, "Interest management is " + msg, ToastLength.Short).Show();
                    return true;
                case 2:
                    this.demo.LocalGameLogic.ChangeGridSize();

                    return true;
                case 3:
                    this.demo.AddClient();
                    return true;
                case 4:
                    this.demo.RemoveClient();
                    return true;
                case 5:
                    this.demo.ShowUserInfo(this.demo.UserInfoIsVisible = !this.demo.UserInfoIsVisible);
                    return true;
                case 6:
                    this.demo.LocalGameLogic.ChangeLocalPlayercolor();
                    return true;
                case 7:
                    msg = (this.demo.LocalGameLogic.MoveInterval.IsEnabled = !this.demo.LocalGameLogic.MoveInterval.IsEnabled) ? "on" : "off";
                    Toast.MakeText(this, "Automove is " + msg, ToastLength.Short).Show();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private static Color IntToColor(int i)
        {
            return Color.Argb(0xFF, (byte)(i >> 16), (byte)(i >> 8), (byte)i);
        }

        private static Color IntToColor(int i, byte alpha)
        {
            return Color.Argb(alpha, (byte)(i >> 16), (byte)(i >> 8), (byte)i);
        }
    }
}