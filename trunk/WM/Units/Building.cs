using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;
using XMLContentShared;

namespace WM.Units
{
    public class Building : UnitBase
    {
        private UnitBase productionUnit;

        public Building(BuildingItem buildingDefinition, UnitItem productionUnitItem)
            : base(buildingDefinition.Name, buildingDefinition.Position, buildingDefinition.Rotation, buildingDefinition.Scale, buildingDefinition.AttackRadius, buildingDefinition.Speed, buildingDefinition.TextureAsset, buildingDefinition.Offset, buildingDefinition.Size)
        {
            // todo Need some enhancement so we do not need to check for strings.
            DetermineProductionUnitFromDefinition(productionUnitItem, buildingDefinition.ProductionUnit);
            //targetRadius = 0; // not really needed when correctly set in XML
            //speed = 0;
        }

        public Building(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size, string productionUnit, UnitItem productionUnitItem)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, offset, size)
        {
            // todo Need some enhancement so we do not need to check for strings.
            DetermineProductionUnitFromDefinition(productionUnitItem, productionUnit);
            //ProductionUnit = productionUnit;
            //targetRadius = 0; // not really needed when correctly set in XML
            //speed = 0;
        }

        public Building(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size, UnitBase productionUnit)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, offset, size)
        {
            ProductionUnit = productionUnit;
            //ProductionUnit = productionUnit;
            //targetRadius = 0; // not really needed when correctly set in XML
            //speed = 0;
        }

        private void DetermineProductionUnitFromDefinition(UnitItem productionUnitItem, string productionName)
        {
            // todo Need some enhancement so we do not need to check for strings.
            if (productionName == "Soldier")
            {
                ProductionUnit = new HumanOid(productionUnitItem);
            }
            else if (productionName == "Tank")
            {
                ProductionUnit = new Vehicle(productionUnitItem);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch, float time)
        {
            base.Draw(batch, time);
        }

        public UnitBase GetProductionUnit() 
        {
            // todo this one should not be empty it should be a copy of a template found in Units class (HumanOidList or VehicleList)
            //if (productionUnit == "Soldier")
            //    ProductionUnit = new HumanOid();
            //else if (productionUnit == "Tank")
            //    ProductionUnit = new Vehicle();

            return productionUnit; 
        }

        public Vector2 GetUnitSpawnPosition()
        {
            // todo update to correct location, maybe set by xml or always default right bottom ???
            return Position+new Vector2(0,Size.Y);
        }

        public override void Load(ContentManager content)
        {
            //texture = content.Load<Texture2D>(textureAsset);
            //spriteSheetUnit = content.Load<SpriteSheetBase>("XML\\Units\\SpriteSheetUnit");
            texture = content.Load<Texture2D>(TextureAsset);
        }

        public UnitBase ProductionUnit
        {
            get { return productionUnit; }
            set { productionUnit = value; }
        }

        public override void UpdateNetworkReader(PacketReader reader)
        {
            base.UpdateNetworkReader(reader);
        }

        public override void UpdateNetworkWriter(PacketWriter writer)
        {
            base.UpdateNetworkWriter(writer);
        }
    }
}
