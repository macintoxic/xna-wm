using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WM.Units
{
    class UnitBase
    {
        Vector2 position;
        float rotation;
        Vector2 scale;
        float targetradius;
        float speed;

        string textureAsset;
        Texture2D texture;

        public UnitBase(Vector2 position, float rotation, Vector2 scale, float targetRadius, float Speed, string textureAsset)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            targetradius = targetRadius;
            speed = Speed;
            TextureAsset = textureAsset;
            
        }

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

        public float TargetRadius
        {
            get { return TargetRadius; }
            set { TargetRadius = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
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


        public virtual void Update(GameTime gameTime)   {}
    }
}
