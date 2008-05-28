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
                Rectangle sourceRectangle = new Rectangle(
                    (int)tile.Offset.X,
                    (int)tile.Offset.Y,
                    (int)tile.Size.X,
                    (int)tile.Size.Y);

                spriteBatch.Draw(
                    texture, 
                    tile.Position,
                    sourceRectangle,
                    color, 
                    tile.Rotation, 
                    new Vector2(tile.Size.X / 2, tile.Size.Y / 2),
                    tile.Scale, 
                    SpriteEffects.None, 
                    depth);
            }

            spriteBatch.End();

            ////draw the background layers
            //groundLayer.Color = Color.LightGray;

            //groundLayer.Draw(spriteBatch);
            //detailLayer.Draw(spriteBatch);
            //rockLayer.Draw(spriteBatch);
            //cloudLayer.Draw(spriteBatch);
        }
    }
}
