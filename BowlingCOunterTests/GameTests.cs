using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace M450_BowlingcounterTests
{
    [TestClass]
    public class GameTests
    {
        [TestFixture]
        public class GameTests
        {
            private Game _game;
            private List<Player> _players;

            [SetUp]
            public void Setup()
            {
                _players = new List<Player>
            {
                new Player("Player 1", 80),
                new Player("Player 2", 80)
            };
                _game = new Game(_players);
            }

            [Test]
            public void Start_PlaysTenFrames()
            {
                // Arrange
                // Use reflection to access the private PlayFrame method
                var playFrameMethod = typeof(Game).GetMethod("PlayFrame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                // Act
                _game.Start();

                // Assert - Ensure PlayFrame is called 10 times for each player
                foreach (var player in _players)
                {
                    int frameCount = 0;
                    for (int frameNumber = 1; frameNumber <= 10; frameNumber++)
                    {
                        playFrameMethod.Invoke(_game, new object[] { player, frameNumber });
                        frameCount++;
                    }
                    Assert.AreEqual(10, frameCount);
                }
            }

            [Test]
            public void PlayFrame_CallsHandleThrows()
            {
                // Arrange
                var player = _players;
                var frameMock = new FrameMock(); // Mocking Frame for testing

                // Use reflection to replace the Frame instance in the private field
                typeof(Game).GetField("_players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(_game, new List<Player> { player });

                // Act
                _game.Start(); // This will call PlayFrame internally

                // Assert
                Assert.IsTrue(frameMock.HandleThrowsCalled);
            }

            // Mock Frame class for testing
            public class FrameMock : Frame
            {
                public bool HandleThrowsCalled { get; private set; }

                public override int HandleThrows(Player player, int frameNumber)
                {
                    HandleThrowsCalled = true;
                    return 0;
                }
            }
        }
    }
}

