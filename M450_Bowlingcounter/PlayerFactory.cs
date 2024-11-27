using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M450_Bowlingcounter
{
    public static class PlayerFactory
    {
        public static List<Player> CreatePlayers()
        {
            List<Player> players = new List<Player>();
            int playerCount = GetPlayerCount();

            for (int i = 1; i <= playerCount; i++)
            {
                string playerName = GetPlayerNameFromConsole(i);
                players.Add(new Player(playerName));
            }

            return players;
        }

        static int GetPlayerCount()
        {
            int playerCount;
            do
            {
                Console.Write("Geben Sie die Anzahl der Spieler ein: ");
            } while (!int.TryParse(Console.ReadLine(), out playerCount) || playerCount <= 1);
            Console.WriteLine($"Anzahl der Spieler: {playerCount}");
            return playerCount;
        }

        public static string GetPlayerNameFromConsole(int playerNumber)
        {
            string playerName;
            do
            {
                Console.Write($"Name des Spielers {playerNumber}: ");
                playerName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(playerName));
            return playerName;
        }
    }

}
