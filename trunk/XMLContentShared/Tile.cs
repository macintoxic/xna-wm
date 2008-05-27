using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XMLContentShared
{
    public class Tile
    {
        //<Position>0 0</Position>
        //<Rotation>0</Rotation>
        //<Scale>.1 .1</Scale>
        //<Offset>0 0</Offset>
        //<Size>100 100</Size>

        private Vector2 position;

        private float rotation;

        private Vector2 scale;

        private Vector2 offset;

        private Vector2 size;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public void Draw(SpriteBatch batch)
        {
            //batch.Draw(
            //    texture,
            //    position,
            //    null,
            //    Color.White,
            //    rotation,
            //    Vector2.Zero,
            //    scale,
            //    SpriteEffects.None,
            //    0f);
        }
    }
}
