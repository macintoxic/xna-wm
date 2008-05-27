using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace XMLContentShared
{
    public class Level
    {
        /// <summary>
        /// The name of this level.
        /// </summary>
        private string name;

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
        /// Gets or sets the layer list of this level.
        /// </summary>
        public List<TileLayer> LayerList
        {
            get { return layerList; }
            set { layerList = value; }
        }
    }
}
