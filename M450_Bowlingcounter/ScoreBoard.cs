using System;
using System.Collections.Generic;
using System.Text;

namespace M450_Bowlingcounter
{
    public static class ScoreBoard
    {
        public static string GetScoreBoard(List<Player> players)
        {
            var table = new StringBuilder();

            // Build the header lines without adding extra newlines
            table.Append(BuildHeaderLine());
            table.Append(BuildWurfLine());
            table.AppendLine(new string('-', 120));

            // Build each player's line
            foreach (var player in players)
            {
                table.AppendLine(BuildPlayerLine(player));
                table.AppendLine(new string('-', 120));
            }

            return table.ToString();
        }

        private static string BuildHeaderLine()
        {
            var header = new StringBuilder("Frame".PadRight(10));

            for (int frame = 1; frame <= 10; frame++)
            {
                header.Append($"| {frame,-6}");
            }

            header.Append("| Total".PadLeft(10));
            header.AppendLine(); // Add a newline at the end
            return header.ToString();
        }

        private static string BuildWurfLine()
        {
            var wurfLine = new StringBuilder("Wurf".PadRight(10));

            for (int frame = 1; frame <= 10; frame++)
            {
                if (frame == 10)
                {
                    wurfLine.Append("| 1  2  3  ");
                }
                else
                {
                    wurfLine.Append("| 1  2  ");
                }
            }

            wurfLine.Append("|");
            wurfLine.AppendLine(); // Add a newline at the end
            return wurfLine.ToString();
        }

        private static string BuildPlayerLine(Player player)
        {
            var playerLine = new StringBuilder(player.Name.PadRight(10));
            var rolls = player.GetRolls();
            int throwIndex = 0;

            for (int frame = 1; frame <= 10; frame++)
            {
                playerLine.Append("| ");
                int throwsInFrame = frame == 10 ? 3 : 2;

                for (int throwNumber = 0; throwNumber < throwsInFrame; throwNumber++)
                {
                    if (throwIndex < rolls.Count)
                    {
                        playerLine.Append($"{rolls[throwIndex],-3}");
                        throwIndex++;
                    }
                    else
                    {
                        playerLine.Append("   "); // Empty space for missing throws
                    }
                }
            }

            playerLine.Append($"| {player.CalculateTotalScore()}");
            return playerLine.ToString();
        }
    }
}
