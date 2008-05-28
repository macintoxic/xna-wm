using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace WM.MatchInfo
{
    public class MatchInfo
    {
        private GameInfo gameInfo;
        private List<Player> players;   // player playing the game
        private string Map;             // the map we are playing
        private string startTime;       // The time the game started
        private int SyncTimeMs;         // used to determine after how many time we need to sync

        public MatchInfo(GameInfo gameInfoObj)
        {
            players = new List<Player>();
            Map = "WvsM";       // or WvsM.xml depends on the loading style.
            startTime = "0";
            SyncTimeMs = 6000;

            gameInfo = gameInfoObj;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void Draw(SpriteBatch spriteBatch, float gameTime)
        {
            for (int i = 0; i < players.Count; i++ )
            {
                players[i].Draw(spriteBatch, gameTime);
            }
        }

        public GameInfo GameInfo
        {
            get { return gameInfo; }
            set { gameInfo = value; }
        }

    }
}
