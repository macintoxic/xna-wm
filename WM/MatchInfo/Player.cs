using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using WM.Units;
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

        public Player(GameInfo info)
        {
            gameInfo = info;

            unitHumanOidList = new List<HumanOid>();
            unitVehicleList = new List<Vehicle>();
            unitBuildingList = new List<Building>();
            creditAmount = 10000;
            rank = 0;
            nickName = "";

            selectedUnitList = new List<UnitBase>();
            selectedBuildingOnMap = null;
            selectedBuildingInHud = null;
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
            
        }

        public void CreateUnit(Vector2 Position, UnitBase UnitType)
        {

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


    }
}
