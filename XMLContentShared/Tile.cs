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

        private Vector2 drawingPosition;

        private float rotation;

        private Vector2 scale;
        private Vector2 drawingScale;

        private Vector2 offset;

        private Vector2 size;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        [ContentSerializerIgnore]
        public Vector2 DrawingPosition
        {
            get { return drawingPosition; }
            set { drawingPosition = value; }
        }

        [ContentSerializerIgnore]
        public Vector2 DrawingScale
        {
            get { return drawingScale; }
            set { drawingScale = value; }
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
    }
}
