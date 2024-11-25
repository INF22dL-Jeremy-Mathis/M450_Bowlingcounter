namespace M450_BowlingcounterTests
{
    [TestClass]
    public class RollerTests
    {
        [TestFixture]
        public class RandomRollerTests
        {
            private RandomRoller _roller;

            [SetUp]
            public void Setup()
            {
                _roller = new RandomRoller();
            }

            [Test]
            public void Roll_ReturnsValueWithinRange()
            {
                // Act
                int roll = _roller.Roll(10);

                // Assert
                Assert.GreaterOrEqual(roll, 0);
                Assert.LessOrEqual(roll, 10);
            }

            [Test]
            public void IsFoul_ReturnsBoolean()
            {
                // Act
                bool isFoul = _roller.IsFoul();

                // Assert - Just verify it returns a boolean value
                Assert.That(isFoul, Is.TypeOf<bool>());
            }
        }
    }
}
