using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using WM.MatchInfo;
using WM.Units;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace WM.Input
{
    class MouseControl
    {
        private GameInfo gameInfo;
        //private GraphicsDevice graphicsDevice;
        //private bool BuildingCreatedThisTurn;
        private MouseState prevMouseState;
        private MouseState currentMouseState;
        private Vector2 mouseLocation;
        private Vector2 SelectionAreaStart;
        private bool bSHouldDrawSelection;

        // a block of vertices used to draw selection area,
        // and will determine how many primitives to draw from positionInBuffer.
        private VertexPositionColor[] vertices;
        private VertexDeclaration vertexDeclaration;
        private BasicEffect basicEffect;

        public MouseControl(GameInfo GameInfoObj)
        {
            gameInfo = GameInfoObj;
            vertices = new VertexPositionColor[8]; // 8 default buffsersize, we need 4 lines for selection area
            mouseLocation = new Vector2(0.0f, 0.0f);
            bSHouldDrawSelection = false;
        }


        public void HandleMouseInput(float elapsed)
        {
            bSHouldDrawSelection = false;
            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // The mouse x and y positions are returned relative to the
            // upper-left corner of the game window.
            //int mouseX = currentMouseState.X;
            //int mouseY = currentMouseState.Y;
            mouseLocation = new Vector2(currentMouseState.X, currentMouseState.Y);
            int leftMouse = (int)currentMouseState.LeftButton;
            int rightMouse = (int)currentMouseState.RightButton;

            // If RightMouse released see if we should process an action.
            if (prevMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                // Clear all selections
                ClearSelections(gameInfo.MyPlayer);
            }
            // If LeftMouse pressed show selection area.
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // Store start point of selection area.
                if (prevMouseState.LeftButton == ButtonState.Released)
                {
                    SelectionAreaStart = mouseLocation;
                }
                bSHouldDrawSelection = true;
            }
            // If LeftMouse released see if we should process an action.
            if (prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                // First find out if the mouse is over the HUD
                // Hud screen pos from XY: 0,472 to XY: 800,600

                if (mouseLocation.Y >= 600-128 &&
                    Math.Abs(mouseLocation.X - SelectionAreaStart.X) < 30 &&
                    Math.Abs(mouseLocation.Y - SelectionAreaStart.Y) < 30)
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
            Vector2 worldPositionStart;
            Vector2 worldExtent;
            bool bSelectionChanged = false;

            // Did a area select occur or just a click.
            if (Math.Abs(mousePosition.X - SelectionAreaStart.X) > 30 || Math.Abs(mousePosition.Y - SelectionAreaStart.Y) > 30)
            {
                // Clear selection when doing a area select.
                ClearSelections(player);

                // Determine world selection start and end points.
                worldPositionStart = DeterminePositionInWorld(SelectionAreaStart);
                Vector2 worldPositionEnd = DeterminePositionInWorld(mousePosition);
                worldExtent = mousePosition - SelectionAreaStart;

                // re-order selection so it simulates a selection from left top to right bottom. (required for correct Square2DCollide collision test)
                worldExtent.X = worldPositionStart.X > worldPositionEnd.X ? worldPositionStart.X - worldPositionEnd.X : worldExtent.X;
                worldExtent.Y = worldPositionStart.Y > worldPositionEnd.Y ? worldPositionStart.Y - worldPositionEnd.Y : worldExtent.Y;
                worldPositionStart.X = worldPositionStart.X < worldPositionEnd.X ? worldPositionStart.X : worldPositionEnd.X;
                worldPositionStart.Y = worldPositionStart.Y < worldPositionEnd.Y ? worldPositionStart.Y : worldPositionEnd.Y;                
            }
            else
            {
                // See if anything at the world position is selectable.
                worldPositionStart = DeterminePositionInWorld(mousePosition);
                worldExtent = new Vector2(1, 1);
                //Trace.WriteLine(worldPosition);
            }
                        
            for (int i = 0; i < player.UnitHumanOidList.Count; i++)
            {
                if (player.Square2DCollide(worldPositionStart, worldExtent, player.UnitHumanOidList[i].Position, player.UnitHumanOidList[i].Size))
                {
                    player.SelectedBuildingOnMap = null;
                    player.SelectedUnitList.Add(player.UnitHumanOidList[i]);
                    bSelectionChanged = true;
                }
            }

            for (int i = 0; i < player.UnitVehicleList.Count; i++)
            {
                if (player.Square2DCollide(worldPositionStart, worldExtent, player.UnitVehicleList[i].Position, player.UnitVehicleList[i].Size))
                {
                    player.SelectedBuildingOnMap = null;
                    player.SelectedUnitList.Add(player.UnitVehicleList[i]);
                    bSelectionChanged = true;
                }
            }

            // determine if any units were selected, if so do not(skip) select buildings.
            if (!bSelectionChanged)
            {
                for (int i = 0; i < player.UnitBuildingList.Count; i++)
                {
                    if (player.Square2DCollide(worldPositionStart, worldExtent, player.UnitBuildingList[i].Position, player.UnitBuildingList[i].Size))
                    {
                        player.SelectedBuildingOnMap = player.UnitBuildingList[i];
                        bSelectionChanged = true;
                    }
                }
            }

            // see if units or buildings were selected, if so return true.
            if (bSelectionChanged)
                return true;

            return false;
        }

        public void Update()
        {
            //BuildingCreatedThisTurn = false;
        }

        public void Draw()
        {
            DrawSelectionArea();
        }

        public void DrawSelectionArea()
        {
            if (bSHouldDrawSelection)
            {
                GraphicsDevice graphicsDevice = gameInfo.Game.ScreenManager.GraphicsDevice;

                // Setup VertexDeclaration
                // create a vertex declaration, which tells the graphics card what kind of
                // data to expect during a draw call. We're drawing using
                // VertexPositionColors, so we'll use those vertex elements.
                if (vertexDeclaration == null)
                    vertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);

                // Setup BasicEffect
                if (basicEffect == null)
                    basicEffect = new BasicEffect(graphicsDevice, null);

                basicEffect.VertexColorEnabled = true;
                // projection uses CreateOrthographicOffCenter to create 2d projection
                // matrix with 0,0 in the upper left.
                basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                    (0, graphicsDevice.Viewport.Width,
                    graphicsDevice.Viewport.Height, 0,
                    0, 1);

                // Setup graphicsDevice
                graphicsDevice.VertexDeclaration = vertexDeclaration;

                // how many verts will each of these primitives require?
                int numVertsPerPrimitive = 2;// PrimitiveType.LineList = 2
                int positionInBuffer = 0;

                // Create Selection area
                Color color = new Color(0, 255, 0);
                List<Vector2> vertex =  new List<Vector2>();
                vertex.Add(SelectionAreaStart);
                vertex.Add(new Vector2(mouseLocation.X, SelectionAreaStart.Y));
                vertex.Add(new Vector2(mouseLocation.X, SelectionAreaStart.Y));
                vertex.Add(mouseLocation);
                vertex.Add(mouseLocation);
                vertex.Add(new Vector2(SelectionAreaStart.X, mouseLocation.Y));
                vertex.Add(new Vector2(SelectionAreaStart.X, mouseLocation.Y));
                vertex.Add(SelectionAreaStart);

                for (int i = 0; i < vertex.Count; i++ )
                {
                    vertices[positionInBuffer].Position = new Vector3(vertex[i], 0.0f);
                    vertices[positionInBuffer].Color = color;
                    positionInBuffer++;
                }

                // how many primitives will we draw?
                int primitiveCount = positionInBuffer / numVertsPerPrimitive;
                                
                // Start drawing
                basicEffect.Begin();
                basicEffect.CurrentTechnique.Passes[0].Begin();

                graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, primitiveCount);
                                
                basicEffect.CurrentTechnique.Passes[0].End();
                basicEffect.End();
            }
        }


    }
}
