using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harjoitustyo
{
    class Player
    {
        private readonly int minScore = 0;
        public string PlayerName { get; set; }
        private int playerScore;
        public int PlayerScore
        {
            get
            {
                return playerScore;
            }
            set
            {
                // if score is under 0 then set score equal to minScore
                if (value > 0)
                {
                    playerScore = value;
                }
                else
                {
                    playerScore = minScore;
                }
            }
        }

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

        public int GetScore()
        {
            return PlayerScore;
        }
    }
}
