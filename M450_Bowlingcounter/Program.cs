using System;
using System.Collections.Generic;

namespace M450_Bowlingcounter
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Bowling-Zähler gestartet!");

            // Create players
            List<Player> players = PlayerFactory.CreatePlayers();

            // Start the game
            Game game = new Game(players, new FrameFactory());
            game.Start();

            // Display final scores using the new scoreboard
            BowlingTable.PrintBowlingTable(players);

            // Wait for user before closing
            Console.WriteLine("\nDrücken Sie eine beliebige Taste, um den Frame zu beenden...");
            Console.ReadKey();
        }
    }

    // Factory for creating players
    public static class PlayerFactory
    {
        public static List<Player> CreatePlayers()
        {
            int playerCount;
            do
            {
                Console.Write("Geben Sie die Anzahl der Spieler ein: ");
            } while (!int.TryParse(Console.ReadLine(), out playerCount) || playerCount <= 1);

            Console.WriteLine($"Anzahl der Spieler: {playerCount}");
            List<Player> players = new List<Player>();

            for (int i = 1; i <= playerCount; i++)
            {
                string playerName;
                do
                {
                    Console.Write($"Name des Spielers {i}: ");
                    playerName = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(playerName)); // Prüft auf leere oder nur Leerzeichen bestehende Eingabe

                players.Add(new Player(playerName));
            }


            return players;
        }
    }

    // Class responsible for displaying the scoreboard
    public static class BowlingTable
    {
        public static void PrintBowlingTable(List<Player> players)
        {
            // Print the frame numbers header
            Console.Write("Frame".PadRight(10));
            for (int frame = 1; frame <= 10; frame++)
            {
                Console.Write("| " + $"{frame}".PadRight(6));
            }
            Console.WriteLine("| Total".PadLeft(10));

            // Print the subheader for throws
            Console.Write("Wurf".PadRight(10));
            for (int frame = 1; frame <= 10; frame++)
            {
                Console.Write("| " + "1".PadRight(3) + "2".PadRight(3));
                if (frame == 10)
                {
                    Console.Write("3".PadRight(3));
                }
            }
            Console.WriteLine("|");

            // Print a horizontal line as a separator
            Console.WriteLine(new string('-', 120));

            // Print the player data
            foreach (var player in players)
            {
                Console.Write(player.Name.PadRight(10));
                int throwIndex = 0;
                List<string> rolls = player.GetRolls();

                for (int frame = 1; frame <= 10; frame++)
                {
                    Console.Write("| ");
                    int throwsInFrame = (frame == 10) ? 3 : 2;

                    for (int throwNumber = 0; throwNumber < throwsInFrame; throwNumber++)
                    {
                        if (throwIndex < rolls.Count)
                        {
                            string throwResult = rolls[throwIndex];
                            Console.Write(throwResult.PadRight(3));
                            throwIndex++;
                        }
                        else
                        {
                            Console.Write("".PadRight(3));
                        }
                    }
                }
                Console.WriteLine("| " + player.CalculateTotalScore().ToString());

                // Print a horizontal line after each player's row
                Console.WriteLine(new string('-', 120));
            }
        }
    }

    // Main Game class
    public class Game
    {
        private readonly List<Player> _players;
        private readonly FrameFactory _frameFactory;
        private const int TotalFrames = 10;

        public Game(List<Player> players, FrameFactory frameFactory)
        {
            _players = players;
            _frameFactory = frameFactory;
        }

        public void Start()
        {
            for (int frameNumber = 1; frameNumber <= TotalFrames; frameNumber++)
            {
                Console.WriteLine($"\n--- Frame {frameNumber} ---");
                foreach (Player player in _players)
                {
                    Frame frame = _frameFactory.CreateFrame();
                    Console.WriteLine($"\n{player.Name} ist an der Reihe:");

                    // Handle the rolls for the frame
                    int frameScore = frame.HandleRolls(player, frameNumber);
                    player.AddTotalScore(frameScore);
                }
            }
        }
    }

    // Factory for creating frames
    public class FrameFactory
    {
        public Frame CreateFrame()
        {
            return new Frame(new RandomRoller());
        }
    }

    // Player class
    public class Player
    {
        public string Name { get; }
        public int TotalScore { get; private set; }
        private List<string> Rolls { get; } // Stores all rolls as strings

        public Player(string name)
        {
            Name = name;
            TotalScore = 0;
            Rolls = new List<string>();
        }

        public void AddTotalScore(int score)
        {
            TotalScore += score;
        }

        public void RecordRoll(string roll)
        {
            Rolls.Add(roll);
        }

        public List<string> GetRolls()
        {
            return Rolls;
        }

        public int CalculateTotalScore()
        {
            List<string> rolls = this.Rolls; // Beispiel: ["X", "-", "7", "/", "9", "-", ...]
            int score = 0;
            int rollIndex = 0;

            for (int frame = 1; frame <= 10; frame++)
            {
                if (IsStrike(rolls, rollIndex))
                {
                    // Strike: 10 Punkte + nächste zwei tatsächliche Würfe als Bonus
                    int bonus1 = GetNextRollValue(rolls, rollIndex + 1, skipSkips: true);
                    int bonus2 = GetNextRollValue(rolls, rollIndex + 2, skipSkips: true);
                    score += 10 + bonus1 + bonus2;
                    rollIndex += 2; // Überspringe Strike und Platzhalter
                }
                else if (IsSpare(rolls, rollIndex))
                {
                    // Spare: 10 Punkte + nächster tatsächlicher Wurf als Bonus
                    int bonus = GetNextRollValue(rolls, rollIndex + 2, skipSkips: true);
                    score += 10 + bonus;
                    rollIndex += 2; // Überspringe beide Würfe des Spares
                }
                else
                {
                    // Offenes Frame: Summe der beiden Würfe
                    int roll1 = GetRollValue(rolls, rollIndex);
                    int roll2 = GetRollValue(rolls, rollIndex + 1);
                    score += roll1 + roll2;
                    rollIndex += 2; // Überspringe beide Würfe des offenen Frames
                }

                // Spezielle Handhabung für den 10. Frame
                if (frame == 10)
                {
                    // Addiere alle verbleibenden Würfe im 10. Frame
                    for (int i = rollIndex; i < rolls.Count; i++)
                    {
                        score += GetRollValue(rolls, i);
                    }
                    break; // Beende die Schleife nach dem 10. Frame
                }
            }

            return score;
        }


        private int GetNextRollValue(List<string> rolls, int startIndex, bool skipSkips = false)
        {
            int index = startIndex;

            if (skipSkips)
            {
                // Überspringe Platzhalter wie "-", "G", "F"
                while (index < rolls.Count && (rolls[index] == "-" || rolls[index] == "G" || rolls[index] == "F"))
                {
                    index++;
                }
            }

            return index < rolls.Count ? GetRollValue(rolls, index) : 0; // Sicherung
        }

        private int GetRollValue(List<string> rolls, int index)
        {
            if (index >= rolls.Count) return 0; // Kein Wurf verfügbar
            if (rolls[index] == "X") return 10; // Strike
            if (rolls[index] == "/") return 10 - (index > 0 ? GetRollValue(rolls, index - 1) : 0); // Spare
            if (rolls[index] == "-") return 0; // Miss
            if (rolls[index] == "G") return 0; // Gutter ball
            if (rolls[index] == "F") return 0; // Foul
            return int.TryParse(rolls[index], out int value) ? value : 0; // Umwandlung in Punkte
        }

        private bool IsStrike(List<string> rolls, int index)
        {
            return index < rolls.Count && rolls[index] == "X";
        }

        private bool IsSpare(List<string> rolls, int index)
        {
            return index + 1 < rolls.Count && rolls[index + 1] == "/";
        }
    }

    // Frame class handles a single frame's logic
    public class Frame
    {
        private readonly IRoller _roller;
        private int _score;

        public Frame(IRoller roller)
        {
            _roller = roller;
            _score = 0;
        }

        public int HandleRolls(Player player, int frameNumber)
        {
            int maxPins = 10;
            int firstRoll = 0, secondRoll = 0, thirdRoll = 0;

            // First Roll
            firstRoll = _roller.Roll(maxPins);
            string firstRollResult = firstRoll == 10 ? "X" : (_roller.IsFoul() ? "F" : firstRoll == 0 ? "G" : firstRoll.ToString());
            player.RecordRoll(firstRollResult);
            Console.WriteLine(firstRollResult == "X" ? "Strike!" : firstRollResult == "F" ? "Foul!" : $"Wurf 1: {firstRollResult}");

            if (frameNumber == 10)
            {
                maxPins = firstRoll == 10 ? 10 : maxPins - firstRoll;
                secondRoll = _roller.Roll(maxPins);
                string secondRollResult = firstRoll == 10
                    ? (secondRoll == 10 ? "X" : (_roller.IsFoul() ? "F" : secondRoll == 0 ? "G" : secondRoll.ToString()))
                    : (firstRoll + secondRoll == 10 ? "/" : (_roller.IsFoul() ? "F" : secondRoll == 0 ? "G" : secondRoll.ToString()));
                player.RecordRoll(secondRollResult);
                Console.WriteLine(secondRollResult == "X" ? "Strike!" : secondRollResult == "/" ? "Spare!" : $"Wurf 2: {secondRollResult}");

                if (firstRoll + secondRoll >= 10)
                {
                    maxPins = 10;
                    thirdRoll = _roller.Roll(maxPins);
                    string thirdRollResult = thirdRoll == 10 ? "X" : (_roller.IsFoul() ? "F" : thirdRoll == 0 ? "G" : thirdRoll.ToString());
                    player.RecordRoll(thirdRollResult);
                    Console.WriteLine(thirdRollResult == "X" ? "Strike!" : thirdRollResult == "F" ? "Foul!" : $"Wurf 3: {thirdRollResult}");
                }
            }
            else
            {
                if (firstRoll != 10)
                {
                    maxPins -= firstRoll;
                    secondRoll = _roller.Roll(maxPins);
                    string secondRollResult = firstRoll + secondRoll == 10 ? "/" : (_roller.IsFoul() ? "F" : secondRoll == 0 ? "G" : secondRoll.ToString());
                    player.RecordRoll(secondRollResult);
                    Console.WriteLine(secondRollResult == "/" ? "Spare!" : secondRollResult == "F" ? "Foul!" : $"Wurf 2: {secondRollResult}");
                }
                else
                {
                    player.RecordRoll("-");
                }
            }

            // Prompt and wait to finish the frame
            Console.WriteLine("\nDrücken Sie eine beliebige Taste, um den Frame zu beenden...");
            Console.ReadKey();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));



            return _score = firstRoll + secondRoll + thirdRoll;
        }
    }

    // Interface for rolling mechanics
    public interface IRoller
    {
        int Roll(int maxPins);
        bool IsFoul();
    }

    // Random roller implementation
    public class RandomRoller : IRoller
    {
        private readonly Random _random;
        private readonly double _strikeProbability; // Probability of a strike (0.0 to 1.0)

        public RandomRoller(double strikeProbability = 0.1) // Default 20% chance of a strike
        {
            _random = new Random();
            _strikeProbability = strikeProbability;
        }

        public int Roll(int maxPins)
        {
            // Determine if a strike occurs based on the probability
            if (maxPins == 10 && _random.NextDouble() < _strikeProbability)
            {
                return 10; // Strike
            }

            // Otherwise, return a random roll between 0 and maxPins
            return _random.Next(0, maxPins + 1);
        }

        public bool IsFoul()
        {
            return _random.Next(0, 100) < 5; // 5% chance of a foul
        }
    }
}
