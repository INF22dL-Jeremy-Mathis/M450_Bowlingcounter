using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M450_Bowlingcounter
{
    public class Frame
    {
        private readonly RandomChance _randomChance;
        private int _score;
        private double _foulRate = 0.5;

        public Frame()
        {
            _randomChance = new RandomChance();
            _score = 0;
        }


        // handles all the players throws of current frame (atleast 1, maximum 3 throws)
        public int HandleThrows(Player player, int frameNumber)
        {
            int maxPins = 10;
            int firstThrow = 0, secondThrow = 0, thirdThrow = 0;

            do
            {
                Console.Write($"Enter the number of pins thrown (0 to {maxPins}): ");
            } while (!int.TryParse(Console.ReadLine(), out firstThrow) || firstThrow < 0 || firstThrow > maxPins);

            HandleThrow(player, firstThrow, 1, frameNumber);

            if (frameNumber == 10)
            {
                maxPins = firstThrow == 10 ? 10 : maxPins - firstThrow;
                do
                {
                    Console.Write($"Enter the number of pins thrown (0 to {maxPins}): ");
                } while (!int.TryParse(Console.ReadLine(), out secondThrow) || secondThrow < 0 || secondThrow > maxPins);


                HandleThrow(player, secondThrow, 2, frameNumber, firstThrow);

                // Bedingung für extra wurf
                if (firstThrow == 10)
                {
                    maxPins = 10;
                    do
                    {
                        Console.Write($"Enter the number of pins thrown (0 to {maxPins}): ");
                    } while (!int.TryParse(Console.ReadLine(), out thirdThrow) || thirdThrow < 0 || thirdThrow > maxPins);
                    HandleThrow(player, thirdThrow, 3, frameNumber, secondThrow);
                }
            }
            else
            {
                if (firstThrow != 10)
                {
                    maxPins -= firstThrow;
                    do
                    {
                        Console.Write($"Enter the number of pins thrown (0 to {maxPins}): ");
                    } while (!int.TryParse(Console.ReadLine(), out secondThrow) || secondThrow < 0 || secondThrow > maxPins);
                    HandleThrow(player, secondThrow, 2, frameNumber, firstThrow);
                }
                else
                {
                    player.RecordRoll("-");
                }
            }

            WaitForFrameEnd();

            return _score = firstThrow + secondThrow + thirdThrow;
        }


        // Single throw logic
        private void HandleThrow(Player player, int roll, int rollNumber, int frameNumber, int previousRoll = 0)

        {
            string rollResult;

            if (roll == 10) // Strike
            {
                rollResult = "X";
            }
            else if (rollNumber == 2 && previousRoll + roll == 10) // Spare
            {
                rollResult = "/"; // spare
            }

            else // Normal roll
            {
                rollResult = _randomChance.IsFoul() ? "F" : roll == 0 ? "G" : roll.ToString();
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
}
