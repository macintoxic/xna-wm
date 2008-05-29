using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using XMLContentShared;

namespace WM.Units
{
    public class HumanOid : UnitBase
    {

        Vector2 targetPosition;
        UnitBase attackTarget;

        public HumanOid(UnitItem unitDefinition)
            : base(unitDefinition.Name, unitDefinition.Position, unitDefinition.Rotation, unitDefinition.Scale, unitDefinition.AttackRadius, unitDefinition.Speed, unitDefinition.TextureAsset, unitDefinition.Offset, unitDefinition.Size)
        {
            initialize();
        }

        public HumanOid(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, offset, size)
        {
            initialize();
        }

        public void initialize()
        {
            SetMoveTargetPosition(new Vector2(-1, -1));
        }

        public override void Update(GameTime gameTime)
        {
            // Handle actions
            Move(gameTime);
            Attack(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, float time)
        {
            /*
            // Draw a spinning cat sprite, looking it up from the sprite sheet by name.
            batch.Draw(spriteSheetUnit.Texture, Position,
                             spriteSheetUnit.SourceRectangle("cat"), Color.White,
                             time, new Vector2(50, 50), Scale.X, SpriteEffects.None, 0);

            // Draw an animating glow effect, by rapidly cycling
            // through 7 slightly different sprite images.
            const int animationFramesPerSecond = 20;
            const int animationFrameCount = 7;

            // Look up the index of the first glow sprite.
            int glowIndex = spriteSheetUnit.GetIndex("glow1");

            // Modify the index to select the current frame of the animation.
            glowIndex += (int)(time * animationFramesPerSecond) % animationFrameCount;

            // Draw the current glow sprite.
            batch.Draw(spriteSheetUnit.Texture, new Rectangle(100, 150, 200, 200),
                             spriteSheetUnit.SourceRectangle(glowIndex), Color.White);
            */

            base.Draw(batch, time);
        }

        public void Move(GameTime gameTime)
        {              
            // stop moving when having a target.
            if (attackTarget != null)
                return;

            // Find a path toward the destination.
            // Is the unit there yet?
            if( Position != TargetPosition 
                && TargetPosition != new Vector2(-1,-1) )
            {
                // Test if we can get at the position or else what is the closest we can get.                
                Vector2 currentTargetPosition = FindClosestPosition(TargetPosition);
                
                Vector2 Distance = currentTargetPosition - Position;
                if (Distance.LengthSquared() <= 10)
                {
                    Position = currentTargetPosition;
                    SetMoveTargetPosition(new Vector2(-1, -1));
                }

                Distance.Normalize();
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += Distance * (Speed * elapsed);// todo may be take ground type into account to determine speed.
                
            }
        }

        public void Attack(GameTime gameTime)
        {
            // When not having any target do not attack.
            if (attackTarget == null)
                return;


        }

        public UnitBase FindTargetWithinRadius()
        {
            return null;
        }

        /// <summary>
        // Searches for the closest location near the TargetPosition which is possible to move to.
        /// </summary>
        private Vector2 FindClosestPosition(Vector2 TargetPosition)
        {
            // todo ...

            // Loop through whole map

            // Test path toward found position.

            // Return the last correct position found on path.

            return TargetPosition;
        }

        public override void UpdateNetworkReader(PacketReader reader)
        {
            base.UpdateNetworkReader(reader);
        }

        public override void UpdateNetworkWriter(PacketWriter writer)
        {
            base.UpdateNetworkWriter(writer);
        }

        public override void SetMoveTargetPosition(Vector2 targetPosition) 
        {
            TargetPosition = targetPosition;
        }

        public override void ClearMoveTargetPosition(Vector2 targetPosition)
        {
            TargetPosition = new Vector2(-1,-1);
        }

        public override void SetAttackTarget(UnitBase target)
        {
            AttackTarget = target;
        }
        
        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }

        public UnitBase AttackTarget
        {
            get { return attackTarget; }
            set { attackTarget = value; }
        }
        
    }
}
