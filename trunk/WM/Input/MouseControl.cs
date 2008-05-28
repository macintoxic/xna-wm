using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using WM.MatchInfo;
using WM.Units;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace WM.Input
{
    class MouseControl
    {
        private GameInfo gameInfo;
        //private bool BuildingCreatedThisTurn;
        private MouseState prevMouseState;
        private MouseState currentMouseState;

        public MouseControl(GameInfo GameInfoObj)
        {
            gameInfo = GameInfoObj;
        }


        public void HandleMouseInput(float elapsed)
        {
            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // The mouse x and y positions are returned relative to the
            // upper-left corner of the game window.
            int mouseX = currentMouseState.X;
            int mouseY = currentMouseState.Y;
            int leftMouse = (int)currentMouseState.LeftButton;
            int rightMouse = (int)currentMouseState.RightButton;

            // If RightMouse released see if we should process an action.
            if (prevMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                // Clear all selections
                ClearSelections(gameInfo.MyPlayer);
            }
            // If LeftMouse released see if we should process an action.
            if (prevMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // First find out if the mouse is over the HUD
                // Hud screen pos from XY: 0,472 to XY: 800,600

                if (mouseY >= 600-128 )
                { // do nothing we are in the HUD zone, Hud is handled differently elsewhere
                }
                else
                {
                    // See if we should produce unit stuff.
                    TryUnitProduction(gameInfo.MyPlayer);

                    // See if we should build a building.
                    TryBuildingProduction(gameInfo.MyPlayer, new Vector2(mouseX, mouseY));

                    // See if there are units selected which should move toward an destination.
                    TryMovement(gameInfo.MyPlayer);

                    // See if there is anything the player should select.
                    TrySelection(gameInfo.MyPlayer, new Vector2(mouseX, mouseY));
                }
            }
        }

        public void ClearSelections(Player player)
        {
            player.ClearSelections();
        }

        public Vector2 DeterminePositionInWorld(Vector2 mousePosition)
        {
            Vector2 worldPosition = new Vector2();
            
            // Determine position
            worldPosition = gameInfo.Camera.Position;
            worldPosition += mousePosition;

            return worldPosition;
        }


        public void TryUnitProduction(Player player)
        {
            if (player.SelectedBuildingOnMap != null)
            {
                Building TargetBuilding = (Building)player.SelectedBuildingOnMap;
                if ( player.DecreaseCredits(TargetBuilding.CreditsCost) )
                {
                    player.CreateUnit(TargetBuilding.GetUnitSpawnPosition(), TargetBuilding.GetProductionUnit());
                }
            }
        }

        public void TryBuildingProduction(Player player, Vector2 mousePosition)
        {
            if (player.SelectedBuildingInHud != null )//&& BuildingCreatedThisTurn == false)
            {                
                Building BuildingToBuild = (Building)player.SelectedBuildingInHud;
                Vector2 TargetedPositionOnMap = DeterminePositionInWorld(mousePosition);
                if (player.DecreaseCredits(BuildingToBuild.CreditsCost))
                {
                    //BuildingCreatedThisTurn = true;
                    player.CreateBuilding(TargetedPositionOnMap, BuildingToBuild);
                }
            }
        }

        public void TryMovement(Player player)
        {

        }

        public void TrySelection(Player player, Vector2 mousePosition)
        {
            // See if anything on screen position is selectable (HUD items).
            //player.SelectedBuildingInHud
            //mousePosition

            // See if anything at the world position is selectable.
            Vector2 worldPosition = DeterminePositionInWorld(mousePosition);           
            // todo           
            //player.SelectedBuildingOnMap
            //player.SelectedUnitList.Add();

        }

        public void Update()
        {
            //BuildingCreatedThisTurn = false;
        }


    }
}
