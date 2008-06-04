using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WM.Units;
using SpriteSheetRuntime;
using XMLContentShared;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using WM.Input;
using WM.MatchInfo;
using System.Diagnostics;
using Microsoft.Xna.Framework.GamerServices;

namespace WM
{
    public class GameInfo
    {
        const int maxGamers = 16;
        const int maxLocalGamers = 4;

        public NetworkSession NetworkSession;
        public PacketWriter PacketWriter = new PacketWriter();
        public PacketReader PacketReader = new PacketReader();

        private const float MovementRate = 500f;
        private const float RotationRate = 1.5f;
        private const float ZoomRate = 0.5f;

        private WMGame game;
        private ContentManager content;
        private MatchInfo.MatchInfo matchInfo;
        private MatchInfo.Player myPlayer;

        private Hud hud;

        private MouseControl mouseControl;

        private Camera2D camera;
        private Vector2 screenCenter;

        private SpriteBatch spriteBatch;

        private Random rand;

        private Level currentLevel;
        private XMLContentShared.Units unitList;

        public GameInfo(WMGame game)
        {
            this.game = game;
            this.rand = new Random();

            mouseControl = new MouseControl(this);
            matchInfo = new MatchInfo.MatchInfo(this);
        }

        private void JoinOrCreateNetworkSession()
        {
            try
            {
                // Search for sessions.
                using (AvailableNetworkSessionCollection availableSessions = 
                    NetworkSession.Find(NetworkSessionType.SystemLink, maxLocalGamers, null))
                {
                    if (availableSessions.Count == 0)
                    {
                        CreateNetworkSession();
                        return;
                    }

                    // Join the first session we found.
                    NetworkSession = NetworkSession.Join(availableSessions[0]);
                    HookSessionEvents();
                }
            }
            catch (Exception)
            {
                CreateNetworkSession();
            }
        }

        private void CreateNetworkSession()
        {
            try
            {
                NetworkSession = NetworkSession.Create(NetworkSessionType.SystemLink, maxLocalGamers, maxGamers);
                HookSessionEvents();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// After creating or joining a network session, we must subscribe to
        /// some events so we will be notified when the session changes state.
        /// </summary>
        private void HookSessionEvents()
        {
            NetworkSession.GamerJoined += GamerJoinedEventHandler;
            NetworkSession.GamerLeft += GamerLeftEventHandler;
            NetworkSession.SessionEnded += SessionEndedEventHandler;
        }

        /// <summary>
        /// This event handler will be called whenever a new gamer joins the session.
        /// </summary>
        private void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            int gamerIndex = NetworkSession.AllGamers.IndexOf(e.Gamer);

            MatchInfo.Player player = new MatchInfo.Player(matchInfo);

            matchInfo.AddPlayer(player);

            if (e.Gamer.IsHost)
            {
                // When we're the host, we should store our player object in the myPlayer var.
                myPlayer = player;
            }

            e.Gamer.Tag = player;
        }

        /// <summary>
        /// This event handler will be called whenever an existing gamer left the session.
        /// </summary>
        private void GamerLeftEventHandler(object sender, GamerLeftEventArgs e)
        {
            int gamerIndex = NetworkSession.AllGamers.IndexOf(e.Gamer);

            MatchInfo.Player player = (MatchInfo.Player)e.Gamer.Tag;

            matchInfo.RemovePlayer(player);
        }   

        /// <summary>
        /// Event handler notifies us when the network session has ended.
        /// </summary>
        private void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            //errorMessage = e.EndReason.ToString();

            NetworkSession.Dispose();
            NetworkSession = null;
        }

        public void LoadContent()
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content");

            spriteBatch = new SpriteBatch(game.ScreenManager.GraphicsDevice);    

            screenCenter = new Vector2(
                (float)game.ScreenManager.GraphicsDevice.Viewport.Width / 2f,
                (float)game.ScreenManager.GraphicsDevice.Viewport.Height / 2f);

            currentLevel = content.Load<Level>(@"XML\Maps\WvsM");
            currentLevel.LoadContent(content);

            unitList = content.Load<XMLContentShared.Units>("XML\\Units\\UnitDefinitions");
            unitList.LoadContent(content);

            camera = new Camera2D();
            camera.MapSize = currentLevel.MapSize;
            ResetToInitialPositions();

            hud = new Hud(this);
            hud.HudElementClick += new HudElementClick(hud_HudElementClick);
            hud.LoadContent(content);
        }

