using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpriteSheetRuntime;

namespace WM.Units
{
    public class UnitBase
    {
        Vector2 position;
        float rotation;
        Vector2 scale;  // we only use the X for now
        float targetradius;
        float speed;
        int creditsCost;

        string textureAsset;
        Texture2D texture; // not used
        protected SpriteSheetBase spriteSheetUnit;

        public UnitBase(Vector2 position, float rotation, Vector2 scale, float targetRadius, float Speed, string TextureAsset)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            targetradius = targetRadius;
            speed = Speed;
            textureAsset = TextureAsset;
            creditsCost = 100;
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

        public int CreditsCost
        {
            get { return creditsCost; }
            set { creditsCost = value; }
        }

        //public string TextureAsset
        //{
        //    get { return textureAsset; }
        //    set { textureAsset = value; }
        //}

        //[ContentSerializerIgnore]
        //public Texture2D Texture
        //{
        //    get { return texture; }
        //}

        public void Load(ContentManager content)
        {
            //texture = content.Load<Texture2D>(textureAsset);                        
            //spriteSheetUnit = content.Load<SpriteSheetBase>("XML\\Units\\SpriteSheetUnit");
            spriteSheetUnit = content.Load<SpriteSheetBase>(textureAsset);
        }

        public virtual void Draw(SpriteBatch batch, float time)
        {
            /*
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
             */
        }


        public virtual void Update(GameTime gameTime)   {}
    }
}
