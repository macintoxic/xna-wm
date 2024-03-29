using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpriteSheetRuntime;
using Microsoft.Xna.Framework.Net;
using WM.MatchInfo;

namespace WM.Units
{
    public abstract class UnitBase
    {
        Vector2 position;
        float rotation;
        Vector2 scale;
        Vector2 offset;
        Vector2 size;
        float targetRadius;
        float speed;
        int creditsCost;
        float health;

        string textureAsset;
        protected Texture2D texture;
        protected SpriteSheetBase spriteSheetUnit;

        public Vector2 DrawingPosition;
        public Vector2 DrawingScale;
        public Vector2 screenCenter;

        MatchInfo.MatchInfo matchInfo;
        public Player unitOwner;

        string name;    // used to identify type, not unique

        public UnitBase()
        {
        }

        public UnitBase(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string TextureAsset, Vector2 offset, Vector2 size, MatchInfo.MatchInfo matchInfo, Player unitOwnerObj)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Offset = offset;
            Size = size;
            Speed = speed;
            TargetRadius = targetRadius;
            speed = Speed;
            textureAsset = TextureAsset;
            creditsCost = 100;
            health = 40;
            Name = name;
            MatchInfo = matchInfo;
            unitOwner = unitOwnerObj;
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
        
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
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

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        
        public string TextureAsset
        {
            get { return textureAsset; }
            set { textureAsset = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public MatchInfo.MatchInfo MatchInfo
        {
            get { return matchInfo; }
            set { matchInfo = value; }
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
            //spriteSheetUnit = content.Load<SpriteSheetBase>(textureAsset);
            texture = content.Load<Texture2D>(TextureAsset);
        }

        public virtual void Draw(SpriteBatch batch, float time)
        {
            if (texture != null)
            {
                Rectangle sourceRect = new Rectangle(
                  (int)offset.X,
                  (int)offset.Y,
                  (int)size.X,
                  (int)size.Y);

                batch.Draw(texture,
                            screenCenter,
                            sourceRect,
                            Color.White,
                            Rotation,
                            DrawingPosition,
                            DrawingScale,
                            SpriteEffects.None,
                            0.0f);

                DrawCenter(batch, screenCenter, DrawingPosition);
                DrawBorder(batch, screenCenter, DrawingPosition);
            }

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

        public void DrawCenter(SpriteBatch batch, Vector2 aCenter, Vector2 aOffset)
        {
            Rectangle sourceRect = new Rectangle(
              355,
              600,
              10,
              10);

            batch.Draw(texture,
                        aCenter,
                        sourceRect,
                        Color.Green,
                        Rotation,
                        aOffset,
                        1,
                        SpriteEffects.None,
                        0.0f);
        }

        public void DrawBorder(SpriteBatch batch, Vector2 aCenter, Vector2 aOffset)
        {
            // todo
            Rectangle sourceRect = new Rectangle(
              355,
              600,
              5,
              5);

            batch.Draw(texture,
                        aCenter,
                        sourceRect,
                        Color.Green,
                        Rotation,
                        aOffset + new Vector2(-size.X/2, 0),
                        1,
                        SpriteEffects.None,
                        0.0f);

            batch.Draw(texture,
                        aCenter,
                        sourceRect,
                        Color.Green,
                        Rotation,
                        aOffset + new Vector2(size.X / 2, 0),
                        1,
                        SpriteEffects.None,
                        0.0f);

            batch.Draw(texture,
                        aCenter,
                        sourceRect,
                        Color.Green,
                        Rotation,
                        aOffset + new Vector2(0, Size.Y/2),
                        1,
                        SpriteEffects.None,
                        0.0f);

            batch.Draw(texture,
                        aCenter,
                        sourceRect,
                        Color.Green,
                        Rotation,
                        aOffset + new Vector2(0, -Size.Y / 2),
                        1,
                        SpriteEffects.None,
                        0.0f);
        }
        
        public virtual void Update(GameTime gameTime) { }
        
        public virtual void UpdateNetworkReader(PacketReader reader)
        {
            position = reader.ReadVector2();
            rotation = reader.ReadSingle();
            scale = reader.ReadVector2();
            offset = reader.ReadVector2();
            size = reader.ReadVector2();
            targetRadius = reader.ReadSingle();
            speed = reader.ReadSingle();
            creditsCost = reader.ReadInt32();
            health = reader.ReadSingle();
            textureAsset = reader.ReadString();
            name = reader.ReadString();

            // search players from player list and set this unit to have that owner.
            unitOwner = matchInfo.GetPlayerByNickName(reader.ReadString());            
        }

        public virtual void UpdateNetworkWriter(PacketWriter writer)
        {
            writer.Write(position);
            writer.Write(rotation);
            writer.Write(scale);
            writer.Write(offset);
            writer.Write(size);
            writer.Write(targetRadius);
            writer.Write(speed);
            writer.Write(creditsCost);
            writer.Write(health);
            writer.Write(textureAsset);
            writer.Write(name);    
            writer.Write(unitOwner.NickName);    
        }

        public virtual void SetMoveTargetPosition(Vector2 targetPosition){}
        public virtual void ClearMoveTargetPosition(Vector2 targetPosition) {}
        public virtual void SetAttackTarget(UnitBase target) {}
        public abstract void TakeHit(float damageAmount);
    }
}