        private void hud_HudElementClick(Hud hud, HudElementType element)
        {            
            switch (element)
            {
                case HudElementType.BuildBarrack:
                    {
                        mouseControl.ClearSelections(MyPlayer);
                        BuildingItem newBuildingItem = (BuildingItem)unitList.GetObjectDefinitionByName("Barrack", 3);
                        if (newBuildingItem != null)
                        {
                            UnitItem productionUnitItem = (UnitItem)unitList.GetObjectDefinitionByName(newBuildingItem.ProductionUnit, 1);
                            MyPlayer.SelectedBuildingInHud = new Building(newBuildingItem, productionUnitItem, matchInfo);
                        }
                        else
                        {
                            MyPlayer.SelectedBuildingInHud = null;
                        }
                    }
                    break;

                case HudElementType.BuildHQ:
                    {
                        mouseControl.ClearSelections(MyPlayer);
                        BuildingItem newBuildingItem = (BuildingItem)unitList.GetObjectDefinitionByName("HeadQuarter", 3);
                        if (newBuildingItem != null)
                        {
                            UnitItem productionUnitItem = (UnitItem)unitList.GetObjectDefinitionByName(newBuildingItem.ProductionUnit, 1);
                            MyPlayer.SelectedBuildingInHud = new Building(newBuildingItem, productionUnitItem, matchInfo);
                        }
                        else
                        {
                            MyPlayer.SelectedBuildingInHud = null;
                        }
                    }
                    break;
                case HudElementType.BuildWarFactory:
                    {
                        mouseControl.ClearSelections(MyPlayer);
                        BuildingItem newBuildingItem = (BuildingItem)unitList.GetObjectDefinitionByName("WarFactory", 3);
                        if (newBuildingItem != null)
                        {
                            UnitItem productionUnitItem = (UnitItem)unitList.GetObjectDefinitionByName(newBuildingItem.ProductionUnit, 2);
                            MyPlayer.SelectedBuildingInHud = new Building(newBuildingItem, productionUnitItem, matchInfo);
                        }
                        else
                        {
                            MyPlayer.SelectedBuildingInHud = null;
                        }
                    }
                    break;

                case HudElementType.BuildSoldier:
                    {
                        UnitItem newUnitItem = (UnitItem)unitList.GetObjectDefinitionByName("Soldier", 1);
                        if (newUnitItem != null)
                        {
                            MyPlayer.SelectedUnitInHud = new HumanOid(newUnitItem, matchInfo);
                            mouseControl.TryUnitProduction(MyPlayer);
                        }
                        else
                        {
                            MyPlayer.SelectedUnitInHud = null;
                        }
                    }
                    break;

                case HudElementType.BuildTank:
                    {
                        UnitItem newUnitItem = (UnitItem)unitList.GetObjectDefinitionByName("Tank", 2);
                        if (newUnitItem != null)
                        {
                            MyPlayer.SelectedUnitInHud = new Vehicle(newUnitItem, matchInfo);
                            mouseControl.TryUnitProduction(MyPlayer);
                        }
                        else
                        {
                            MyPlayer.SelectedUnitInHud = null;
                        }
                    }
                    break;

            }
        }

        public void UnloadContent()
        {
            content.Unload();

            if (currentLevel != null)
                currentLevel.UnloadContent(content);

            if (hud != null)
                hud.UnloadContent(content);
        }

        /// <summary>
        /// Updates the state of the network session, moving the tanks
        /// around and synchronizing their state over the network.
        /// </summary>
        private void UpdateNetworkSession()
        {
            if (NetworkSession != null)
            {
                // Read inputs for locally controlled units, and send them to the server.
                foreach (LocalNetworkGamer gamer in NetworkSession.LocalGamers)
                {
                    UpdateLocalGamer(gamer);
                }

                // If we are the server, update all the units and transmit their latest positions back out over 
                // the network.
                if (NetworkSession.IsHost)
                {
                    UpdateServer();
                }

                // Pump the underlying session object.
                NetworkSession.Update();

                // Make sure the session has not ended.
                if (NetworkSession == null)
                    return;

                // Read any incoming network packets.
                foreach (LocalNetworkGamer gamer in NetworkSession.LocalGamers)
                {
                    if (gamer.IsHost)
                    {
                        ServerReadInputFromClients(gamer);
                    }
                    else
                    {
                        ClientReadGameStateFromServer(gamer);
                    }
                }
            }
            else
                JoinOrCreateNetworkSession();
        }
        
