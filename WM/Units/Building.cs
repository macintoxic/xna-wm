using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WM.Units
{
    class Building : UnitBase
    {
        public Building(Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset)
            : base(position, rotation, scale, targetRadius, speed, textureAsset)
        {
            targetRadius = 0;
            speed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}