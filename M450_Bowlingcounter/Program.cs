using System;
using System.Collections.Generic;
using System.Text;

namespace M450_Bowlingcounter
{
    internal class Program
    {
        static void Main()
        {
            int input;
            do
            {
                Console.Clear();
                Console.WriteLine("Bowling-Zähler gestartet!");

                // Get and Create Players (all have the same skill level)
                List<Player> players = PlayerFactory.CreatePlayers();


                // create and start game
                Game game = new Game(players);
                game.Start();


                // print results
                Console.WriteLine(ScoreBoard.GetScoreBoard(players));


                // ask for replay or exit
                Console.WriteLine("\nDrücken Sie 0, um zu beenden, oder 1, um erneut zu spielen.");
                do
                {
                    Console.Write("Eingabe: ");
                } while (!int.TryParse(Console.ReadLine(), out input) || (input != 0 && input != 1));

            } while (input != 0);
        }

       
    }
}