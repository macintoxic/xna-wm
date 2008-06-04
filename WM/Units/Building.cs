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

        public Building()
        {
        }

        public Building(BuildingItem buildingDefinition, UnitItem productionUnitItem, MatchInfo.MatchInfo matchInfo)
            : base(buildingDefinition.Name, buildingDefinition.Position, buildingDefinition.Rotation, buildingDefinition.Scale, buildingDefinition.AttackRadius, buildingDefinition.Speed, buildingDefinition.TextureAsset, buildingDefinition.Offset, buildingDefinition.Size, matchInfo)
        {
            // todo Need some enhancement so we do not need to check for strings.
            DetermineProductionUnitFromDefinition(productionUnitItem, buildingDefinition.ProductionUnit);
        }

        public Building(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size, string productionUnit, UnitItem productionUnitItem, MatchInfo.MatchInfo matchInfo)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, offset, size, matchInfo)
        {
            // todo Need some enhancement so we do not need to check for strings.
            DetermineProductionUnitFromDefinition(productionUnitItem, productionUnit);
        }

        public Building(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size, UnitBase productionUnit, MatchInfo.MatchInfo matchInfo)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, offset, size, matchInfo)
        {
            ProductionUnit = productionUnit;
        }

        private void DetermineProductionUnitFromDefinition(UnitItem productionUnitItem, string productionName)
        {
            // todo Need some enhancement so we do not need to check for strings.
            if (productionName == "Soldier")
            {
                ProductionUnit = new HumanOid(productionUnitItem, MatchInfo);
            }
            else if (productionName == "Tank")
            {
                ProductionUnit = new Vehicle(productionUnitItem, MatchInfo);
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
            return productionUnit; 
        }
                
        /// <summary>
        // return Vector2(0,0) when no valid spawn location found.
        /// </summary>
        public Vector2 GetUnitSpawnPosition(GameInfo gameInfo)
        {
            // Set unit spawn location below the building, if there is already a unit, spawn it next to it.
            Vector2 spawnPosition = gameInfo.MatchInfo.FindAvailableUnitSpawnPosition(Position, ProductionUnit.Size);
            return spawnPosition;
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

        public override void TakeHit(float damageAmount)
        {
            // todo ..import Health variable from ItemDefinition/UnitItem/BuildingItem (XML) so we can substract
            Health -= damageAmount;
            if (Health <= 0)
            {
                // todo ..Destroy UntiBase from level

                // todo check object type, humanoid or building.

                // Remove it from the lists
                MatchInfo.GameInfo.MyPlayer.RemoveBuilding(this);
            }
        }
    }
}
