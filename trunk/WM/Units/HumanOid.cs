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
        float FiringTime;
        float FireDelay;

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
            FiringTime = 0.0f;
            FireDelay = 2.0f;
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

                Vector2 Distance = TargetPosition - Position;//closestTargetPosition - Position;
                //Trace.WriteLine(Distance.LengthSquared());
                if (Distance.LengthSquared() <= 10)
                {
                    Trace.Write("  Stop movement 1");
                    //Trace.Write(Position);
                    //Trace.WriteLine("FOUND DEST... ");
                    //Position = closestTargetPosition;
                    //SetMoveTargetPosition(new Vector2(-1, -1));
                    bMoveTowardtarget = false;
                    return;
                }

                if (closestTargetPosition != Position)
                {
                    Distance = closestTargetPosition - Position;
                    Distance.Normalize();
                    Position += Distance * GetUnitSpeed(elapsed);
                }
            }
        }

        /// <summary>
        // Searches for the closest location near the TargetPosition which is possible to move to.
        /// </summary>
        private Vector2 FindClosestPosition(Vector2 TargetPosition, float elapsedTime)
        {
            // todo ... complete correct path finding
            // ....
            bool bFindAvailablePosition = true;
            short TryAllDirections = 3;                     // used for trying to find a movement direction in 4 directions.
            float rotationAngle = (2 * (float)Math.PI) / 4; // 90 degrees in radians
            Vector2 newDir = new Vector2(0, 0);             // stores the newly calculated dir
            Vector2 newTargetPosition;

            // See if next step is possible.
            //newTargetPosition = CalculateNextDirection(new Vector2(TargetPosition.X, TargetPosition.Y), elapsedTime);
            if ( CalculateNextDirection(new Vector2(TargetPosition.X, TargetPosition.Y), elapsedTime) )
            {
                return TargetPosition;
            }
            // else continue and search for an evasive route
            Vector2 distanceNormalized = new Vector2(TargetPosition.X, TargetPosition.Y) - Position;
            distanceNormalized.Normalize();
            newTargetPosition = new Vector2(Position.X, Position.Y);
            newTargetPosition += distanceNormalized * GetUnitSpeed(elapsedTime);
            /* //rotate 4 times 90 degrees counterclockwise, example
            Vector2 oldDir = new Vector2(1, 0);
            Vector2 newDir = new Vector2(0, 0);
            float r = (2 * (float)Math.PI) / 4; // 90 degrees in radians
            Trace.WriteLine(r);
            for (int i = 0; i < 4; i++)
            {
                Trace.WriteLine(i);
                Trace.Write("   ");
                Trace.WriteLine(oldDir);
                newDir.X = (float)Math.Cos(r) * oldDir.X - (float)Math.Sin(r) * oldDir.Y;
                newDir.Y = (float)Math.Cos(r) * oldDir.Y + (float)Math.Sin(r) * oldDir.X;
                oldDir = new Vector2(newDir.X, newDir.Y);
            }
            */

            // Loop through whole map

            // Test path toward Target position. If it is reachable. Else return closest reachable point.

            // Return the last correct position found on path. For example when we need to go around a building, or mountain, etc

            // Find Location between Destination and origin
            while (bFindAvailablePosition && bMoveTowardtarget && TryAllDirections >= 0)  
            {
                //Trace.WriteLine(" find new pos");
                List<UnitBase> UnitListFound = MatchInfo.IsPositionAvailable(newTargetPosition, Size);
                if (UnitListFound.Count == 0)
                {
                    Trace.Write("  go to this direction. ");
                    bFindAvailablePosition = false;
                }
                else if ( UnitListFound.Count == 1 && UnitListFound[0] == this )    // there is only a collision with ourself so exit pathfinding.
                {
                    //Trace.Write("     self");
                    Trace.Write("  Stop movement 3");
                    bFindAvailablePosition = false;
                    //bMoveTowardtarget = false; // <--- very ugly to stop complete movement
                }
                else
                {
                    Trace.WriteLine("     Correct the path:   FIX_ME, i cannot find a path and am search to long... help let me stop after searching for awhile... i want to stop search my target. bMoveTowardtarget = false ");
                    // todo..  doesn't work very well so for now we just make the unit hold its current position.
                    newTargetPosition = Position;

                    /* // this code below. Evade path should be done/calculated by the function CalculateNextDirection(..)
                    if ( TryAllDirections == 3 ) // verify 90 degrees, counterclockwise
                    {
                        TryAllDirections = 2;
                        newDir.X = (float)Math.Cos(rotationAngle) * distanceNormalized.X - (float)Math.Sin(rotationAngle) * distanceNormalized.Y;
                        newDir.Y = (float)Math.Cos(rotationAngle) * distanceNormalized.Y + (float)Math.Sin(rotationAngle) * distanceNormalized.X;                        
                        distanceNormalized = new Vector2(newDir.X,newDir.Y);
                        newTargetPosition.X = TargetPosition.X + (distanceNormalized.X * Size.X) + (distanceNormalized.X * GetUnitSpeed(elapsedTime));
                        newTargetPosition.Y = TargetPosition.Y + (distanceNormalized.Y * Size.Y) + (distanceNormalized.Y * GetUnitSpeed(elapsedTime)); 
                    }
                    else if (TryAllDirections == 2) // verify 180 degrees, counterclockwise (since dir is already rotated 90 we only need another 90 to make 180.)
                    {
                        TryAllDirections = 1;
                        newDir.X = (float)Math.Cos(rotationAngle) * distanceNormalized.X - (float)Math.Sin(rotationAngle) * distanceNormalized.Y;
                        newDir.Y = (float)Math.Cos(rotationAngle) * distanceNormalized.Y + (float)Math.Sin(rotationAngle) * distanceNormalized.X;
                        distanceNormalized = new Vector2(newDir.X, newDir.Y);
                        newTargetPosition.X = TargetPosition.X + (distanceNormalized.X * Size.X) + (distanceNormalized.X * GetUnitSpeed(elapsedTime));
                        newTargetPosition.Y = TargetPosition.Y + (distanceNormalized.Y * Size.Y) + (distanceNormalized.Y * GetUnitSpeed(elapsedTime)); 
                    }
                    else if (TryAllDirections == 1) // verify 270 degrees, it is already 180 degrees rotated.
                    {
                        // End of all test positions/directions afterwards end loop.
                        TryAllDirections = 0;
                        newDir.X = (float)Math.Cos(rotationAngle) * distanceNormalized.X - (float)Math.Sin(rotationAngle) * distanceNormalized.Y;
                        newDir.Y = (float)Math.Cos(rotationAngle) * distanceNormalized.Y + (float)Math.Sin(rotationAngle) * distanceNormalized.X;
                        distanceNormalized = new Vector2(newDir.X, newDir.Y);
                        newTargetPosition.X = TargetPosition.X + (distanceNormalized.X * Size.X) + (distanceNormalized.X * GetUnitSpeed(elapsedTime));
                        newTargetPosition.Y = TargetPosition.Y + (distanceNormalized.Y * Size.Y) + (distanceNormalized.Y * GetUnitSpeed(elapsedTime)); 
                    }
                    else
                    {
                        Trace.WriteLine("No good position found"); 
                        // Keep retrying for some time and if it still doesn't work, stop unit.
                        //bMoveTowardtarget = false;                        
                        TryAllDirections = -1;
                        //newTargetPosition = Position;
                    }
                    */
                }
            }
            return newTargetPosition;
        }

        /// <summary>
        // Tests if the new position doesn't collide with anything if it does it provides another direction.
        // possibly to move around.
        /// </summary>
        //private Vector2 CalculateNextDirection(Vector2 TargetPosition, float elapsedTime)
        private bool CalculateNextDirection(Vector2 TargetPosition, float elapsedTime)
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
                        //Trace.WriteLine("  COLLIDE    ");
                        // temporary code. Lets just stop the unit.
                        // bMoveTowardtarget = false;
                        //TargetPosition = Position;
                        return false;
                    }
                }
            }

            // temporary just return original path / target position.
            return true;
        }
        
        /// <summary>
        // Use this function to get the speed of the unit, since it takes all settings into account, like ground type, etc
        /// </summary>
        public float GetUnitSpeed(float elapsedTime)
        {
            return (Speed * elapsedTime);// todo maybe take ground type into account to determine speed.
        }

        public void Attack(GameTime gameTime)
        {
            // If no target available try finding one.            
            if (attackTarget == null )
            {
                FindTargetWithinRadius();
                // When still not having found any target do not attack.
                if (attackTarget == null)
                {
                    FiringTime = 0.0f;
                    return;
                }
            }
            else
            {
                // Check if current target is still in range else find another
                Vector2 distance = attackTarget.Position - Position;
                if ( Math.Abs(distance.Length()) > TargetRadius )
                {
                    FindTargetWithinRadius();
                    // When still not having found any target do not attack.
                    if (attackTarget == null)
                    {
                        FiringTime = 0.0f;
                        return;
                    }
                }
            }

            // If we came this far we do have a target now, start shooting.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FireAtTarget(elapsed);
        }

        public UnitBase FindTargetWithinRadius()
        {
            List<UnitBase> targetList = MatchInfo.AllObjectsWithinRadius(Position, TargetRadius);
            if (targetList.Count > 0)
                attackTarget = targetList[0];

            return attackTarget;
        }

        public void FireAtTarget(float elapsed)
        {
            FiringTime += elapsed;
            // If we waited longer then our firedelay shoot at target.
            if (FiringTime >= FireDelay)
            {
                FiringTime -= FireDelay;

                // todo .. Spawn a projectile

            }

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
