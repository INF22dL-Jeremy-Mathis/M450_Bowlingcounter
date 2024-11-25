namespace M450_BowlingcounterTests
{
    [TestClass]
    public class PlayerFactoryTests
    {
        [TestFixture]
        public class PlayerFactoryTests
        {
            [Test]
            public void CreatePlayers_CreatesCorrectNumberOfPlayers()
            {
                // Redirect Console input
                string input = "2\nPlayer 1\nPlayer 2\n";
                StringReader stringReader = new StringReader(input);
                Console.SetIn(stringReader);

                // Act
                List<Player> players = PlayerFactory.CreatePlayers(80);

                // Assert
                Assert.AreEqual(2, players.Count);
                Assert.AreEqual("Player 1", players.Name);
                Assert.AreEqual("Player 2", players[1].Name);
            }
        }
    }
}
