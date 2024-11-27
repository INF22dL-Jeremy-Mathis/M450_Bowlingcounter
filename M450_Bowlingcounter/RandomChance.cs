using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M450_Bowlingcounter
{
    public class RandomChance
    {
        private readonly Random _random;

        public RandomChance()
        {
            _random = new Random();
        }

        // random chance of throw being a foul
        public bool IsFoul(int foulRate = 5)
        {
            return _random.Next(0, 100) < foulRate;
        }
    }

}