        /// <summary>
        /// Helper for updating a locally controlled gamer.
        /// </summary>
        private void UpdateLocalGamer(LocalNetworkGamer gamer)
        {
            // Look up what player data is associated with this local player.
            Player localPlayer = gamer.Tag as MatchInfo.Player;

            // Only send if we are not the server. There is no point sending packets
            // to ourselves, because we already know what they will contain!
            if (!NetworkSession.IsHost)
            {
                localPlayer.UpdateNetworkWriter(PacketWriter);

                // Send our input data to the server.
                gamer.SendData(PacketWriter, SendDataOptions.InOrder, NetworkSession.Host);
            }
        }


        /// <summary>
        /// This method only runs on the server. It calls Update on all the tank instances, both local and remote, 
        /// using inputs that have been received over the network. It then sends the resulting tank position data 
        /// to everyone in the session.
        /// </summary>
        private void UpdateServer()
        {
            // First off, our packet will indicate how many players it has data for.
            PacketWriter.Write(NetworkSession.AllGamers.Count);

            // Loop over all the players in the session, not just the local ones!
            foreach (NetworkGamer gamer in NetworkSession.AllGamers)
            {
                // Look up what tank is associated with this player.
                Player player = gamer.Tag as Player;

                // Update the player.
                //player.Update();

                // Write the player state into the output network packet.
                player.UpdateNetworkWriter(PacketWriter);
            }

            // Send the combined data for all tanks to everyone in the session.
            LocalNetworkGamer server = (LocalNetworkGamer)NetworkSession.Host;

            server.SendData(PacketWriter, SendDataOptions.InOrder);
        }

