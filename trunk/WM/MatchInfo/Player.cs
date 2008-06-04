using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using WM.Units;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Net;
using WM.Units.Projectiles;

namespace WM.MatchInfo
{
    public class Player
    {
        private GameInfo gameInfo;
        private List<HumanOid> unitHumanOidList;
        private List<Vehicle> unitVehicleList;
        private List<Building> unitBuildingList;
        private List<ProjectileBase> projectileList;
        private int creditAmount;
        private int rank;
        private string nickName;
        public bool buildingsLoseConditionValid;
        public bool PlayersEnterdValid;

        private Building selectedBuildingOnMap;
        private Building selectedBuildingInHud;
        private List<UnitBase> selectedUnitList;
        private UnitBase selectedUnitInHud;

        private MatchInfo matchInfo;

        public Player(MatchInfo matchInfoObj)
        {
            unitHumanOidList = new List<HumanOid>();
            unitVehicleList = new List<Vehicle>();
            unitBuildingList = new List<Building>();
            projectileList = new List<ProjectileBase>();
            creditAmount = 10000;
            rank = 0;
            nickName = "";
            buildingsLoseConditionValid = false;
            PlayersEnterdValid = false;

            selectedUnitList = new List<UnitBase>(); // selected on the map/in the world.
            selectedUnitInHud = null;
            selectedBuildingOnMap = null;
            selectedBuildingInHud = null;            

            matchInfo = matchInfoObj;
        }

        public void UpdateNetworkReader(PacketReader reader)
        {
            creditAmount = reader.ReadInt32();
            rank = reader.ReadInt32();
            nickName = reader.ReadString();

            foreach (HumanOid item in unitHumanOidList)
                item.UpdateNetworkReader(reader);

            foreach (Vehicle item in unitVehicleList)
                item.UpdateNetworkReader(reader);

            foreach (Building item in unitBuildingList)
                item.UpdateNetworkReader(reader);
        }

        public void UpdateNetworkWriter(PacketWriter writer)
        {
            writer.Write(creditAmount);
            writer.Write(rank);
            writer.Write(nickName);

            foreach (HumanOid item in unitHumanOidList)
                item.UpdateNetworkWriter(writer);

            foreach (Vehicle item in unitVehicleList)
                item.UpdateNetworkWriter(writer);

            foreach (Building item in unitBuildingList)
                item.UpdateNetworkWriter(writer);
        }
                
        public void CreateBuilding(Vector2 Position, Building BuildingType)
        {
            Building newBuilding = new Building(BuildingType.Name, Position, BuildingType.Rotation, BuildingType.Scale, BuildingType.TargetRadius, BuildingType.Speed, BuildingType.TextureAsset, BuildingType.Offset, BuildingType.Size, BuildingType.ProductionUnit, matchInfo);
            newBuilding.Load(matchInfo.GameInfo.Content);
            unitBuildingList.Add(newBuilding);
            // set building lose condition valid to true, so the player cna lose the game.
            buildingsLoseConditionValid = true;
        }

        public void CreateUnit(Vector2 Position, UnitBase UnitType)
        {
            if (UnitType.GetType().Name == "HumanOid")
            {
                HumanOid newUnit = new HumanOid(UnitType.Name, Position, UnitType.Rotation, UnitType.Scale, UnitType.TargetRadius, UnitType.Speed, UnitType.TextureAsset, UnitType.Offset, UnitType.Size, matchInfo, ((HumanOid)UnitType).AttackPower);
                newUnit.Load(matchInfo.GameInfo.Content);
                unitHumanOidList.Add(newUnit);                
            }
            else if (UnitType.GetType().Name == "Vehicle")
            {
                Vehicle newUnit = new Vehicle(UnitType.Name, Position, UnitType.Rotation, UnitType.Scale, UnitType.TargetRadius, UnitType.Speed, UnitType.TextureAsset, UnitType.Offset, UnitType.Size, matchInfo, ((Vehicle)UnitType).AttackPower);
                newUnit.Load(matchInfo.GameInfo.Content);
                unitVehicleList.Add(newUnit);
            }
        }

        public void ClearSelections()
        {
            selectedUnitList.Clear();
            selectedUnitInHud = null;
            selectedBuildingOnMap = null;
            selectedBuildingInHud = null;
        }

