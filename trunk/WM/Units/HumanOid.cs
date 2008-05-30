using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using XMLContentShared;
using System.Diagnostics;
using System.Collections.Generic;

namespace WM.Units
{
    public class HumanOid : UnitBase
    {

        bool bMoveTowardtarget;
        Vector2 targetPosition;
        Vector2 targetPositionMoveOffset;



        UnitBase attackTarget;

        public HumanOid(UnitItem unitDefinition, MatchInfo.MatchInfo matchInfo)
            : base(unitDefinition.Name, unitDefinition.Position, unitDefinition.Rotation, unitDefinition.Scale, unitDefinition.AttackRadius, unitDefinition.Speed, unitDefinition.TextureAsset, unitDefinition.Offset, unitDefinition.Size, matchInfo)
        {
            initialize();
        }

        public HumanOid(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size, MatchInfo.MatchInfo matchInfo)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, offset, size, matchInfo)
        {
            initialize();
        }

        public void initialize()
        {
            SetMoveTargetPosition(new Vector2(-1, -1));
            bMoveTowardtarget = false;
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
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // stop moving when having a target.
            if (attackTarget != null)
                return;

            //Trace.WriteLine(Position);
            //Trace.Write("   ");
            //Trace.Write(TargetPosition);
            //Trace.Write("   ");
            //Trace.Write(DrawingPosition);
            //Trace.WriteLine("");
            // Find a path toward the destination.
            // Is the unit there yet?
            if( Position != TargetPosition && bMoveTowardtarget )
            {
                // Test if we can get at the position or else what is the closest we can get.                
                Vector2 closestTargetPosition = FindClosestPosition(TargetPosition, elapsed);

                Vector2 Distance = closestTargetPosition - Position;
                //Trace.WriteLine(Distance.LengthSquared());
                if (Distance.LengthSquared() <= 10)
                {
                    //Trace.Write(Position);
                    //Trace.WriteLine("FOUND DEST... ");
                    //Position = closestTargetPosition;
                    //SetMoveTargetPosition(new Vector2(-1, -1));
                    bMoveTowardtarget = false;
                    return;
                }
                
                Distance.Normalize();
                Position += Distance * GetUnitSpeed(elapsed);
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
        private Vector2 FindClosestPosition(Vector2 TargetPosition, float elapsedTime)
        {
            // todo ... complete correct path finding
            bool bFindAvailablePosition = true;            
            Vector2 newTargetPosition = new Vector2(TargetPosition.X, TargetPosition.Y);

            // See if next step is possible.
            newTargetPosition = CalculateNextDirection(new Vector2(TargetPosition.X, TargetPosition.Y), elapsedTime);

            // Loop through whole map

            // Test path toward Target position. If it is reachable. Else return closest reachable point.

            // Return the last correct position found on path. For example when we need to go around a building, or mountain, etc

            // Find Location between Destination and origin
            while (bFindAvailablePosition && bMoveTowardtarget)                
            {
                //Trace.WriteLine(" find new pos");
                List<UnitBase> UnitListFound = MatchInfo.IsPositionAvailable(newTargetPosition, Size);
                if (UnitListFound.Count == 0)
                {
                    bFindAvailablePosition = false;
                }
                else if ( UnitListFound.Count == 1 && UnitListFound[0] == this )
                {
                    //Trace.Write("     self");
                    //newTargetPosition = Position;
                    bFindAvailablePosition = false;
                    bMoveTowardtarget = false; // <--- very ugly to stop complete movement
                }
                else
                {
                    //Trace.Write("     Correct the path: ");
                    Vector2 distanceNormalized = newTargetPosition - Position;
                    distanceNormalized.Normalize();
                    newTargetPosition.X -= distanceNormalized.X * 34; // todo Use unit size now it uses static 34
                    newTargetPosition.Y -= distanceNormalized.Y * 34;
                    //Trace.WriteLine(newTargetPosition);
                }
            }

            return newTargetPosition;
        }

        /// <summary>
        // Tests if the new position doesn't collide with anything if it does it provides another direction.
        // possibly to move around.
        /// </summary>
        private Vector2 CalculateNextDirection(Vector2 TargetPosition, float elapsedTime)
        {
            Vector2 Distance = TargetPosition - Position;
            Distance.Normalize();            
            Vector2 nextPosition = new Vector2( Position.X, Position.Y );
            nextPosition += Distance * GetUnitSpeed(elapsedTime);
            
            // If the new position is colliding, move around it. When possible.
            List<UnitBase> UnitListFound = MatchInfo.IsPositionAvailable(nextPosition, Size);
            if (UnitListFound.Count > 0)
            {
                for(int i=0; i < UnitListFound.Count; i++)
                {
                    if (UnitListFound[i] == this)
                    {}
                    else
                    {
                        // Unit is going to collide. Unit should evade.
                        // todo ..
                        //Trace.Write("  COLLIDE    ");
                        // temporary code. Lets just stop the unit.
                        bMoveTowardtarget = false;
                        TargetPosition = Position;
                    }
                }
            }

            // temporary just return original path / target position.
            return TargetPosition;
        }

        /// <summary>
        // Use this function to get the speed of the unit, since it takes all settings into account, like ground type, etc
        /// </summary>
        public float GetUnitSpeed(float elapsedTime)
        {
            return (Speed * elapsedTime);// todo maybe take ground type into account to determine speed.
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
            bMoveTowardtarget = true;
            TargetPosition = targetPosition;// +new Vector2(Size.X, Size.Y);
            targetPositionMoveOffset = new Vector2(0, 0);
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
