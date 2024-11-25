using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M450_Bowlingcounter
{
    public class Game
    {
        private readonly List<Player> _players;
        private const int TotalFrames = 10;

        public Game(List<Player> players)
        {
            _players = players;
        }

        public void Start()
        {
            // play 10 Frames
            for (int frameNumber = 1; frameNumber <= TotalFrames; frameNumber++)
            {
                Console.WriteLine($"\n--- Frame {frameNumber} ---");

                foreach (Player player in _players)
                {
                    PlayFrame(player, frameNumber);
                }
            }
        }

        // frame for a single Player
        private void PlayFrame(Player player, int frameNumber)
        {
            Frame playerFrame = new Frame();
            Console.WriteLine($"\n{player.Name} ist an der Reihe:");

            playerFrame.HandleThrows(player, frameNumber);
        }
    }
}