        public bool DecreaseCredits(int amount)
        {
            if (creditAmount - amount > 0) 
            {
                creditAmount -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        // Returns a list of objects based on UnitBase located at the position.
        /// </summary>
        public List<UnitBase> IsPositionAvailable(Vector2 testPostition, Vector2 extent)
        {
            List<UnitBase> CollidingUnits = new List<UnitBase>();

            for (int i = 0; i < UnitBuildingList.Count; i++)
            {
                if (Square2DCollide(testPostition, extent, UnitBuildingList[i].Position, UnitBuildingList[i].Size))
                {
                    //Trace.Write(" Building found ");
                    CollidingUnits.Add(UnitBuildingList[i]);
                }                
            }
                        
            for (int i = 0; i < UnitHumanOidList.Count; i++)
            {
                if (Square2DCollide(testPostition, extent, UnitHumanOidList[i].Position, UnitHumanOidList[i].Size))
                {
                    //Trace.Write(" Human found ");
                    CollidingUnits.Add(UnitHumanOidList[i]);
                }                
            }

            for (int i = 0; i < UnitVehicleList.Count; i++)
            {
                if (Square2DCollide(testPostition, extent, UnitVehicleList[i].Position, UnitVehicleList[i].Size))
                {
                    //Trace.Write(" Vehicle found ");
                    CollidingUnits.Add(UnitVehicleList[i]);
                }                
            }

            return CollidingUnits;
        }

        public List<UnitBase> ObjectsWithinRadius(Vector2 testPostition, float radius)
        {
            List<UnitBase> CollidingUnits = new List<UnitBase>();

            // todo .. our other object its radius is not available. Calculate an radius which is +- overlaps the building area.
            //          We actually need something which can see if the radius overlaps another building/unit.

            for (int i = 0; i < UnitBuildingList.Count; i++)
            {
                if (Radius2DCollide(testPostition, radius, UnitBuildingList[i].Position, UnitBuildingList[i].Size.X)) // default use X but it isn't compatible with the other object.
                {
                    //Trace.Write(" Building found ");
                    CollidingUnits.Add(UnitBuildingList[i]);
                }
            }

            for (int i = 0; i < UnitHumanOidList.Count; i++)
            {
                if (Radius2DCollide(testPostition, radius, UnitHumanOidList[i].Position, UnitHumanOidList[i].Size.X))
                {
                    //Trace.Write(" Human found ");
                    CollidingUnits.Add(UnitHumanOidList[i]);
                }
            }

            for (int i = 0; i < UnitVehicleList.Count; i++)
            {
                if (Radius2DCollide(testPostition, radius, UnitVehicleList[i].Position, UnitVehicleList[i].Size.X))
                {
                    //Trace.Write(" Vehicle found ");
                    CollidingUnits.Add(UnitVehicleList[i]);
                }
            }

            return CollidingUnits;
        }

        /// <summary>
        // 2D Square Object-to-object bounding-box collision detection
        /// </summary>
        public bool Square2DCollide(Vector2 Position1, Vector2 extent1, Vector2 Position2, Vector2 extent2) 
        {
            float left1, left2;
            float right1, right2;
            float top1, top2;
            float bottom1, bottom2;

            left1 = Position1.X;
            left2 = Position2.X;
            right1 = Position1.X + extent1.X;
            right2 = Position2.X + extent2.X;
            top1 = Position1.Y;
            top2 = Position2.Y;
            bottom1 = Position1.Y + extent1.Y;
            bottom2 = Position2.Y + extent2.Y;

            if (bottom1 < top2) return false;
            if (top1 > bottom2) return false;

            if (right1 < left2) return false;
            if (left1 > right2) return false; //&& right1 > left2

            return true;
        }

        /// <summary>
        // 2D Radius Object-to-object collision detection
        // Collision( Actor *a, Actor *b )
        // float r=(float)sqrt((a->x - b->x) * (a->x - b->x) + (a->y - b->y) * (a->y - b->y))
        /// </summary>
        public bool Radius2DCollide(Vector2 Position1, float radius1, Vector2 Position2, float radius2)
        {
            // some info for usage of radius
            // void Collision( Actor *a, Actor *b )
            // float r=(float)sqrt((a->x - b->x) * (a->x - b->x) + (a->y - b->y) * (a->y - b->y));
            // if( r<(a->colr+b->colr) )
            float r = (float)Math.Sqrt((Position1.X - Position2.X) * (Position1.X - Position2.X) + 
                                       (Position1.Y - Position2.Y) * (Position1.Y - Position2.Y));

            if (r < (radius1 + radius2))
                return true;

            return false;
        }

        public void Update(GameTime gameTime)
        {
            for(int i=0; i<unitHumanOidList.Count; i++)
                unitHumanOidList[i].Update(gameTime);

            for(int i=0; i<unitVehicleList.Count; i++)
                unitVehicleList[i].Update(gameTime);

            for(int i=0; i<unitBuildingList.Count; i++)
                unitBuildingList[i].Update(gameTime);

            for (int i = 0; i < projectileList.Count; i++)
                projectileList[i].Update(gameTime);

            // check for win or lose
            if (CheckWin())
            {
                // the player won, notify of win.

            }
            else if (CheckLose())
            {
                // the player lost, be removed from game.

            }

        }

        public void Draw(SpriteBatch batch, float time)
        {
            batch.Begin();
            for (int i = 0; i < unitHumanOidList.Count; i++)
                unitHumanOidList[i].Draw(batch, time);

            for (int i = 0; i < unitVehicleList.Count; i++)
                unitVehicleList[i].Draw(batch, time);
            
            batch.End();

            batch.Begin();
            for (int i = 0; i < unitBuildingList.Count; i++)
                unitBuildingList[i].Draw(batch, time);

            batch.End();

            batch.Begin();
            for (int i = 0; i < projectileList.Count; i++)
                projectileList[i].Draw(batch, time);

            batch.End();
        }

        public void UpdateUnitPositions()
        {
            UpdateUnitPositionsVehicle();
            UpdateUnitPositionsHumanOid();            
        }

        public void UpdateUnitPositionsVehicle()
        {
            for (int i = 0; i < unitVehicleList.Count; i++)
            {
                Vector2 position = Vector2.Zero;
                position.X = unitVehicleList[i].Position.X * unitVehicleList[i].Scale.X;
                position.Y = unitVehicleList[i].Position.Y * unitVehicleList[i].Scale.Y;

                Vector2 worldOffset = new Vector2(0, 0);// layer.Offset;
                Vector2 cameraPosition = matchInfo.GameInfo.Camera.Position;
                Vector2 scaleValue = unitVehicleList[i].Scale;
                float zoomValue = matchInfo.GameInfo.Camera.Zoom;

                // Offset the positions by the word position of the tile grid 
                // this is the actual position of the tile in world coordinates.
                Vector2.Add(ref position, ref worldOffset, out position);

                // Now, we get the camera position relative to the tile's position
                Vector2.Subtract(ref cameraPosition, ref position, out position);

                // Get the tile's final size (note that scaling is done after 
                // determining the position)
                Vector2 scale;
                Vector2.Multiply(ref scaleValue, zoomValue, out scale);

                unitVehicleList[i].DrawingPosition = position + new Vector2(400, 300);
                unitVehicleList[i].DrawingScale = scale;
                unitVehicleList[i].screenCenter = matchInfo.GameInfo.ScreenCenter;
            }       
        }

        public void UpdateUnitPositionsHumanOid()
        {
            for (int i = 0; i < unitHumanOidList.Count; i++)
            {
                Vector2 position = Vector2.Zero;
                position.X = unitHumanOidList[i].Position.X * unitHumanOidList[i].Scale.X;
                position.Y = unitHumanOidList[i].Position.Y * unitHumanOidList[i].Scale.Y;

                Vector2 worldOffset = new Vector2(0, 0);// layer.Offset;
                Vector2 cameraPosition = matchInfo.GameInfo.Camera.Position;
                Vector2 scaleValue = unitHumanOidList[i].Scale;
                float zoomValue = matchInfo.GameInfo.Camera.Zoom;

                // Offset the positions by the word position of the tile grid 
                // this is the actual position of the tile in world coordinates.
                Vector2.Add(ref position, ref worldOffset, out position);

                // Now, we get the camera position relative to the tile's position
                Vector2.Subtract(ref cameraPosition, ref position, out position);

                // Get the tile's final size (note that scaling is done after 
                // determining the position)
                Vector2 scale;
                Vector2.Multiply(ref scaleValue, zoomValue, out scale);

                unitHumanOidList[i].DrawingPosition = position + new Vector2(400, 300);
                unitHumanOidList[i].DrawingScale = scale;
                unitHumanOidList[i].screenCenter = matchInfo.GameInfo.ScreenCenter;
            }                   
        }

        public void UpdateBuildingPositions()
        {
            for (int i = 0; i < unitBuildingList.Count; i++)
            {                
                Vector2 position = Vector2.Zero;
                position.X = unitBuildingList[i].Position.X * unitBuildingList[i].Scale.X;
                position.Y = unitBuildingList[i].Position.Y * unitBuildingList[i].Scale.Y;

                Vector2 worldOffset = new Vector2(0, 0);// layer.Offset;
                Vector2 cameraPosition = matchInfo.GameInfo.Camera.Position;
                Vector2 scaleValue = unitBuildingList[i].Scale;
                float zoomValue = matchInfo.GameInfo.Camera.Zoom;

                // Offset the positions by the word position of the tile grid 
                // this is the actual position of the tile in world coordinates.
                Vector2.Add(ref position, ref worldOffset, out position);

                // Now, we get the camera position relative to the tile's position
                Vector2.Subtract(ref cameraPosition, ref position, out position);

                // Get the tile's final size (note that scaling is done after 
                // determining the position)
                Vector2 scale;
                Vector2.Multiply(ref scaleValue, zoomValue, out scale);

                unitBuildingList[i].DrawingPosition = position + new Vector2(400, 300);
                unitBuildingList[i].DrawingScale = scale;
                unitBuildingList[i].screenCenter = matchInfo.GameInfo.ScreenCenter;
            }            
        }

        public void UpdateProjectilePositions()
        {
            for (int i = 0; i < projectileList.Count; i++)
            {
                Vector2 position = Vector2.Zero;
                position.X = projectileList[i].Position.X * projectileList[i].Scale.X;
                position.Y = projectileList[i].Position.Y * projectileList[i].Scale.Y;

                Vector2 worldOffset = new Vector2(0, 0);// layer.Offset;
                Vector2 cameraPosition = matchInfo.GameInfo.Camera.Position;
                Vector2 scaleValue = projectileList[i].Scale;
                float zoomValue = matchInfo.GameInfo.Camera.Zoom;

                // Offset the positions by the word position of the tile grid 
                // this is the actual position of the tile in world coordinates.
                Vector2.Add(ref position, ref worldOffset, out position);

                // Now, we get the camera position relative to the tile's position
                Vector2.Subtract(ref cameraPosition, ref position, out position);

                // Get the tile's final size (note that scaling is done after 
                // determining the position)
                Vector2 scale;
                Vector2.Multiply(ref scaleValue, zoomValue, out scale);

                projectileList[i].DrawingPosition = position + new Vector2(400, 300);
                projectileList[i].DrawingScale = scale;
                projectileList[i].screenCenter = matchInfo.GameInfo.ScreenCenter;
            }        
        }
        
        public bool RemoveProjectile(ProjectileBase projectile)
        {
            Trace.WriteLine(projectile);
            return projectileList.Remove(projectile);            
        }

        public bool RemoveUnit(UnitBase obj)
        {
            Trace.WriteLine(obj);
            if (unitVehicleList.Remove((Vehicle)obj))
                return true;

            return unitHumanOidList.Remove((HumanOid)obj);            
        }

        public bool RemoveBuilding(UnitBase obj)
        {
            Trace.WriteLine(obj);
            return unitBuildingList.Remove((Building)obj);
        }

        public bool CheckWin()
        {
            if (gameInfo.MatchInfo.OnlySelfLeft() && PlayersEnterdValid)
            {
                return true;
            }

            return false;
        }

        public bool CheckLose()
        {
            //if all buildings are lost the player loses.
            if (unitBuildingList.Count <= 0 && buildingsLoseConditionValid )
                return true;

            return false;
        }

        public int CreditAmount
        {
            get { return creditAmount; }
            set { creditAmount = value; }
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        public Building SelectedBuildingOnMap
        {
            get { return selectedBuildingOnMap; }
            set { selectedBuildingOnMap = value; }
        }

        public Building SelectedBuildingInHud
        {
            get { return selectedBuildingInHud; }
            set { selectedBuildingInHud = value; }
        }
                
        public List<UnitBase> SelectedUnitList
        {
            get { return selectedUnitList; }
            set { selectedUnitList = value; }
        }

        public UnitBase SelectedUnitInHud
        {
            get { return selectedUnitInHud; }
            set { selectedUnitInHud = value; }
        }

        public List<Building> UnitBuildingList
        {
            get { return unitBuildingList; }
            set { unitBuildingList = value; }
        }

        public List<HumanOid> UnitHumanOidList
        {
            get { return unitHumanOidList; }
            set { unitHumanOidList = value; }
        }

        public List<Vehicle> UnitVehicleList
        {
            get { return unitVehicleList; }
            set { unitVehicleList = value; }
        }

        public List<ProjectileBase> ProjectileList
        {
            get { return projectileList; }
            set { projectileList = value; }
        }
                
    }
}
