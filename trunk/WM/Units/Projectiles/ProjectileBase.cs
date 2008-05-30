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

        private float damage;
        private float radius;
        private float speed;

        public ProjectileBase(Vector2 position, float rotation, Vector2 scale, Vector2 offset, Vector2 size,
                              Texture2D texture, float damage, float radius, float speed)
        {

        }

        public void Explode()
        {

        }

        public void ApplyDamageToTarget(UnitBase HitTarget)
        {

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

        public virtual void Update(GameTime gameTime) { }

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


    }
}
