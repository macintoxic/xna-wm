using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WM.Units.Projectiles
{
    public class ProjectileBase
    {
        private Vector2 position;
        private float rotation;
        Vector2 scale;
        Vector2 offset;
        Vector2 size;

        protected Texture2D texture;

        public Vector2 DrawingPosition;
        public Vector2 DrawingScale;
        public Vector2 screenCenter;

        private Vector2 direction;
        private float damage;
        private float radius;
        private float speed;
        private float currentLifeTime; 
        private float lifeTime; // LifeTime * Speed = Range
        
        MatchInfo.MatchInfo matchInfo;

        public ProjectileBase(Vector2 position, float rotation, Vector2 scale, Vector2 offset, Vector2 size,
                              Texture2D texture, Vector2 direction, float damage, float radius, float speed, 
                              float lifeTime, MatchInfo.MatchInfo matchInfo)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Offset = offset;
            Size = size;
            Texture = texture;
            Direction = direction;
            Damage= damage;
            Radius = radius;
            Speed = speed;
            LifeTime = lifeTime;
            MatchInfo = matchInfo;

            currentLifeTime = 0;

            // setup hardcoded a bullet texture for now.
            Texture = matchInfo.GameInfo.Content.Load<Texture2D>("Textures\\Terrain\\Water");

        }

        public bool CheckForHit()
        {
            List<UnitBase> collisionList = matchInfo.IsPositionAvailable(position, size);
            if ( collisionList.Count > 0 )
                //!(collisionList.Count==1 && collisionList[0]==this) ) // not possible to hit itself, since bullets are not returned by that function.
            {
                Explode();
                for (int i = 0; i< collisionList.Count; i++)
                    ApplyDamageToTarget(collisionList[i]);

                return true;
            }

            return false;
        }

        public void Explode()
        {        
            // todo... some explosion effect at the current bullet position.    

        }

        public void ApplyDamageToTarget(UnitBase HitTarget)
        {
            HitTarget.TakeHit(damage);
        }

        public virtual void Draw(SpriteBatch batch, float time)
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
        }

        public virtual void Update(GameTime gameTime) 
        { 
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentLifeTime + elapsed <= lifeTime) // travel speed distance
                Position += direction * speed * elapsed;
            else if (currentLifeTime < lifeTime)     // travel resulting distance
                Position += direction * speed * (lifeTime - currentLifeTime);
            else
            {
                // todo...  destroy/cleanup bullet.
                Explode();
            }

            CheckForHit();
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

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        public MatchInfo.MatchInfo MatchInfo
        {
            get { return matchInfo; }
            set { matchInfo = value; }
        }        
    
    }
}
