using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpriteSheetRuntime;
using Microsoft.Xna.Framework.Net;

namespace WM.Units
{
    public class UnitBase
    {
        Vector2 position;
        float rotation;
        Vector2 scale;  // we only use the X for now
        Vector2 offset;  
        float targetRadius;
        float speed;
        int creditsCost;

        string textureAsset;
        protected Texture2D texture; // not used
        protected SpriteSheetBase spriteSheetUnit;

        public Vector2 DrawingPosition;
        public Vector2 DrawingScale;
        public Vector2 screenCenter;

        public UnitBase(Vector2 position, float rotation, Vector2 scale, float targetRadius, float Speed, string TextureAsset, Vector2 offset)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Offset = offset;
            TargetRadius = targetRadius;
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

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public float TargetRadius
        {
            get { return targetRadius; }
            set { targetRadius = value; }
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

        public string TextureAsset
        {
            get { return textureAsset; }
            set { textureAsset = value; }
        }

        //[ContentSerializerIgnore]
        //public Texture2D Texture
        //{
        //    get { return texture; }
        //}

        public virtual void Load(ContentManager content)
        {
            //texture = content.Load<Texture2D>(textureAsset);
            //spriteSheetUnit = content.Load<SpriteSheetBase>("XML\\Units\\SpriteSheetUnit");
            spriteSheetUnit = content.Load<SpriteSheetBase>(textureAsset);      
        }

        public virtual void Draw(SpriteBatch batch, float time)
        {
            /*
              Rectangle sourceRect = new Rectangle(
                (int)tile.Offset.X,
                (int)tile.Offset.Y,
                (int)tile.Size.X,
                (int)tile.Size.Y);
              
              batch.Draw(texture, screenCenter, sourceRect, color,
                cameraRotation, tile.DrawingPosition, tile.DrawingScale,
                SpriteEffects.None, 0.0f);          
            */

            Rectangle sourceRect = new Rectangle(
              (int)offset.X,
              (int)offset.Y,
              64,
              64);            
            
            batch.Draw(texture, 
                        screenCenter,
                        sourceRect, 
                        Color.White,
                        Rotation, 
                        DrawingPosition, 
                        DrawingScale,
                        SpriteEffects.None, 
                        0.0f);       
            
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


        public virtual void Update(GameTime gameTime) { }


        public virtual void UpdateNetworkReader(PacketReader reader)
        {
            position = reader.ReadVector2();
            rotation = reader.ReadSingle();
            scale = reader.ReadVector2();
            targetRadius = reader.ReadSingle();
            speed = reader.ReadSingle();
            creditsCost = reader.ReadInt32();
        }

        public virtual void UpdateNetworkWriter(PacketWriter writer)
        {
            writer.Write(position);
            writer.Write(rotation);
            writer.Write(scale);
            writer.Write(targetRadius);
            writer.Write(speed);
            writer.Write(creditsCost);
        }
    }
}
