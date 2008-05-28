using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XMLContentShared
{
    public class TileLayer
    {
        /// <summary>
        /// The name of this layer.
        /// </summary>
        private string name;

        /// <summary>
        /// The depth of this layer.
        /// </summary>
        private float depth = 0.0f;

        /// <summary>
        /// The color of this layer.
        /// </summary>
        private Color color = Color.LightGray;

        /// <summary>
        /// An optional offset to the world.
        /// </summary>
        private Vector2 offset;

        private Vector2 screenCenter;

        /// <summary>
        /// The position of the camera on this layer.
        /// </summary>
        private Vector2 cameraPosition;

        /// <summary>
        /// The rotation of the camera on this layer.
        /// </summary>
        private float cameraRotation;

        /// <summary>
        /// The zoom of the camera on this layer.
        /// </summary>
        private float cameraZoom;

        /// <summary>
        /// The name of the texture that this layer uses.
        /// </summary>
        private string textureAsset;

        /// <summary>
        /// The texture that is used by this layer.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// A list with all the tiles that this layer contains.
        /// </summary>
        private List<Tile> tileList;

        /// <summary>
        /// Gets or sets the name of this tile layer.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the depth of this layer.
        /// </summary>
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        /// <summary>
        /// Gets or sets the color of this layer.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Gets or sets the optional offset to the rest of the world.
        /// </summary>
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        /// <summary>
        /// Gets or sets the optional offset to the rest of the world.
        /// </summary>
        [ContentSerializerIgnore]
        public Vector2 ScreenCenter
        {
            get { return screenCenter; }
            set { screenCenter = value; }
        }

        public Vector2 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        public float CameraRotation
        {
            get { return cameraRotation; }
            set { cameraRotation = value; }
        }

        public float CameraZoom
        {
            get { return cameraZoom; }
            set { cameraZoom = value; }
        }

        /// <summary>
        /// Gets or sets the texture that is used by this tile layer.
        /// </summary>
        public string TextureAsset
        {
            get { return textureAsset; }
            set { textureAsset = value; }
        }

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// Gets or sets the list of tiles that this layer contains.
        /// </summary>
        public List<Tile> TileList
        {
            get { return tileList; }
            set { tileList = value; }
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureAsset);
        }


        public void UnloadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (Tile tile in tileList)
            {
                Rectangle sourceRect = new Rectangle(
                    (int)tile.Offset.X,
                    (int)tile.Offset.Y,
                    (int)tile.Size.X,
                    (int)tile.Size.Y);

                spriteBatch.Draw(texture, screenCenter, sourceRect, color,
                    cameraRotation, tile.DrawingPosition, tile.DrawingScale, 
                    SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();
        }
    }
}
