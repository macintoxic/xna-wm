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

        /// <summary>
        /// Return a object based on type and name.
        /// ObjectType: 
        ///     1 = HumanOid
        ///     2 = Vehicle
        ///     3 = Building
        /// </summary>
        public ItemDefinition GetObjectDefinitionByName(string nameToIdentify, int ObjectType)
        {
            if (ObjectType == 1)
            {
                for (int i = 0; i < humanOidList.Count; i++)
                    if (humanOidList[i].Name == nameToIdentify)
                        return humanOidList[i];
            }
            else if (ObjectType == 2)
            {
                for (int i = 0; i < vehicleList.Count; i++)
                    if (vehicleList[i].Name == nameToIdentify)
                        return vehicleList[i];
            }
            else if (ObjectType == 3)
            {
                for (int i = 0; i < buildingList.Count; i++)
                    if (buildingList[i].Name == nameToIdentify)
                        return buildingList[i];
            }
            return null;
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
