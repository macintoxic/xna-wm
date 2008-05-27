using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WM.Units
{
    class HumanOid : UnitBase
    {
        public HumanOid(Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset)
            : base(position, rotation, scale, targetRadius, speed, textureAsset)
        {            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

        public void Move(GameTime gameTime)
        {                    
        }

        public void Attack(GameTime gameTime, UnitBase OtherTarget)
        {
        }

        public UnitBase FindTargetWithinRadius()
        {
            return null;
        }

    }
}
