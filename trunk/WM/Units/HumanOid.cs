using System;
using Microsoft.Xna.Framework;

namespace WM.Units
{
    class HumanOid : UnitBase
    {
        public HumanOid(Vector2 position, float rotation, Vector2 scale, string textureAsset)
            : base(position, rotation, scale, textureAsset)
        {            
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
