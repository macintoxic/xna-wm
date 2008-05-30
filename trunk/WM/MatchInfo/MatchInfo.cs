using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using System.Diagnostics;
using WM.Units;

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

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < players.Count; i++)
                players[i].Update(gameTime);
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
        public Vector2 FindAvailableUnitSpawnPosition(Vector2 startLocation, Vector2 extent)
        {
            Vector2 tryLocation = new Vector2(startLocation.X,startLocation.Y);
            for( int i = 0; i< 25; i++ )
            {
                // todo use the size of the image (currently using hard coded values(32,64) add 2 to both as offset so they fit nicely.
                tryLocation.X = startLocation.X + ((i % 5) * 34);   
                /*for (int k = 0; k<players.Count; k++ )
                {
                    if (players[k].IsPositionAvailable(tryLocation))
                        return tryLocation;
                }
                */
                if (IsPositionAvailable(tryLocation, extent).Count == 0)
                    return tryLocation;
                
                if ((i % 5) == 4) 
                    tryLocation.Y += 66;                    
            }
            
            return new Vector2(0,0);
        }

        ///<summary>
        // Loops over the player list and checks every unit based on UnitBase if it resides at the appointed position.
        // returns a list of UnitBase based objects which are located there.
        ///</summary>        
        public List<UnitBase> IsPositionAvailable(Vector2 tryPosition, Vector2 extent)
        {
            List<UnitBase> combinedUnitListFound =  new List<UnitBase>();
            for (int k = 0; k < players.Count; k++)
            {
                List<UnitBase> unitFound = players[k].IsPositionAvailable(tryPosition, extent);
                for(int i=0; i<unitFound.Count; i++)
                    combinedUnitListFound.Add(unitFound[i]);
            }
            return combinedUnitListFound;
        }

        ///<summary>
        // Loops over the player list and checks every unit based on UnitBase if it 
        // resides within the assigned radius it returns it in a list.
        // Skips own units for now.
        ///</summary>        
        public List<UnitBase> AllObjectsWithinRadius(Vector2 tryPosition, float radius)
        {
            List<UnitBase> combinedUnitListFound = new List<UnitBase>();
            for (int k = 0; k < players.Count; k++)
            {
                // Skip own units. For now no reason to also find own units.
                if (players[k] != gameInfo.MyPlayer) 
                {
                    List<UnitBase> unitFound = players[k].ObjectsWithinRadius(tryPosition, radius);
                    for (int i = 0; i < unitFound.Count; i++)
                        combinedUnitListFound.Add(unitFound[i]);
                }
            }
            return combinedUnitListFound;
        }
                
        public GameInfo GameInfo
        {
            get { return gameInfo; }
            set { gameInfo = value; }
        }

    }
}
