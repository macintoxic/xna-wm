using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;

namespace WM.Units
{
    public class Building : UnitBase
    {
        private UnitBase productionUnit;

        public Building(Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 Offset)
            : base(position, rotation, scale, targetRadius, speed, textureAsset, Offset)
        {
            targetRadius = 0;
            speed = 0;
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

        public Vector2 GetUnitSpawnPosition()
        {
            // todo update to correct location, maybe set by xml or always default right bottom ???
            return Position;
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
