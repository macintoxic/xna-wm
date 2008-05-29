using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WM
{
    public delegate void HudElementClick(Hud hud, HudElementType element);

    public enum HudElementType
    {
        BuildSoldier,
        BuildTank,
        BuildHQ,
        BuildBarrack,
        BuildWarFactory
    }

    public class HudElement
    {
        public int SpriteIndex;
        public Vector2 Position;
        public HudElementType HudElementType;

        public HudElement(int spriteIndex, Vector2 pos, HudElementType type)
        {
            Position = pos;
            SpriteIndex = spriteIndex;
            HudElementType = type;
        }
    }

    public class Hud
    {
        MouseState lastMouseState;
        MouseState currMouseState;

        private Texture2D texture;
        private SpriteSheet spriteSheet;

        public event HudElementClick HudElementClick;

        private List<HudElement> hudElementsLine1;
        private List<HudElement> hudElementsLine2;

        private GameInfo gameInfo;

        public Hud(GameInfo aGameInfo)
        {
            gameInfo = aGameInfo;
        }

        public virtual void OnHudElementClick(HudElementType type)
        {
            if (HudElementClick != null)
                HudElementClick(this, type);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures\\HUD");
            spriteSheet = new SpriteSheet(texture);

            spriteSheet.AddSourceSprite(0, new Rectangle(  0, 0, 64, 64));
            spriteSheet.AddSourceSprite(1, new Rectangle(64,  0, 64, 64));
            spriteSheet.AddSourceSprite(2, new Rectangle(128, 0, 64, 64));
            spriteSheet.AddSourceSprite(3, new Rectangle(192, 0, 64, 64));
            spriteSheet.AddSourceSprite(4, new Rectangle(256, 0, 64, 64));

            hudElementsLine1 = new List<HudElement>();
            hudElementsLine2 = new List<HudElement>();

            //hudElementsLine1.Add(new HudElement(0, new Vector2(0, 0), HudElementType.BuildSoldier));
            //hudElementsLine1.Add(new HudElement(1, new Vector2(64, 0), HudElementType.BuildTank));
            hudElementsLine2.Add(new HudElement(2, new Vector2(0, 0), HudElementType.BuildBarrack));
            hudElementsLine2.Add(new HudElement(3, new Vector2(64, 0), HudElementType.BuildHQ));
            hudElementsLine2.Add(new HudElement(4, new Vector2(128, 0), HudElementType.BuildWarFactory));
        }

        public void UnloadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            lastMouseState = currMouseState;
            currMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released &&
                currMouseState.LeftButton == ButtonState.Pressed)
            {
                int mouseX = currMouseState.X;
                int mouseY = currMouseState.Y;

                if (mouseY > graphics.Viewport.Height - 64)
                {
                    foreach (HudElement el in hudElementsLine2)
                    {
                        if (mouseX > el.Position.X &&
                            mouseX < el.Position.X + 64)
                        {
                            OnHudElementClick(el.HudElementType);
                            return;
                        }
                    }
                }
                else if (mouseY > graphics.Viewport.Height - 128)
                {
                    foreach (HudElement el in hudElementsLine1)
                    {
                        if (mouseX > el.Position.X &&
                            mouseX < el.Position.X + 64)
                        {
                            OnHudElementClick(el.HudElementType);
                            return;
                        }
                    }
                }
            }

            // When a building is selected generate unit menu belonging to the building.
            if (gameInfo.MyPlayer.SelectedBuildingOnMap != null)
            {
                ClearUnitHud();
                GenerateUnitHud();
            }
            else if (hudElementsLine1.Count > 0)
                ClearUnitHud();
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();

            for (int i = 0; i < hudElementsLine1.Count; ++i)
            {
                HudElement el = hudElementsLine1[i];

                Vector2 position = el.Position;
                position.Y = graphics.Viewport.Height - 128;
                
                Rectangle sourceRect = Rectangle.Empty;
                spriteSheet.GetRectangle(ref el.SpriteIndex, out sourceRect);

                batch.Draw(
                    texture,
                    position,
                    sourceRect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f);
            }

            for (int i = 0; i < hudElementsLine2.Count; ++i)
            {
                HudElement el = hudElementsLine2[i];

                Vector2 position = el.Position;
                position.Y = graphics.Viewport.Height - 64;

                Rectangle sourceRect = Rectangle.Empty;
                spriteSheet.GetRectangle(ref el.SpriteIndex, out sourceRect);

                batch.Draw(
                    texture,
                    position,
                    sourceRect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f);
            }


            batch.End();
        }
    
        public void GenerateUnitHud()
        {
            if (hudElementsLine1.Count == 0)
            {
                if (gameInfo.MyPlayer.SelectedBuildingOnMap.GetProductionUnit() != null)
                {
                    if (gameInfo.MyPlayer.SelectedBuildingOnMap.GetProductionUnit().Name == "Soldier")
                        hudElementsLine1.Add(new HudElement(0, new Vector2(0, 0), HudElementType.BuildSoldier));

                    if (gameInfo.MyPlayer.SelectedBuildingOnMap.GetProductionUnit().Name == "Tank")
                        hudElementsLine1.Add(new HudElement(1, new Vector2(64, 0), HudElementType.BuildTank));
                }
            }
        }

        public void ClearUnitHud()
        {
            hudElementsLine1.Clear();
        }

    }
}
