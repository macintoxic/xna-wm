using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WM.Units
{
    class Vehicle : HumanOid
    {
        public Vehicle(Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset)
            : base(position, rotation, scale, targetRadius, speed, textureAsset)
        {            
        }

    }
}
