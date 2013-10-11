using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

using ExitGames.Client.DemoParticle;
using ExitGames.Client.Photon.LoadBalancing;

namespace ExitGames.Client.DemoParticle
{
	public class Demo
	{
		// Connection
		private string[] connectionParams;

		// Game logic
		public GameLogic LocalGameLogic { get; private set; }
		private Queue<GameLogic> backgroundGames;

		// Rooms
		private string[] availableRooms;

		// States
		public bool UserInfoIsVisible; 

		// Actions
		private Action PaintAction;

		/// <summary>
		/// Game initialization
		/// </summary>
		/// <param name="connectionParams">
		/// Master Server address, App ID, Game version
		/// </param>
		public Demo (string[] connectionParams, Action PaintAction)
		{
			this.connectionParams = connectionParams;

			this.LocalGameLogic = new GameLogic(
				connectionParams[0],
				connectionParams[1],
				connectionParams[2]
			);

			this.LocalGameLogic.Start();

			this.backgroundGames = new Queue<GameLogic>();

			// Run game loop in a separate thread
			Thread thread = new Thread(this.GameLogic);
			thread.IsBackground = true;
			thread.Start();

			// Set the function for playground repaint
			this.PaintAction = PaintAction;
		}

		/// <summary>
		/// Game loop
		/// </summary>
		/// <param name="DrawAction">
		/// Action to be called for game area repaint.
		/// </param>
		public void GameLogic()
		{
			while (true)
			{
				// Call update loop as often as possible. 
				// It does its work in the intervals only.
				this.LocalGameLogic.UpdateLoop();

				// Lock background games to be sure
				// that they will not be accessed from another thread.
				lock (this.backgroundGames)
				{
					foreach (GameLogic game in this.backgroundGames)
					{
						game.UpdateLoop();
					}
				}
				
				// When anything changed in the game logic, it will "flag" 
				// UpdateVisuals as true and the form is repaint.
				if (this.LocalGameLogic.UpdateVisuals)
				{ 
					if (this.LocalGameLogic.State == ClientState.JoinedLobby)
					{
						Dictionary<string, RoomInfo> rooms = this.LocalGameLogic.RoomInfoList;
						availableRooms = new string[rooms.Count];
						this.LocalGameLogic.RoomInfoList.Keys.CopyTo(availableRooms, 0);
					}
					
					this.LocalGameLogic.UpdateVisuals = false;

					// Repaint.
					PaintAction();
				}
				Thread.Sleep(1);
			}
		}

		/// <summary>
		/// Check if local player is in game.
		/// </summary>
		private bool IsLocalPlayerInGame()
		{
			ParticlePlayer local = this.LocalGameLogic.LocalPlayer;
			if (local != null)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Add a client.
		/// </summary>
		public void AddClient()
		{
			GameLogic backgroundPlayer = new GameLogic(this.connectionParams[0], this.connectionParams[1], this.connectionParams[2]);
			backgroundPlayer.SetUseInterestGroups(this.LocalGameLogic.UseInterestGroups);
			backgroundPlayer.Start();
			lock (this.backgroundGames)
			{
				this.backgroundGames.Enqueue(backgroundPlayer);
			}
		}

		/// <summary>
		/// Remove a client.
		/// </summary>
		public void RemoveClient()
		{
			if (!IsLocalPlayerInGame())
			{
				return;
			}
			lock (this.backgroundGames)
			{
				if (backgroundGames.Count > 0)
				{
					GameLogic removedPlayer = backgroundGames.Dequeue();
					removedPlayer.Disconnect();
				}
			}
		}

		public void AutomoveOnOff(bool automove)
		{
			this.LocalGameLogic.MoveInterval.IsEnabled = automove;
		}

		/// <summary>
		/// Show or hide client name.
		/// </summary>
		public void ShowUserInfo(bool visible)
		{
			this.UserInfoIsVisible = visible;
			this.LocalGameLogic.UpdateVisuals = true;
		}

		public void ChangeGridSize()
		{
			this.LocalGameLogic.ChangeGridSize();
		}

		public void ChangeLocalPlayerColor()
		{
			if (this.LocalGameLogic.State == ClientState.Joined) 
				this.LocalGameLogic.ChangeLocalPlayercolor();
		}

		public void UpdateVisuals(bool update)
		{
			this.LocalGameLogic.UpdateVisuals = update;
		}

		#region Move the local client

		public void MoveLocalPlayerUp()
		{
			if (this.LocalGameLogic.MoveInterval.IsEnabled) 
				return;
			this.LocalGameLogic.LocalPlayer.PosY += 1;
			this.ClampLocalPlayerPositionAndUpdate();
		}

		public void MoveLocalPlayerDown()
		{
			if (this.LocalGameLogic.MoveInterval.IsEnabled) 
				return;
			this.LocalGameLogic.LocalPlayer.PosY -= 1;
			this.ClampLocalPlayerPositionAndUpdate();
		}

		public void MoveLocalPlayerLeft()
		{
			if (this.LocalGameLogic.MoveInterval.IsEnabled) 
				return;
			this.LocalGameLogic.LocalPlayer.PosX -= 1;
			this.ClampLocalPlayerPositionAndUpdate();
		}

		public void MoveLocalPlayerRight()
		{
			if (this.LocalGameLogic.MoveInterval.IsEnabled) 
				return;
			this.LocalGameLogic.LocalPlayer.PosX += 1;
			this.ClampLocalPlayerPositionAndUpdate();
		}
		#endregion

		/// <summary>
		/// Turn interest management on/off.
		/// </summary>

		public void InterestGroupsOnOff()
		{
			this.LocalGameLogic.SetUseInterestGroups(!this.LocalGameLogic.UseInterestGroups);
			
			lock (this.backgroundGames)
			{
				foreach (GameLogic game in backgroundGames)
				{
					game.SetUseInterestGroups(this.LocalGameLogic.UseInterestGroups);
				}
			}
		}

		private void ClampLocalPlayerPositionAndUpdate()
		{
			this.LocalGameLogic.LocalPlayer.ClampPosition();
			this.LocalGameLogic.UpdateVisuals = true;
		}
	
	}
}