        /// <summary>
        /// This method only runs on the server. It reads tank inputs that have been sent over the network by a 
        /// client machine, storing them for later use by the UpdateServer method.
        /// </summary>
        private void ServerReadInputFromClients(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(PacketReader, out sender);

                if (!sender.IsLocal)
                {
                    // Look up the tank associated with whoever sent this packet.
                    Player remotePlayer = sender.Tag as Player;

                    // Read the latest inputs controlling this tank.
                    remotePlayer.UpdateNetworkReader(PacketReader);
                }
            }
        }

        /// <summary>
        /// This method only runs on client machines. It reads the unit position data that has been computed by 
        /// the server.
        /// </summary>
        private void ClientReadGameStateFromServer(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(PacketReader, out sender);

                // If a player has recently joined or left, it is possible the server might have sent information 
                // about a different number of players than the client currently knows about. If so, we will be 
                // unable to match up which data refers to which player. The solution is just to ignore the packet 
                // for now: this situation will resolve itself as soon as the client gets the join/leave notification.
                if (NetworkSession.AllGamers.Count != PacketReader.ReadInt32())
                    continue;

                // This packet contains data about all the players in the session.
                foreach (NetworkGamer remoteGamer in NetworkSession.AllGamers)
                {
                    Player player = remoteGamer.Tag as Player;

                    player.UpdateNetworkReader(PacketReader);
                }
            }
        }
        
        /// <summary>
        /// Helper for reading incoming network packets.
        /// </summary>
        void ReadIncomingPackets(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(PacketReader, out sender);

                // Discard packets sent by local gamers: we already know their state!
                if (sender.IsLocal)
                    continue;

                // Look up the tank associated with whoever sent this packet.
                Player remotePlayer = sender.Tag as Player;

                remotePlayer.UpdateNetworkReader(PacketReader);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Gamer.SignedInGamers.Count == 0)
            {
                // If there are no profiles signed in, we cannot proceed.
                // Show the Guide so the user can sign in.
                Guide.ShowSignIn(4, false);
            }
            else
            {
                UpdateNetworkSession();

                if (camera.IsChanged)
                    CameraChanged();

                UpdateTiles();

                GraphicsDevice graphics = game.ScreenManager.GraphicsDevice;

                currentLevel.Update(gameTime);

                hud.Update(gameTime, graphics);

                MatchInfo.Update(gameTime);
            }
            
            mouseControl.Update();
        }

        private void UpdateTiles()
        {
            foreach (TileLayer layer in currentLevel.LayerList)
            {
                layer.ScreenCenter = screenCenter;

                foreach (Tile tile in layer.TileList)
                {
                    Vector2 position = Vector2.Zero;
                    position.X = tile.Position.X * tile.Scale.X;
                    position.Y = tile.Position.Y * tile.Scale.Y;

                    Vector2 worldOffset = layer.Offset;
                    Vector2 cameraPosition = camera.Position;
                    Vector2 scaleValue = tile.Scale;
                    float zoomValue = camera.Zoom;

                    // Offset the positions by the word position of the tile grid 
                    // this is the actual position of the tile in world coordinates.
                    Vector2.Add(ref position, ref worldOffset, out position);

                    // Now, we get the camera position relative to the tile's position
                    Vector2.Subtract(ref cameraPosition, ref position, out position);

                    // Get the tile's final size (note that scaling is done after 
                    // determining the position)
                    Vector2 scale;
                    Vector2.Multiply(ref scaleValue, zoomValue, out scale);

                    tile.DrawingPosition = position;
                    tile.DrawingScale = scale;
                }
            }

            //matchInfo.UpdateUnitPositions();
            //matchInfo.UpdateBuildingPositions();
            MyPlayer.UpdateUnitPositions();
            MyPlayer.UpdateBuildingPositions();
            MyPlayer.UpdateProjectilePositions();
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Call sample-specific input handling function
            HandleKeyboardInput(elapsed, input);
            mouseControl.HandleMouseInput(elapsed);//HandleMouseInput(elapsed, input);
        }
        
        public void HandleKeyboardInput(float elapsed, InputState input)
        {
            //check for camera movement
            float dX = ReadKeyboardAxis(input.CurrentKeyboardStates[0], Keys.Left, Keys.Right) * elapsed * MovementRate;
            float dY = ReadKeyboardAxis(input.CurrentKeyboardStates[0], Keys.Down, Keys.Up) * elapsed * MovementRate;

            camera.MoveRight(ref dX);
            camera.MoveUp(ref dY);

            //Trace.WriteLine(camera.Position);
        }

        /// <summary>
        /// Uses a pair of keys to simulate a positive or negative axis input.
        /// </summary>
        private static float ReadKeyboardAxis(KeyboardState keyState, Keys downKey, Keys upKey)
        {
            float value = 0;

            if (keyState.IsKeyDown(downKey))
                value -= 1.0f;

            if (keyState.IsKeyDown(upKey))
                value += 1.0f;

            return value;
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = game.ScreenManager.GraphicsDevice;
            
            //since we're drawing in order from back to front,
            //depth buffer is disabled
            graphics.RenderState.DepthBufferEnable = false;
            graphics.Clear(Color.Black);

            if (Gamer.SignedInGamers.Count > 0 && myPlayer != null)
            {
                //SpriteBatch spriteBatch = game.ScreenManager.SpriteBatch;
                float time = (float)gameTime.TotalGameTime.TotalSeconds;
                //Draw level
                currentLevel.Draw(gameTime, spriteBatch);
                hud.Draw(gameTime, graphics, spriteBatch);

                //Draw Units/Buildings on Map
                matchInfo.Draw(spriteBatch, time);
            }             

            // Draw anything the mouseControl wants us to draw.
            mouseControl.Draw();
        }

        /// <summary>
        /// Reset the camera to the center of the tile grid
        /// and reset the position of the animted sprite
        /// </summary>
        private void ResetToInitialPositions()
        {
            camera.Position = Vector2.Zero;
            camera.Rotation = 0f;
            camera.Zoom = 1f;
            camera.MoveUsingScreenAxis = true;

            CameraChanged();
        }

        /// <summary>
        /// This function is called when the camera's values have changed
        /// and is used to update the properties of the tiles and animated sprite
        /// </summary>
        public void CameraChanged()
        {
            foreach (TileLayer layer in currentLevel.LayerList)
            {
                layer.CameraPosition = camera.Position;
                layer.CameraRotation = camera.Rotation;
                layer.CameraZoom = camera.Zoom;
            }

            //changes have been accounted for, reset the changed value so that this
            //function is not called unnecessarily
            camera.ResetChanged();
        }


        public WMGame Game
        {
            get { return game; }
            set { game = value; }
        }

        public MatchInfo.Player MyPlayer
        {
            get { return myPlayer; }
            set { myPlayer = value; }
        }

        public MatchInfo.MatchInfo MatchInfo
        {
            get { return matchInfo; }
            set { matchInfo = value; }
        }        

        public Camera2D Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        public Vector2 ScreenCenter
        {
            get { return screenCenter; }
            set { screenCenter = value; }
        }

        public XMLContentShared.Units UnitList
        {
            get { return unitList; }
            set { unitList = value; }
        }
    }
}
