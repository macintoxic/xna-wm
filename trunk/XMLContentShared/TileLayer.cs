using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XMLContentShared
{
    public class TileLayer
    {
        /// <summary>
        /// The name of this layer.
        /// </summary>
        private string name;

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

        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureAsset);
        }
    }
}
