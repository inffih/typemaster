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

        public Player()
        {

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
