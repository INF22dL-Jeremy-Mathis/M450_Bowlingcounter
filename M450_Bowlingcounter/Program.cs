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

                // Get skill
                Console.WriteLine("\nBitte ihre Wunsch-Strike-Rate zwischen 0 und 100 (%) eingeben:");
                int skill;
                do
                {
                    Console.Write("Eingabe: ");
                } while (!int.TryParse(Console.ReadLine(), out skill) || skill < 0 || skill > 100);

                // Get and Create Players (all have the same skill level)
                List<Player> players = PlayerFactory.CreatePlayers(skill);


                // create and start game
                Game game = new Game(players);
                game.Start();


                // print results
                Console.WriteLine(BowlingTable.GetBowlingTable(players));


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