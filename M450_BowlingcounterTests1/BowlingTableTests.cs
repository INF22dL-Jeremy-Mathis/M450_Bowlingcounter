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
            string table = BowlingTable.GetBowlingTable(players);

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
            string table = BowlingTable.GetBowlingTable(players);
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
            string table = BowlingTable.GetBowlingTable(players);
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
            string table = BowlingTable.GetBowlingTable(players);
            Console.WriteLine(table);

            // Assert
            Assert.IsTrue(table.Contains("Perfect   | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  -  | X  X  X  | 300"));
        }
    }
}
