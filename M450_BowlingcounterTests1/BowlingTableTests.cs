using Microsoft.VisualStudio.TestTools.UnitTesting;
using M450_Bowlingcounter; // Reference the main project namespace
using System.Collections.Generic;
using System;

namespace M450_Bowlingcounter.Tests
{
    [TestClass]
    public class BowlingTableTests
    {
        [TestMethod]
        public void GetBowlingTable_ShouldReturnCorrectHeader()
        {
            // Arrange
            var players = new List<Player>();

            // Act
            string table = ScoreBoard.GetBowlingTable(players);

            // Assert
            Assert.IsTrue(table.Contains("Frame     | 1     | 2     | 3     | 4     | 5     | 6     | 7     | 8     | 9     | 10       | Total"));
            Assert.IsTrue(table.Contains("Wurf      | 1  2  | 1  2  | 1  2  | 1  2  | 1  2  | 1  2  | 1  2  | 1  2  | 1  2  | 1  2  3  |"));
        }

        
        [TestMethod]
        public void GetBowlingTable_ShouldFormatSinglePlayerCorrectly()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player("Alice", 5) // Name: Alice, Skill: 5
            };

            players[0].RecordRoll("X");
            players[0].RecordRoll("-");
            players[0].RecordRoll("7");
            players[0].RecordRoll("/");
            players[0].RecordRoll("9");
            players[0].RecordRoll("/");
            players[0].RecordRoll("X");
            players[0].RecordRoll("-");
            players[0].RecordRoll("X");
            players[0].RecordRoll("-");
            players[0].RecordRoll("9");
            players[0].RecordRoll("/");
            players[0].RecordRoll("8");
            players[0].RecordRoll("1");
            players[0].RecordRoll("X");
            players[0].RecordRoll("-");
            players[0].RecordRoll("X");
            players[0].RecordRoll("-");
            players[0].RecordRoll("X");
            players[0].RecordRoll("X");
            players[0].RecordRoll("X");

            // Act
            string table = ScoreBoard.GetBowlingTable(players);
            Console.WriteLine(table);
            string awaited = "Alice     | X  -  | 7  /  | 9  /  | X  -  | X  -  | 9  /  | 8  1  | X  -  | X  -  | X  X  X  |";
            Console.WriteLine(awaited);

            // Assert
            Assert.IsTrue(table.Contains(awaited));
        }

        [TestMethod]
        public void GetBowlingTable_ShouldFormatMultiplePlayersCorrectly()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player("Bob", 5),
                new Player("Charlie", 4)
            };

            // Bob's rolls
            players[0].RecordRoll("X");      // Frame 1: Strike (10 pins) -> Score: 10 + Next 2 valid Rolls (7, /) = 20
            players[0].RecordRoll("-");      // Frame 1: Skipped second throw cuz of previous strike
            players[0].RecordRoll("7");      // Frame 2: (7 pins)
            players[0].RecordRoll("/");      // Frame 2: Spare (3 pins) -> Score: 20 + 10(spare) + Next Roll (9) = 39
            players[0].RecordRoll("9");      // Frame 3: (9 pins)
            players[0].RecordRoll("/");      // Frame 3: Spare (1 pin) -> Score: 39 + 10 + Next Roll (10) = 59
            players[0].RecordRoll("X");      // Frame 4: Strike (10 pins) -> Score: 59 + 10 + Next 2 Rolls (10, 9) = 88
            players[0].RecordRoll("-");      // Frame 4: Skipped second throw cuz of previous strike
            players[0].RecordRoll("X");      // Frame 5: Strike (10 pins) -> Score: 88 + 10 + Next 2 Rolls (9, /) = 108
            players[0].RecordRoll("-");      // Frame 5: Skipped second throw cuz of previous strike
            players[0].RecordRoll("9");      // Frame 6: (9 pins)
            players[0].RecordRoll("/");      // Frame 6: Spare (1 pin) -> Score: 118 + Next Roll (8) = 126
            players[0].RecordRoll("8");      // Frame 7: (8 pins) -> Score: 126 + 8 = 134
            players[0].RecordRoll("1");      // Frame 7: (1 pin) -> Score: 134 + 1 = 135
            players[0].RecordRoll("X");      // Frame 8: Strike (10 pins) -> Score: 135 + Next 2 Rolls (9, /) = 144
            players[0].RecordRoll("-");      // Frame 9: Gutter (0 pins) -> Score: 144 + 9 (Frame 8 bonus) = 153
            players[0].RecordRoll("X");      // Frame 9: Strike (10 pins) -> Score: 153 + Next 2 Rolls (10, 10) = 173
            players[0].RecordRoll("-");      // Frame 10: Gutter (0 pins) -> No score added
            players[0].RecordRoll("X");      // Frame 10: Strike (10 pins) -> Score: 173 + 10 = 183
            players[0].RecordRoll("X");      // Frame 10 Bonus: Strike (10 pins) -> Score: 183 + 10 = 193
            players[0].RecordRoll("X");      // Frame 10 Bonus: Strike (10 pins) -> Score: 193 + 10 = 203


            // Charlie's rolls
            players[1].RecordRoll("X");
            players[1].RecordRoll("-");
            players[1].RecordRoll("1");
            players[1].RecordRoll("1");
            players[1].RecordRoll("9");
            players[1].RecordRoll("G");
            players[1].RecordRoll("X");
            players[1].RecordRoll("-");
            players[1].RecordRoll("F");
            players[1].RecordRoll("F");
            players[1].RecordRoll("1");
            players[1].RecordRoll("/");
            players[1].RecordRoll("5");
            players[1].RecordRoll("1");
            players[1].RecordRoll("X");
            players[1].RecordRoll("-");
            players[1].RecordRoll("9");
            players[1].RecordRoll("0");
            players[1].RecordRoll("X");
            players[1].RecordRoll("1");
            players[1].RecordRoll("2");

            string bob = "Bob       | X  -  | 7  /  | 9  /  | X  -  | X  -  | 9  /  | 8  1  | X  -  | X  -  | X  X  X  |";
            string charlie = "Charlie   | X  -  | 1  1  | 9  G  | X  -  | F  F  | 1  /  | 5  1  | X  -  | 9  0  | X  1  2  |";

            // Act
            string table = ScoreBoard.GetBowlingTable(players);
            Console.WriteLine(table); // Add this line to see the generated table.

            // Assert
            Assert.IsTrue(table.Contains(bob));
            Assert.IsTrue(table.Contains(charlie));
        }

        [TestMethod]
        public void GetBowlingTable_ShouldHandlePerfectGame()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player("Perfect", 10)
            };

            for (int i = 0; i < 12; i++)
            {
                players[0].RecordRoll("X");
            }

            // Act
            string table = ScoreBoard.GetBowlingTable(players);
            Console.WriteLine(table);

            // Assert
            Assert.IsTrue(table.Contains("Perfect   | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  X  X  | 300"));
        }
    }
}
