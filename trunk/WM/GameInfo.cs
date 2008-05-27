using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WM
{
    public class GameInfo
    {
        private WMGame game;
        private ContentManager content;

        private Camera2D camera;
        private Vector2 screenCenter;

        private SpriteSheet groundSheet;
        private SpriteSheet cloudSheet;

        private TileGrid rockLayer;
        private TileGrid groundLayer;
        private TileGrid cloudLayer;
        private TileGrid detailLayer;

        public enum TileName : int
        {
            Empty = 0,
            Base = 1,
            Detail1 = 2,
            Detail2 = 3,
            Detail3 = 4,
            Detail4 = 5,
            SoftDetail1 = 6,
            SoftDetail2 = 7,
            SoftDetail3 = 8,
            SoftDetail4 = 9,
            Rocks1 = 10,
            Rocks2 = 11,
            Rocks3 = 12,
            Rocks4 = 13,
            Clouds = 14
        }

        private const int numTiles = 200;
        private Random rand;

        public GameInfo(WMGame game)
        {
            this.game = game;
            this.rand = new Random();
        }

        public void LoadContent()
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content");

            //When the backbuffer resolution changes, this part of the
            //LoadContent calback is used to reset the screen center
            screenCenter = new Vector2(
                (float)game.ScreenManager.GraphicsDevice.Viewport.Width / 2f,
                (float)game.ScreenManager.GraphicsDevice.Viewport.Height / 2f);


            Texture2D groundTexture = content.Load<Texture2D>("Textures\\Terrain\\ground");
            Texture2D cloudTexture = content.Load<Texture2D>("Textures\\Terrain\\clouds");

            cloudSheet = new SpriteSheet(cloudTexture);
            cloudSheet.AddSourceSprite((int)TileName.Clouds, new Rectangle(0, 0, 1024, 1024));

            groundSheet = new SpriteSheet(groundTexture);
            groundSheet.AddSourceSprite((int)TileName.Base, new Rectangle(0, 0, 510, 510));
            groundSheet.AddSourceSprite((int)TileName.Detail1, new Rectangle(514, 0, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Detail2, new Rectangle(769, 0, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Detail3, new Rectangle(514, 256, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Detail4, new Rectangle(769, 256, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.SoftDetail1, new Rectangle(514, 514, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.SoftDetail2, new Rectangle(769, 514, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.SoftDetail3, new Rectangle(514, 769, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.SoftDetail4, new Rectangle(769, 769, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Rocks1, new Rectangle(0, 514, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Rocks2, new Rectangle(256, 514, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Rocks3, new Rectangle(0, 769, 255, 255));
            groundSheet.AddSourceSprite((int)TileName.Rocks4, new Rectangle(256, 769, 255, 255));

            //calculate the number of detial tiles, which are 
            //half the size of the base tiles, so there are
            //twice as many (minus one since they are being offset)
            int numDetailTiles = (numTiles * 2 - 1);
            //add an offset to break up the pattern
            int numCloudTiles = numTiles / 6 + 1;

            //Create the ground layer tile
            groundLayer = new TileGrid(510, 510, numTiles, numTiles, Vector2.Zero, groundSheet, game.ScreenManager.GraphicsDevice);
            detailLayer = new TileGrid(255, 255, numDetailTiles, numDetailTiles, new Vector2(127, 127), groundSheet, game.ScreenManager.GraphicsDevice);
            rockLayer = new TileGrid(255, 255, numDetailTiles, numDetailTiles, new Vector2(0, 0), groundSheet, game.ScreenManager.GraphicsDevice);
            cloudLayer = new TileGrid(1024, 1024, numCloudTiles, numCloudTiles, Vector2.Zero, cloudSheet, game.ScreenManager.GraphicsDevice);

            //These loops fill the datas with some appropriate data.  
            //The clouds and ground clutter have been randomized.
            for (int i = 0; i < numTiles; i++)
            {
                for (int j = 0; j < numTiles; j++)
                {
                    groundLayer.SetTile(i, j, 1);
                }
            }

            for (int i = 0; i < numDetailTiles; i++)
            {
                for (int j = 0; j < numDetailTiles; j++)
                {
                    switch (rand.Next(20))
                    {
                        case 0:
                            detailLayer.SetTile(i, j, (int)TileName.Detail1);
                            break;
                        case 1:
                            detailLayer.SetTile(i, j, (int)TileName.Detail2);
                            break;
                        case 2:
                            detailLayer.SetTile(i, j, (int)TileName.Detail3);
                            break;
                        case 3:
                            detailLayer.SetTile(i, j, (int)TileName.Detail4);
                            break;
                        case 4:
                        case 5:
                            detailLayer.SetTile(i, j, (int)TileName.SoftDetail1);
                            break;
                        case 6:
                        case 7:
                            detailLayer.SetTile(i, j, (int)TileName.SoftDetail2);
                            break;
                        case 8:
                        case 9:
                            detailLayer.SetTile(i, j, (int)TileName.SoftDetail3);
                            break;
                        case 10:
                        case 11:
                            detailLayer.SetTile(i, j, (int)TileName.SoftDetail4);
                            break;
                    }
                }
            }
            for (int i = 0; i < numDetailTiles; i++)
            {
                for (int j = 0; j < numDetailTiles; j++)
                {
                    switch (rand.Next(25))
                    {
                        case 0:
                            rockLayer.SetTile(i, j, (int)TileName.Rocks1);
                            break;
                        case 1:
                            rockLayer.SetTile(i, j, (int)TileName.Rocks2);
                            break;
                        case 2:
                            rockLayer.SetTile(i, j, (int)TileName.Rocks3);
                            break;
                        case 3:
                        case 4:
                            rockLayer.SetTile(i, j, (int)TileName.Rocks4);
                            break;
                    }
                }
            }

            for (int i = 0; i < numCloudTiles; i++)
            {
                for (int j = 0; j < numCloudTiles; j++)
                {

                    cloudLayer.SetTile(i, j, (int)TileName.Clouds);

                }
            }

            camera = new Camera2D();

            ResetToInitialPositions();
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void HandleInput(InputState input)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (camera.IsChanged)
            {
                CameraChanged();
            }
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = game.ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = game.ScreenManager.SpriteBatch;

            //since we're drawing in order from back to front,
            //depth buffer is disabled
            graphics.RenderState.DepthBufferEnable = false;
            graphics.Clear(Color.CornflowerBlue);

            //draw the background layers
            groundLayer.Color = Color.LightGray;

            groundLayer.Draw(spriteBatch);
            detailLayer.Draw(spriteBatch);
            rockLayer.Draw(spriteBatch);

            //draw the clouds
            cloudLayer.Draw(spriteBatch);

            //spriteBatch.Begin();
            //spriteBatch.DrawString(gameFont, "// TODO", playerPosition, Color.Green);
            //spriteBatch.DrawString(gameFont, "Insert Gameplay Here", enemyPosition, Color.DarkRed);
            //spriteBatch.End();
        }

        /// <summary>
        /// Reset the camera to the center of the tile grid
        /// and reset the position of the animted sprite
        /// </summary>
        private void ResetToInitialPositions()
        {
            //set up the 2D camera
            //set the initial position to the center of the
            //tile field
            camera.Position = new Vector2(numTiles * 255);
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
            //set rotation
            groundLayer.CameraRotation = camera.Rotation;
            detailLayer.CameraRotation = camera.Rotation;
            cloudLayer.CameraRotation = camera.Rotation;
            rockLayer.CameraRotation = camera.Rotation;

            //set zoom
            groundLayer.CameraZoom = detailLayer.CameraZoom =
            rockLayer.CameraZoom = camera.Zoom;
            cloudLayer.CameraZoom = camera.Zoom + 1.0f;

            //For an extra special effect, the camera zoom is figured into the cloud
            //alpha. The clouds will appear to fade out as camera zooms in.
            cloudLayer.Color = new Color(new Vector4(
                    1.0f, 1.0f, 1.0f, 2 / (2f * camera.Zoom + 1.0f)));

            //set position
            groundLayer.CameraPosition = camera.Position;
            detailLayer.CameraPosition = camera.Position;
            rockLayer.CameraPosition = camera.Position;
            //to acheive a paralax effect, scale down cloud movement
            cloudLayer.CameraPosition = camera.Position / 3.0f;

            //changes have been accounted for, reset the changed value so that this
            //function is not called unnecessarily
            camera.ResetChanged();
        }
    }
}
