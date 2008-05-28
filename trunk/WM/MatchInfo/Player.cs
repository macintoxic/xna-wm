using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using WM.Units;

namespace WM.MatchInfo
{
    public class Player
    {
        private List<HumanOid> unitHumanOidList;
        private List<Vehicle> unitVehicleList;
        private List<Building> unitBuildingList;
        private int creditAmount;
        private int rank;
        private string nickName;
        private string ip;
        private int port;

        private Building selectedBuildingOnMap;
        private Building selectedBuildingInHud;
        private List<UnitBase> selectedUnitList;


        public Player()
        {
            unitHumanOidList = new List<HumanOid>();
            unitVehicleList = new List<Vehicle>();
            unitBuildingList = new List<Building>();
            creditAmount = 10000;
            rank = 0;
            nickName = "";
            ip = "192.168.0.10";
            port = 4000;

            selectedUnitList = new List<UnitBase>();
            selectedBuildingOnMap = null;
            selectedBuildingInHud = null;
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

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
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
