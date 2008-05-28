using System;
using System.Collections.Generic;
using System.Text;

namespace WM.MatchInfo
{
    public class MatchInfo
    {        
        private List<Player> players;   // player playing the game
        private string Map;             // the map we are playing
        private string startTime;       // The time the game started
        private int SyncTimeMs;         // used to determine after how many time we need to sync

        public MatchInfo()
        {
            players = new List<Player>();
            Map = "WvsM";       // or WvsM.xml depends on the loading style.
            startTime = "0";
            SyncTimeMs = 6000;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

    }
}
