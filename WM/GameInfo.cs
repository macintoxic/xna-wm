using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WM
{
    public class GameInfo
    {
        private WMGame game;

        private ContentManager content;

        public GameInfo(WMGame game)
        {
            this.game = game;
        }

        public void LoadContent()
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content");
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void HandleInput(InputState input)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = game.ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = game.ScreenManager.SpriteBatch;

            // This game has a blue background. Why? Because!
            graphics.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            //spriteBatch.Begin();
            //spriteBatch.DrawString(gameFont, "// TODO", playerPosition, Color.Green);
            //spriteBatch.DrawString(gameFont, "Insert Gameplay Here", enemyPosition, Color.DarkRed);
            //spriteBatch.End();
        }
    }
}
