using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WM
{
    /// <summary>
    /// A static class to manage the fonts used in the game.
    /// </summary>
    public static class Fonts
    {
        private static SpriteFont moneyFont;

        private static Color moneyFontColor;

        public static void LoadContent(ContentManager content)
        {
            moneyFont = content.Load<SpriteFont>("Fonts\\Money");

            moneyFontColor = Color.White;
        }

        public static void UnloadContent(ContentManager content)
        {
            moneyFont = null;
        }

        public static SpriteFont MoneyFont
        {
            get { return moneyFont; }
        }

        public static Color MoneyFontColor
        {
            get { return moneyFontColor; }
        }
    }
}
