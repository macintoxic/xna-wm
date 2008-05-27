using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XMLContentShared;
using System.Collections.Generic;

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
        private SpriteSheet waterSheet;
        private SpriteSheet dirtSheet;

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

        private SpriteSheet LoadCloudTiles(Texture2D texture)
        {
            SpriteSheet sheet = new SpriteSheet(texture);
            sheet.AddSourceSprite((int)TileName.Clouds, new Rectangle(0, 0, 1024, 1024));
            return sheet;
        }

        private SpriteSheet LoadGroundTiles(Texture2D texture)
        {
            SpriteSheet sheet = new SpriteSheet(texture);
            sheet.AddSourceSprite((int)TileName.Base, new Rectangle(0, 0, 510, 510));
            sheet.AddSourceSprite((int)TileName.Detail1, new Rectangle(514, 0, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail2, new Rectangle(769, 0, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail3, new Rectangle(514, 256, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail4, new Rectangle(769, 256, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail1, new Rectangle(514, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail2, new Rectangle(769, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail3, new Rectangle(514, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail4, new Rectangle(769, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks1, new Rectangle(0, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks2, new Rectangle(256, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks3, new Rectangle(0, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks4, new Rectangle(256, 769, 255, 255));
            return sheet;
        }

        private SpriteSheet LoadWaterTiles(Texture2D texture)
        {
            SpriteSheet sheet = new SpriteSheet(texture);
            sheet.AddSourceSprite((int)TileName.Base, new Rectangle(0, 0, 510, 510));
            sheet.AddSourceSprite((int)TileName.Detail1, new Rectangle(514, 0, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail2, new Rectangle(769, 0, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail3, new Rectangle(514, 256, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail4, new Rectangle(769, 256, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail1, new Rectangle(514, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail2, new Rectangle(769, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail3, new Rectangle(514, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail4, new Rectangle(769, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks1, new Rectangle(0, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks2, new Rectangle(256, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks3, new Rectangle(0, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks4, new Rectangle(256, 769, 255, 255));
            return sheet;
        }

        private SpriteSheet LoadDirtTiles(Texture2D texture)
        {
            SpriteSheet sheet = new SpriteSheet(texture);
            sheet.AddSourceSprite((int)TileName.Base, new Rectangle(0, 0, 510, 510));
            sheet.AddSourceSprite((int)TileName.Detail1, new Rectangle(514, 0, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail2, new Rectangle(769, 0, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail3, new Rectangle(514, 256, 255, 255));
            sheet.AddSourceSprite((int)TileName.Detail4, new Rectangle(769, 256, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail1, new Rectangle(514, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail2, new Rectangle(769, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail3, new Rectangle(514, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.SoftDetail4, new Rectangle(769, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks1, new Rectangle(0, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks2, new Rectangle(256, 514, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks3, new Rectangle(0, 769, 255, 255));
            sheet.AddSourceSprite((int)TileName.Rocks4, new Rectangle(256, 769, 255, 255));
            return sheet;
        }

        private TileGrid LoadGroundLayer()
        {
            TileGrid grid = new TileGrid(510, 510, numTiles, numTiles, Vector2.Zero, groundSheet, game.ScreenManager.GraphicsDevice);

            for (int i = 0; i < numTiles; i++)
            {
                for (int j = 0; j < numTiles; j++)
                {
                    grid.SetTile(i, j, 1);
                }
            }

            return grid;
        }

        private TileGrid LoadDetailLayer(int numDetailTiles)
        {
            TileGrid grid = new TileGrid(255, 255, numDetailTiles, numDetailTiles, new Vector2(127, 127), groundSheet, game.ScreenManager.GraphicsDevice);

            for (int i = 0; i < numDetailTiles; i++)
            {
                for (int j = 0; j < numDetailTiles; j++)
                {
                    switch (rand.Next(20))
                    {
                        case 0:
                            grid.SetTile(i, j, (int)TileName.Detail1);
                            break;
                        case 1:
                            grid.SetTile(i, j, (int)TileName.Detail2);
                            break;
                        case 2:
                            grid.SetTile(i, j, (int)TileName.Detail3);
                            break;
                        case 3:
                            grid.SetTile(i, j, (int)TileName.Detail4);
                            break;
                        case 4:
                        case 5:
                            grid.SetTile(i, j, (int)TileName.SoftDetail1);
                            break;
                        case 6:
                        case 7:
                            grid.SetTile(i, j, (int)TileName.SoftDetail2);
                            break;
                        case 8:
                        case 9:
                            grid.SetTile(i, j, (int)TileName.SoftDetail3);
                            break;
                        case 10:
                        case 11:
                            grid.SetTile(i, j, (int)TileName.SoftDetail4);
                            break;
                    }
                }
            }

            return grid;
        }

        private TileGrid LoadRockLayer(int numDetailTiles)
        {
            TileGrid grid = new TileGrid(255, 255, numDetailTiles, numDetailTiles, new Vector2(0, 0), groundSheet, game.ScreenManager.GraphicsDevice);

            for (int i = 0; i < numDetailTiles; i++)
            {
                for (int j = 0; j < numDetailTiles; j++)
                {
                    switch (rand.Next(25))
                    {
                        case 0:
                            grid.SetTile(i, j, (int)TileName.Rocks1);
                            break;
                        case 1:
                            grid.SetTile(i, j, (int)TileName.Rocks2);
                            break;
                        case 2:
                            grid.SetTile(i, j, (int)TileName.Rocks3);
                            break;
                        case 3:
                        case 4:
                            grid.SetTile(i, j, (int)TileName.Rocks4);
                            break;
                    }
                }
            }

            return grid;
        }

        private TileGrid LoadCloudLayer(int numCloudTiles)
        {
            TileGrid grid = new TileGrid(1024, 1024, numCloudTiles, numCloudTiles, Vector2.Zero, cloudSheet, game.ScreenManager.GraphicsDevice);

            for (int i = 0; i < numCloudTiles; i++)
            {
                for (int j = 0; j < numCloudTiles; j++)
                {

                    grid.SetTile(i, j, (int)TileName.Clouds);

                }
            }

            return grid;
        }

        public void LoadContent()
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content");

            screenCenter = new Vector2(
                (float)game.ScreenManager.GraphicsDevice.Viewport.Width / 2f,
                (float)game.ScreenManager.GraphicsDevice.Viewport.Height / 2f);

            Level level = content.Load<Level>(@"XML\Maps\WvsM");
            //List<Tile> tileList = content.Load<List<Tile>>("XML\\Maps\\WvsM");
            Texture2D groundTexture = content.Load<Texture2D>("Textures\\Terrain\\ground");
            Texture2D cloudTexture = content.Load<Texture2D>("Textures\\Terrain\\clouds");
            //Texture2D waterTexture = content.Load<Texture2D>("Textures\\Terrain\\water");
            //Texture2D dirtTexture = content.Load<Texture2D>("Textures\\Terrain\\dirt");

            groundSheet = LoadGroundTiles(groundTexture);
            cloudSheet = LoadCloudTiles(cloudTexture);
            //waterSheet = LoadWaterTiles(waterTexture);
            //dirtSheet = LoadDirtTiles(dirtTexture);

            int numDetailTiles = (numTiles * 2 - 1);
            int numCloudTiles = numTiles / 6 + 1;

            groundLayer = LoadGroundLayer();
            detailLayer = LoadDetailLayer(numDetailTiles);
            rockLayer = LoadRockLayer(numDetailTiles);
            cloudLayer = LoadCloudLayer(numCloudTiles);

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
            cloudLayer.Draw(spriteBatch);
        }

        /// <summary>
        /// Reset the camera to the center of the tile grid
        /// and reset the position of the animted sprite
        /// </summary>
        private void ResetToInitialPositions()
        {
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
            rockLayer.CameraRotation = camera.Rotation;
            cloudLayer.CameraRotation = camera.Rotation;

            //set zoom
            groundLayer.CameraZoom = camera.Zoom;
            detailLayer.CameraZoom = camera.Zoom; 
            rockLayer.CameraZoom = camera.Zoom;
            cloudLayer.CameraZoom = camera.Zoom + 1.0f;

            //For an extra special effect, the camera zoom is figured into the cloud
            //alpha. The clouds will appear to fade out as camera zooms in.
            cloudLayer.Color = new Color(new Vector4(1.0f, 1.0f, 1.0f, 2 / (2f * camera.Zoom + 1.0f)));

            //set position
            groundLayer.CameraPosition = camera.Position;
            detailLayer.CameraPosition = camera.Position;
            rockLayer.CameraPosition = camera.Position;
            cloudLayer.CameraPosition = camera.Position / 3.0f; // to acheive a paralax effect, scale down cloud movement

            //changes have been accounted for, reset the changed value so that this
            //function is not called unnecessarily
            camera.ResetChanged();
        }
    }
}
