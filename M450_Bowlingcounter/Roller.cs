using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M450_Bowlingcounter
{
    public class RandomRoller
    {
        private readonly Random _random;

        public RandomRoller()
        {
            _random = new Random();
        }

        // roll a number between 0 and maxPins, with a probability between 0 and 100 % to hit maxPins
        public int Roll(int maxPins, int _strikeProbability = 15)
        {
            if (maxPins == 10 && _random.NextDouble() < Convert.ToDouble((double)_strikeProbability / 100.0))
            {
                return 10;
            }
            return _random.Next(0, maxPins + 1);
        }

        // random chance of throw being a foul
        public bool IsFoul(int foulRate = 5)
        {
            return _random.Next(0, 100) < foulRate;
        }
    }

}
