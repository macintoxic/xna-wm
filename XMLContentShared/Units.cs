using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace XMLContentShared
{
    public class Units
    {
        /// <summary>
        /// The name of this level.
        /// </summary>
        private string name;

        /// <summary>
        /// The layer list of this level.
        /// </summary>
        private List<UnitItem> humanOidList;

        private List<UnitItem> vehicleList;

        /// <summary>
        /// The layer list of this level.
        /// </summary>
        private List<BuildingItem> buildingList;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public Units()
        {
            //layerList = new List<TileLayer>();
            humanOidList = new List<UnitItem>();
            vehicleList = new List<UnitItem>();
            buildingList = new List<BuildingItem>();
        }

        public void LoadContent(ContentManager content)
        {
            foreach (UnitItem unitItem in humanOidList)
                unitItem.LoadContent(content);

            foreach (UnitItem unitItem in vehicleList)
                unitItem.LoadContent(content);

            foreach (BuildingItem buildingItem in buildingList)
                buildingItem.LoadContent(content);
        }

        public void UnloadContent(ContentManager content)
        {
            //foreach (TileLayer layer in layerList)
            //    layer.UnloadContent(content);
        }

        /// <summary>
        /// Gets or sets the name of this level.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /*
        /// <summary>
        /// Gets or sets the layer list of this level.
        /// </summary>
        public List<TileLayer> LayerList
        {
            get { return layerList; }
            set { layerList = value; }
        }
        */
        /// <summary>
        /// Gets or sets the layer list of this level.
        /// </summary>
        public List<UnitItem> HumanOidList
        {
            get { return humanOidList; }
            set { humanOidList = value; }
        }

        public List<UnitItem> VehicleList
        {
            get { return vehicleList; }
            set { vehicleList = value; }
        }

        /// <summary>
        /// Gets or sets the layer list of this level.
        /// </summary>
        public List<BuildingItem> BuildingList
        {
            get { return buildingList; }
            set { buildingList = value; }
        }
    }
}
