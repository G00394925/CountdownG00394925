using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown
{
    public class GameStats
    {
        public GameStats() { }

        public GameStats(string player1, string player2, int score1, int score2, string timestamp)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.score1 = score1;
            this.score2 = score2;
            this.timestamp = timestamp;
        }

        public string player1 { get; set; }
        public string player2 { get; set; }
        public int score1 { get; set; }
        public int score2 { get; set; }
        public string timestamp { get; set; } 

    }
}
