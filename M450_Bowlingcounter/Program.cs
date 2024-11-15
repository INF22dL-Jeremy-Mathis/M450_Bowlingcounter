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
                    if (frame.IsFoul())
                    {
                        firstRoll = 0;
                        Console.WriteLine("Foul! Wurf mit 0 Punkten bewertet.");
                    }
                    else if (firstRoll == 0)
                        Console.WriteLine("Miss! Kein Pin getroffen.");
                    else
                        Console.WriteLine($"Wurf 1: Anzahl der umgefallenen Pins: {firstRoll}");
                    maxPins -= firstRoll;
                    frame.AddScore(firstRoll);

                    

                    // Prüfe auf Strike
                    if (firstRoll == 10)
                    {
                        Console.WriteLine("Strike! X");
                        player.AddTotalScore(frame.Score);
                        player.AddBonusPoints(StrikeBonus(player, frameNumber));
                        PrintScores(player, frame);
                        continue; // Zum nächsten Spieler
                    }

                    // Zweiter Wurf
                    int secondRoll = frame.Roll(maxPins);
                    if (frame.IsFoul())
                    {
                        secondRoll = 0;
                        Console.WriteLine("Foul! Wurf mit 0 Punkten bewertet.");
                    }
                    else if (secondRoll == 0)
                        Console.WriteLine("Miss! Kein Pin getroffen.");
                    else
                        Console.WriteLine($"Wurf 2: Anzahl der umgefallenen Pins: {secondRoll}");
                    frame.AddScore(secondRoll);

                    

                    // Prüfe auf Spare
                    if (firstRoll + secondRoll == 10)
                    {
                        Console.WriteLine("Spare! /");
                        player.AddBonusPoints(SpareBonus(player, frameNumber));
                    }

                    player.AddTotalScore(frame.Score);
                    PrintScores(player, frame);
                }
            }
        }

        // Berechnet Bonuspunkte für einen Strike
        private int StrikeBonus(Player player, int currentFrame)
        {
            if (currentFrame < 10)
                return player.GetNextTwoRolls();
            return 0;
        }

        // Berechnet Bonuspunkte für einen Spare
        private int SpareBonus(Player player, int currentFrame)
        {
            if (currentFrame < 10)
                return player.GetNextRoll();
            return 0;
        }

        // Hilfsfunktion, um die Punkte auszugeben
        private void PrintScores(Player player, Frame frame)
        {
            Console.WriteLine($"{player.Name}'s Punktzahl im Frame: {frame.Score} Punkte");
            Console.WriteLine($"{player.Name}'s Gesamtpunktzahl: {player.TotalScore} Punkte");
            Console.ReadKey();
        }
    }

    public class Player
    {
        public string Name { get; private set; }
        public int TotalScore { get; private set; }
        private Queue<int> Rolls; // Speichert alle Würfe des Spielers

        public Player(string name)
        {
            Name = name;
            TotalScore = 0;
            Rolls = new Queue<int>();
        }

        public void AddTotalScore(int score)
        {
            TotalScore += score;
        }

        public void AddBonusPoints(int points)
        {
            TotalScore += points;
        }

        public void RecordRoll(int pins)
        {
            Rolls.Enqueue(pins);
        }

        // Gibt die Punkte der nächsten zwei Würfe zurück (für Strike)
        public int GetNextTwoRolls()
        {
            int bonus = 0;
            if (Rolls.Count > 0) bonus += Rolls.Dequeue();
            if (Rolls.Count > 0) bonus += Rolls.Dequeue();
            return bonus;
        }

        // Gibt die Punkte des nächsten Wurfs zurück (für Spare)
        public int GetNextRoll()
        {
            if (Rolls.Count > 0) return Rolls.Dequeue();
            return 0;
        }
    }

    public class Frame
    {
        public int Score { get; private set; }
        private Random random;

        public Frame()
        {
            Score = 0;
            random = new Random();
        }

        public void AddScore(int pins)
        {
            Score += pins;
        }

        // Funktion, um einen Wurf auszuführen und eine 5% Chance auf ein Foul zu simulieren
        public int Roll(int maxPins)
        {
            if (IsFoul()) return 0; // Foul: Wurf zählt nicht
            return random.Next(0, maxPins + 1);
        }

        // Prüft, ob ein Foul vorliegt (5% Chance)
        public bool IsFoul()
        {
            return random.Next(0, 100) < 5; // 5% Wahrscheinlichkeit
        }
    }
}
