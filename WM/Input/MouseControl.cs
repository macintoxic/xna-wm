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
            //int mouseX = currentMouseState.X;
            //int mouseY = currentMouseState.Y;
            Vector2 mouseLocation = new Vector2(currentMouseState.X, currentMouseState.Y);
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

                if (mouseLocation.Y >= 600-128 )
                { // do nothing we are in the HUD zone, Hud is handled differently elsewhere
                }
                else
                {
                    // See if there is anything the player should select.
                    if ( !TrySelection(gameInfo.MyPlayer, mouseLocation) )
                    {
                        // See if we should produce unit stuff.
                        //TryUnitProduction(gameInfo.MyPlayer); // should be swapped to the HUD.

                        // See if we should build a building.
                        TryBuildingProduction(gameInfo.MyPlayer, mouseLocation);

                        // See if there are units selected which should move toward an destination.
                        TryMovement(gameInfo.MyPlayer, mouseLocation);
                    }
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
                // verify if there is a valid unit to produce and if the play has enough credits
                if ( TargetBuilding.GetProductionUnit() != null )
                {
                    Vector2 spawnPosition = TargetBuilding.GetUnitSpawnPosition(gameInfo);
                    if (spawnPosition.X != 0
                        && spawnPosition.Y != 0
                        && player.DecreaseCredits(TargetBuilding.CreditsCost))
                    {
                        player.CreateUnit(TargetBuilding.GetUnitSpawnPosition(gameInfo), TargetBuilding.GetProductionUnit());
                    }                    
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
                    ClearSelections(player);
                    player.CreateBuilding(TargetedPositionOnMap, BuildingToBuild);
                }
            }
        }

        public void TryMovement(Player player, Vector2 mousePosition)
        {
            // Move unit selection toward this point
            for (int i = 0; i < player.SelectedUnitList.Count; i++)
                player.SelectedUnitList[i].SetMoveTargetPosition(DeterminePositionInWorld(mousePosition));
        }

        public bool TrySelection(Player player, Vector2 mousePosition)
        {
            // See if anything at the world position is selectable.
            Vector2 worldPosition = DeterminePositionInWorld(mousePosition);
            //Trace.WriteLine(worldPosition);

            // todo performance update, maybe break when building found ??
            for (int i = 0; i < player.UnitBuildingList.Count; i++)
            {
                if ( player.Square2DCollide(worldPosition, new Vector2(1, 1), player.UnitBuildingList[i].Position, player.UnitBuildingList[i].Size) )
                {
                    player.SelectedBuildingOnMap = player.UnitBuildingList[i];
                }             
            }
                        
            for (int i = 0; i < player.UnitHumanOidList.Count; i++)
            {
                if (player.Square2DCollide(worldPosition, new Vector2(1, 1), player.UnitHumanOidList[i].Position, player.UnitHumanOidList[i].Size))
                {
                    player.SelectedUnitList.Add( player.UnitHumanOidList[i] );
                }   
            }
                        
            for (int i = 0; i < player.UnitVehicleList.Count; i++)
            {
                if (player.Square2DCollide(worldPosition, new Vector2(1, 1), player.UnitVehicleList[i].Position, player.UnitVehicleList[i].Size))
                {
                    player.SelectedUnitList.Add(player.UnitVehicleList[i]);
                }
            }


            return false;
        }

        public void Update()
        {
            //BuildingCreatedThisTurn = false;
        }


    }
}
