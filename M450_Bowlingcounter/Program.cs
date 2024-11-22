using System;
using System.Collections.Generic;
using System.Text;

namespace M450_Bowlingcounter
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Bowling-Zähler gestartet!");

            List<Player> players = PlayerFactory.CreatePlayers(GetPlayerCount(), PlayerFactory.GetPlayerNameFromConsole);

            Game game = new Game(players, new FrameFactory());
            game.Start();

            Console.WriteLine(BowlingTable.GetBowlingTable(players));

            Console.WriteLine("\nDrücken Sie eine beliebige Taste, um das Programm zu beenden...");
            Console.ReadKey();
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
    }

    public static class PlayerFactory
    {
        public static List<Player> CreatePlayers(int playerCount, Func<int, string> getPlayerName)
        {
            List<Player> players = new List<Player>();

            for (int i = 1; i <= playerCount; i++)
            {
                string playerName = getPlayerName(i);
                players.Add(new Player(playerName));
            }

            return players;
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

    public static class BowlingTable
    {
        public static string GetBowlingTable(List<Player> players)
        {
            StringBuilder table = new StringBuilder();

            table.Append("Frame".PadRight(10));
            for (int frame = 1; frame <= 10; frame++)
            {
                table.Append("| " + $"{frame}".PadRight(6));
            }
            table.AppendLine("| Total".PadLeft(10));

            table.Append("Wurf".PadRight(10));
            for (int frame = 1; frame <= 10; frame++)
            {
                table.Append("| " + "1".PadRight(3) + "2".PadRight(3));
                if (frame == 10)
                {
                    table.Append("3".PadRight(3));
                }
            }
            table.AppendLine("|");

            table.AppendLine(new string('-', 120));

            foreach (var player in players)
            {
                table.Append(player.Name.PadRight(10));
                int throwIndex = 0;
                List<string> rolls = player.GetRolls();
                for (int frame = 1; frame <= 10; frame++)
                {
                    table.Append("| ");
                    int throwsInFrame = (frame == 10) ? 3 : 2;
                    for (int throwNumber = 0; throwNumber < throwsInFrame; throwNumber++)
                    {
                        if (throwIndex < rolls.Count)
                        {
                            string throwResult = rolls[throwIndex];
                            table.Append(throwResult.PadRight(3));
                            throwIndex++;
                        }
                        else
                        {
                            table.Append("".PadRight(3));
                        }
                    }
                }
                table.AppendLine("| " + player.CalculateTotalScore().ToString());

                table.AppendLine(new string('-', 120));
            }

            return table.ToString();
        }
    }

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
                    PlayFrame(player, frameNumber);
                }
            }
        }

        private void PlayFrame(Player player, int frameNumber)
        {
            Frame frame = _frameFactory.CreateFrame();
            Console.WriteLine($"\n{player.Name} ist an der Reihe:");

            int frameScore = frame.HandleRolls(player, frameNumber);
            player.AddTotalScore(frameScore);
        }
    }

    public class FrameFactory
    {
        public Frame CreateFrame()
        {
            return new Frame(new RandomRoller());
        }
    }

    public class Player
    {
        public string Name { get; }
        public int TotalScore { get; private set; }
        private List<string> Rolls { get; }

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
            List<string> rolls = this.Rolls;
            int score = 0;
            int rollIndex = 0;

            for (int frame = 1; frame <= 10; frame++)
            {
                if (frame == 10)
                {
                    score += CalculateFrameTenScore(rolls, rollIndex);
                    break;
                }
                else if (IsStrike(rolls, rollIndex))
                {
                    score += CalculateStrikeScore(rolls, rollIndex);
                    rollIndex += 2;
                }
                else if (IsSpare(rolls, rollIndex))
                {
                    score += CalculateSpareScore(rolls, rollIndex);
                    rollIndex += 2;
                }
                else
                {
                    score += CalculateOpenFrameScore(rolls, rollIndex);
                    rollIndex += 2;
                }

            }

            return score;
        }

        private int CalculateStrikeScore(List<string> rolls, int rollIndex)
        {
            int bonus1 = GetNextRollValue(rolls, rollIndex + 1, skipSkips: true);
            int bonus2 = GetNextRollValue(rolls, rollIndex + 2, skipSkips: true);
            return 10 + bonus1 + bonus2;
        }

        private int CalculateSpareScore(List<string> rolls, int rollIndex)
        {
            int bonus = GetNextRollValue(rolls, rollIndex + 2, skipSkips: true);
            return 10 + bonus;
        }

        private int CalculateOpenFrameScore(List<string> rolls, int rollIndex)
        {
            int roll1 = GetRollValue(rolls, rollIndex);
            int roll2 = GetRollValue(rolls, rollIndex + 1);
            return roll1 + roll2;
        }

        private int CalculateFrameTenScore(List<string> rolls, int rollIndex)
        {
            int score = 0;
            for (int i = rollIndex; i < rolls.Count; i++)
            {
                score += GetRollValue(rolls, i);
            }
            return score;
        }

        private int GetNextRollValue(List<string> rolls, int startIndex, bool skipSkips = false)
        {
            int index = startIndex;
            if (skipSkips)
            {
                while (index < rolls.Count && (rolls[index] == "-" || rolls[index] == "G" || rolls[index] == "F"))
                {
                    index++;
                }
            }
            return index < rolls.Count ? GetRollValue(rolls, index) : 0;
        }

        private int GetRollValue(List<string> rolls, int index)
        {
            if (index >= rolls.Count) return 0;
            if (rolls[index] == "X") return 10;
            if (rolls[index] == "/") return 10 - (index > 0 ? GetRollValue(rolls, index - 1) : 0);
            if (rolls[index] == "-") return 0;
            if (rolls[index] == "G") return 0;
            if (rolls[index] == "F") return 0;
            return int.TryParse(rolls[index], out int value) ? value : 0;
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

            firstRoll = _roller.Roll(maxPins);
            HandleRoll(player, firstRoll, 1, frameNumber);

            if (frameNumber == 10)
            {
                maxPins = firstRoll == 10 ? 10 : maxPins - firstRoll;
                secondRoll = _roller.Roll(maxPins);
                HandleRoll(player, secondRoll, 2, frameNumber, firstRoll);

                if (firstRoll + secondRoll >= 10)
                {
                    maxPins = 10;
                    thirdRoll = _roller.Roll(maxPins);
                    HandleRoll(player, thirdRoll, 3, frameNumber);
                }
            }
            else
            {
                if (firstRoll != 10)
                {
                    maxPins -= firstRoll;
                    secondRoll = _roller.Roll(maxPins);
                    HandleRoll(player, secondRoll, 2, frameNumber, firstRoll);
                }
                else
                {
                    player.RecordRoll("-");
                }
            }

            WaitForFrameEnd();

            return _score = firstRoll + secondRoll + thirdRoll;
        }

        private void HandleRoll(Player player, int roll, int rollNumber, int frameNumber, int previousRoll = 0)
        {
            string rollResult;

            if (frameNumber == 10 && roll == 10) // Frame 10, Strike
            {
                rollResult = "X";
            }
            else if (rollNumber == 1 && roll == 10) // Strike in regular frames
            {
                rollResult = "X";
            }
            else if (rollNumber == 2 && previousRoll + roll == 10) // Spare
            {
                rollResult = "/";
            }
            else // Normal roll
            {
                rollResult = _roller.IsFoul() ? "F" : roll == 0 ? "G" : roll.ToString();
            }

            player.RecordRoll(rollResult);
            PrintRollResult(rollResult, rollNumber);
        }
        private void PrintRollResult(string rollResult, int rollNumber)
        {
            switch (rollResult)
            {
                case "X":
                    Console.WriteLine("Strike!");
                    break;
                case "/":
                    Console.WriteLine("Spare!");
                    break;
                case "F":
                    Console.WriteLine("Foul!");
                    break;
                default:
                    Console.WriteLine($"Wurf {rollNumber}: {rollResult}");
                    break;
            }
        }

        private void WaitForFrameEnd()
        {
            Console.WriteLine("\nDrücken Sie eine beliebige Taste, um den Frame zu beenden...");
            Console.ReadKey();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }

    public interface IRoller
    {
        int Roll(int maxPins);
        bool IsFoul();
    }

    public class RandomRoller : IRoller
    {
        private readonly Random _random;
        private readonly double _strikeProbability;

        public RandomRoller(double strikeProbability = 1)
        {
            _random = new Random();
            _strikeProbability = strikeProbability;
        }

        public int Roll(int maxPins)
        {
            if (maxPins == 10 && _random.NextDouble() < _strikeProbability)
            {
                return 10;
            }
            return _random.Next(0, maxPins + 1);
        }

        public bool IsFoul()
        {
            return _random.Next(0, 100) < 5;
        }
    }
}