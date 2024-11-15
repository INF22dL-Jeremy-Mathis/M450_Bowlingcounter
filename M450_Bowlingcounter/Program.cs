using System;
using System.Collections.Generic;

namespace M450_Bowlingcounter
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Bowling-Zähler gestartet!");

            // Spieler erstellen
            Console.Write("Geben Sie die Anzahl der Spieler ein: ");
            int playerCount = int.Parse(Console.ReadLine());
            List<Player> players = new List<Player>();
            for (int i = 1; i <= playerCount; i++)
            {
                Console.Write($"Name des Spielers {i}: ");
                string playerName = Console.ReadLine();
                players.Add(new Player(playerName));
            }

            // Spiel erstellen und starten
            Game game = new Game(players);
            game.Start();

            // Endstand anzeigen
            Console.WriteLine("\n--- Endstand ---");
            foreach (Player player in players)
            {
                Console.WriteLine($"{player.Name}: {player.TotalScore} Punkte");
            }
            Console.ReadKey();
        }
    }

    public class Game
    {
        private List<Player> Players;
        private const int TotalFrames = 10;

        public Game(List<Player> players)
        {
            Players = players;
        }

        public void Start()
        {
            for (int frameNumber = 1; frameNumber <= TotalFrames; frameNumber++)
            {
                Console.WriteLine($"\n--- Frame {frameNumber} ---");
                foreach (Player player in Players)
                {
                    Frame frame = new Frame();
                    Console.WriteLine($"\n{player.Name} ist an der Reihe:");
                    int maxPins = 10; // Start mit 10 Pins pro Frame

                    // Erster Wurf
                    int firstRoll = frame.Roll(maxPins);
                    maxPins -= firstRoll;
                    frame.AddScore(firstRoll);
                    Console.WriteLine($"Wurf 1: Anzahl der umgefallenen Pins: {firstRoll}");

                    // Prüfe auf Strike
                    if (firstRoll == 10)
                    {
                        Console.WriteLine("Strike! Nächster Spieler.");
                        player.AddTotalScore(frame.Score);

                        Console.WriteLine($"{player.Name}'s Punktzahl im Frame: {frame.Score} Punkte");
                        Console.WriteLine($"{player.Name}'s Gesamtpunktzahl: {player.TotalScore} Punkte");
                        Console.ReadKey();
                        continue; // Zum nächsten Spieler
                    }
                    Console.ReadKey();

                    // Zweiter Wurf (wenn kein Strike)
                    int secondRoll = frame.Roll(maxPins);
                    frame.AddScore(secondRoll);
                    Console.WriteLine($"Wurf 2: Anzahl der umgefallenen Pins: {secondRoll}");

                    // Prüfe auf Spare
                    if (firstRoll + secondRoll == 10)
                    {
                        Console.WriteLine("Spare!");
                    }

                    // Punkte zur Gesamtpunktzahl des Spielers hinzufügen
                    player.AddTotalScore(frame.Score);
                    Console.WriteLine($"{player.Name}'s Punktzahl im Frame: {frame.Score} Punkte");
                    Console.WriteLine($"{player.Name}'s Gesamtpunktzahl: {player.TotalScore} Punkte");
                    Console.ReadKey();
                }
            }
        }
    }

    public class Player
    {
        public string Name { get; private set; }
        public int TotalScore { get; private set; }

        public Player(string name)
        {
            Name = name;
            TotalScore = 0;
        }

        public void AddTotalScore(int score)
        {
            TotalScore += score;
        }
    }

    public class Frame
    {
        public int Score { get; private set; }

        public Frame()
        {
            Score = 0;
        }

        public void AddScore(int pins)
        {
            Score += pins;
        }

        public int Roll(int maxPins)
        {
            Random random = new Random();
            return random.Next(0, maxPins + 1); // Zufällige Anzahl Pins von 0 bis maxPins
        }
    }
}
