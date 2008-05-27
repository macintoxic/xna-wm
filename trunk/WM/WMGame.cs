using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using WM.Screens;

namespace WM
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WMGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// The graphics device manager used to draw everything.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The screen manager is used to manage, right, screens.
        /// </summary>
        private ScreenManager screenManager;

        private GameInfo gameInfo;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public WMGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            gameInfo = new GameInfo(this);

            // Create the screen manager component.
            screenManager = new ScreenManager(this, gameInfo);
            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }

        public GameInfo GameInfo
        {
            get { return gameInfo; }
        }

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }
}
