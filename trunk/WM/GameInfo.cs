using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WM.Units;
using SpriteSheetRuntime;
using XMLContentShared;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using WM.Input;
using WM.Input;
using WM.MatchInfo;
using System.Diagnostics;


namespace WM
{
    public class GameInfo
    {
        private const float MovementRate = 500f;
        private const float RotationRate = 1.5f;
        private const float ZoomRate = 0.5f;

        private WMGame game;
        private ContentManager content;
        private MatchInfo.MatchInfo matchInfo;
        private MatchInfo.Player myPlayer;

        private MouseControl mouseControl;

        private Camera2D camera;
        private Vector2 screenCenter;

        private SpriteBatch spriteBatch;

        private List<UnitBase> UnitsOnMap;
        private Random rand;

        private Level currentLevel;

        public GameInfo(WMGame game)
        {
            this.game = game;
            this.rand = new Random();

            mouseControl = new MouseControl(this);

            matchInfo = new MatchInfo.MatchInfo();
            myPlayer = new MatchInfo.Player();
            matchInfo.AddPlayer(myPlayer);

            this.UnitsOnMap = new List<UnitBase>();
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

            XMLContentShared.Units unitList = content.Load<XMLContentShared.Units>("XML\\Units\\UnitDefinitions");
            GenerateUnitsOnMapList();

            camera = new Camera2D();
            ResetToInitialPositions();
        }

        public void UnloadContent()
        {
            content.Unload();

            if (currentLevel != null)
                currentLevel.UnloadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            //Call sample-specific input handling function
            //HandleKeyboardInput((float)gameTime.ElapsedGameTime.TotalSeconds);
            //mouseControl.HandleMouseInput((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (camera.IsChanged)
                CameraChanged();

            UpdateTiles();

            currentLevel.Update(gameTime);
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
            //SpriteBatch spriteBatch = game.ScreenManager.SpriteBatch;
            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            //since we're drawing in order from back to front,
            //depth buffer is disabled
            graphics.RenderState.DepthBufferEnable = false;
            graphics.Clear(Color.CornflowerBlue);

            currentLevel.Draw(gameTime, spriteBatch);

            spriteBatch.Begin();
            for (int i = 0; i < UnitsOnMap.Count; i++)
            {
                UnitsOnMap[i].Draw(spriteBatch, time);               
            }
            spriteBatch.End();              
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
   
        public void GenerateUnitsOnMapList()
        {
            HumanOid NewUnit = new HumanOid(new Vector2(100, 100), 0, new Vector2(0.5f, 0.5f), 10, 1, "XML\\Units\\SpriteSheetUnit");
            NewUnit.Load(content);
            UnitsOnMap.Add( NewUnit );

            NewUnit = new HumanOid(new Vector2(200, 200), 0, new Vector2(0.5f, 0.5f), 10, 1, "XML\\Units\\SpriteSheetUnit");
            NewUnit.Load(content);
            UnitsOnMap.Add( NewUnit );

            NewUnit = new HumanOid(new Vector2(300, 300), 0, new Vector2(0.5f, 0.5f), 10, 1, "XML\\Units\\SpriteSheetUnit");
            NewUnit.Load(content);
            UnitsOnMap.Add( NewUnit );

        }

        public MatchInfo.Player MyPlayer
        {
            get { return myPlayer; }
            set { myPlayer = value; }
        }

        public Camera2D Camera
        {
            get { return camera; }
            set { camera = value; }
        }

    }
}
