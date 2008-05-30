using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WM.Units.Projectiles
{
    public class Bullet : ProjectileBase
    {

        public Bullet(Vector2 position, float rotation, Vector2 scale, Vector2 offset, Vector2 size,
                      Texture2D texture, Vector2 direction, float damage, float radius, float speed,
                      float lifeTime, MatchInfo.MatchInfo matchInfo)
            : base(position, rotation, scale, offset, size, texture, direction, damage, radius, speed, lifeTime, matchInfo)
        {
            
        }

    }
}
