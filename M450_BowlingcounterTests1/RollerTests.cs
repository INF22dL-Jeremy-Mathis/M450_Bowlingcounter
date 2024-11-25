using Microsoft.VisualStudio.TestTools.UnitTesting;
using M450_Bowlingcounter;
using System;

namespace M450_Bowlingcounter.Tests
{
    [TestClass]
    public class RandomRollerTests
    {
        private RandomRoller _roller;
        private Random _mockRandom;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize the RandomRoller instance
            _roller = new RandomRoller();
            _mockRandom = new Random(42); // Seeded Random for consistent behavior
        }

        [TestMethod]
        public void Roll_ShouldReturnStrike_WhenStrikeProbabilityIsHigh()
        {
            // Arrange
            int maxPins = 10;
            int strikeProbability = 100; // Guaranteed strike

            // Act
            int result = _roller.Roll(maxPins, strikeProbability);

            // Assert
            Assert.AreEqual(10, result, "Roll should return a strike when strikeProbability is 100%");
        }

        [TestMethod]
        public void Roll_ShouldReturnPinsLessThanOrEqualToMaxPins_WhenStrikeProbabilityIsLow()
        {
            // Arrange
            int maxPins = 10;
            int strikeProbability = 0; // No strikes

            // Act
            int result = _roller.Roll(maxPins, strikeProbability);

            // Assert
            Assert.IsTrue(result >= 0 && result < maxPins, "Roll should return a number between 0 and maxPins");
        }

        [TestMethod]
        public void Roll_ShouldHandleNonStrikeFrames_Correctly()
        {
            // Arrange
            int maxPins = 7;
            int strikeProbability = 0; // No strikes

            // Act
            int result = _roller.Roll(maxPins, strikeProbability);

            // Assert
            Assert.IsTrue(result >= 0 && result <= maxPins, "Roll should respect the maxPins boundary for non-strike frames");
        }

        [TestMethod]
        public void IsFoul_ShouldReturnTrue_WhenFoulRateIsHigh()
        {
            // Arrange
            int foulRate = 100; // Guaranteed foul

            // Act
            bool isFoul = _roller.IsFoul(foulRate);

            // Assert
            Assert.IsTrue(isFoul, "IsFoul should return true when foulRate is 100%");
        }

        [TestMethod]
        public void IsFoul_ShouldReturnFalse_WhenFoulRateIsZero()
        {
            // Arrange
            int foulRate = 0; // No fouls

            // Act
            bool isFoul = _roller.IsFoul(foulRate);

            // Assert
            Assert.IsFalse(isFoul, "IsFoul should return false when foulRate is 0%");
        }

        [TestMethod]
        public void IsFoul_ShouldReturnTrueOccasionally_WithDefaultFoulRate()
        {
            // Arrange
            int foulRate = 5; // Default foul rate

            // Act
            bool isFoul = false;
            for (int i = 0; i < 1000; i++) // Test a large number of iterations
            {
                if (_roller.IsFoul(foulRate))
                {
                    isFoul = true;
                    break;
                }
            }

            // Assert
            Assert.IsTrue(isFoul, "IsFoul should occasionally return true with a default foul rate");
        }

        [TestMethod]
        public void Roll_ShouldReturnStrikeProbabilityCorrectly()
        {
            // Arrange
            int maxPins = 10;
            int strikeProbability = 50; // 50% chance for a strike
            int totalRolls = 10000;
            int strikes = 0;

            // Act
            for (int i = 0; i < totalRolls; i++)
            {
                if (_roller.Roll(maxPins, strikeProbability) == 10)
                {
                    strikes++;
                }
            }

            double strikePercentage = (double)strikes / totalRolls * 100;

            // Assert
            Assert.IsTrue(
                strikePercentage >= 45 && strikePercentage <= 55,
                $"Strike percentage {strikePercentage}% is outside expected range"
            );
        }
    }
}
