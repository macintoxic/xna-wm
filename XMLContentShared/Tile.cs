using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XMLContentShared
{
    public class Tile
    {
        Vector2 position;
        float rotation;
        Vector2 scale;

        string textureAsset;
        Texture2D texture;

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

        public string TextureAsset
        {
            get { return textureAsset; }
            set { textureAsset = value; }
        }

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }

        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureAsset);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(
                texture,
                position,
                null,
                Color.White,
                rotation,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f);
        }
    }
}
