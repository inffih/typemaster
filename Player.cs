using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harjoitustyo
{
    class Player
    {
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }

        // default constructor
        public Player(){}

        // parametric constructor
        public Player(string playerName, int playerScore)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
        }

        public void AddPoints(int points)
        {
            PlayerScore = PlayerScore + points;
        }

        public void DecreasePoints(int points)
        {
            PlayerScore = PlayerScore - points;
        }
    }
}
