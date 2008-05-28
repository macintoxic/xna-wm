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

        public MouseControl(GameInfo GameInfoObj)
        {
            gameInfo = GameInfoObj;
        }


        public void HandleMouseInput(float elapsed)
        {
            MouseState current_mouse = Mouse.GetState();

            // The mouse x and y positions are returned relative to the
            // upper-left corner of the game window.
            int mouseX = current_mouse.X;
            int mouseY = current_mouse.Y;
            int leftMouse = (int)current_mouse.LeftButton;
            int rightMouse = (int)current_mouse.RightButton;

            // If clicked see if we should process an action.
            if (rightMouse == 1)
            {
                // Clear all selections
                ClearSelections(gameInfo.MyPlayer);

            }
            if (leftMouse == 1)
            {
                // See if we should produce unit stuff
                TryUnitProduction(gameInfo.MyPlayer);

                // See if we should build a building
                TryBuildingProduction(gameInfo.MyPlayer, new Vector2(mouseX, mouseY));

                // See if there are units selected which should move toward an destination.
                TryMovement(gameInfo.MyPlayer);
            }

            // Change background color based on mouse position.
            //Color backColor = new Color((byte)(mouseX / 3), (byte)(mouseY / 2), 0);
            //game.ScreenManager.GraphicsDevice.Clear(backColor); 
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

            Trace.WriteLine(worldPosition);

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
            if (player.SelectedBuildingInHud != null)
            {                
                Building BuildingToBuild = (Building)player.SelectedBuildingInHud;
                Vector2 TargetedPositionOnMap = DeterminePositionInWorld(mousePosition); // todo find location
                if (player.DecreaseCredits(BuildingToBuild.CreditsCost))
                {
                    player.CreateBuilding(TargetedPositionOnMap, BuildingToBuild);
                }
            }
        }

        public void TryMovement(Player player)
        {

        }


    }
}
