using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;

namespace WM.Units
{
    public class Vehicle : HumanOid
    {
        public Vehicle()            
        {
        }

        public Vehicle(Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 Offset, Vector2 size)
            : base(position, rotation, scale, targetRadius, speed, textureAsset, Offset, size)
        {            
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
