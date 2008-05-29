using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XMLContentShared
{
    public class Level
    {
        /// <summary>
        /// The name of this level.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of this level.
        /// </summary>
        private Vector2 mapSize;

        /// <summary>
        /// The layer list of this level.
        /// </summary>
        private List<TileLayer> layerList;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public Level()
        {
            layerList = new List<TileLayer>();
        }

        /// <summary>
        /// Gets or sets the name of this level.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the name of this level.
        /// </summary>
        public Vector2 MapSize
        {
            get { return mapSize; }
            set { mapSize = value; }
        }

        /// <summary>
        /// Gets or sets the layer list of this level.
        /// </summary>
        public List<TileLayer> LayerList
        {
            get { return layerList; }
            set { layerList = value; }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (TileLayer layer in layerList)
                layer.LoadContent(content);

        }

        public void UnloadContent(ContentManager content)
        {
            foreach (TileLayer layer in layerList)
                layer.UnloadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            foreach (TileLayer layer in layerList)
                layer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (TileLayer layer in layerList)
                layer.Draw(gameTime, spriteBatch);
        }
    }
}
