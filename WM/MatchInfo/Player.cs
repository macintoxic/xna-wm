using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using WM.Units;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Net;

namespace WM.MatchInfo
{
    public class Player
    {
        private GameInfo gameInfo;
        private List<HumanOid> unitHumanOidList;
        private List<Vehicle> unitVehicleList;
        private List<Building> unitBuildingList;
        private int creditAmount;
        private int rank;
        private string nickName;

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
            creditAmount = 10000;
            rank = 0;
            nickName = "";

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
            Building newBuilding = new Building(Position, BuildingType.Rotation, BuildingType.Scale, BuildingType.TargetRadius, BuildingType.Speed, BuildingType.TextureAsset, BuildingType.Offset);
            newBuilding.Load(matchInfo.GameInfo.Content);
            unitBuildingList.Add(newBuilding);

            Trace.Write("new Building");
        }

        public void CreateUnit(Vector2 Position, UnitBase UnitType)
        {
            if (UnitType.GetType().Name == "HumanOid")
            {
                HumanOid newUnit = new HumanOid(Position, UnitType.Rotation, UnitType.Scale, UnitType.TargetRadius, UnitType.Speed, UnitType.TextureAsset, UnitType.Offset);
                newUnit.Load(matchInfo.GameInfo.Content);
                unitHumanOidList.Add(newUnit);                
            }
            else if (UnitType.GetType().Name == "Vehicle")
            {
                Vehicle newUnit = new Vehicle(Position, UnitType.Rotation, UnitType.Scale, UnitType.TargetRadius, UnitType.Speed, UnitType.TextureAsset, UnitType.Offset);
                newUnit.Load(matchInfo.GameInfo.Content);
                unitVehicleList.Add(newUnit);
            }
        }

        public void ClearSelections()
        {
            selectedUnitList.Clear();
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
        }

        public void UpdateUnitPositions()
        {
            
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

    }
}
