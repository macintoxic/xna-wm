using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using System.Diagnostics;

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

        ///<summary>
        // returns Vector2(0,0) when no valid spawn location found.
        // Try location loop from a certain beginning toward a certain end.(left top -> right bottom)
        //
        //  | ****บบบบ |    * Building location    
        //  | ****บบบบ |    บ Try spawn locations
        //  | ****บบบบ |
        //  | บบบบบบบบ |
        //  | บบบบบบบบ |
        ///</summary>
        public Vector2 FindAvailableUnitSpawnPosition(Vector2 startLocation)
        {
            Vector2 tryLocation = new Vector2(startLocation.X,startLocation.Y);
            for( int i = 0; i< 25; i++ )
            {
                // todo use the size of the image (currently using hard coded values(32,64) add 2 to both as offset so they fit nicely.
                tryLocation.X = startLocation.X + ((i % 5) * 34);   
                for (int k = 0; k<players.Count; k++ )
                {
                    if (players[k].IsPositionAvailable(tryLocation))
                        return tryLocation;
                }
                
                if ((i % 5) == 4) 
                    tryLocation.Y += 66;                    
            }
            
            return new Vector2(0,0);
        }


        public GameInfo GameInfo
        {
            get { return gameInfo; }
            set { gameInfo = value; }
        }

    }
}
