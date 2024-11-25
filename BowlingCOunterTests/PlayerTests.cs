namespace M450_BowlingcounterTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestFixture]
        public class PlayerTests
        {
            private Player _player;

            [SetUp]
            public void Setup()
            {
                _player = new Player("Test Player", 80);
            }

            // CalculateTotalScore
            [Test]
            public void CalculateTotalScore_OpenFrames_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("4");
                _player.RecordRoll("5");
                // ... weitere Würfe für 10 Frames hinzufügen

                // Act
                int score = _player.CalculateTotalScore();

                // Assert
                Assert.AreEqual(90, score); // Erwarteter Wert basierend auf den Würfen
            }

            [Test]
            public void CalculateTotalScore_WithSpares_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("9");
                _player.RecordRoll("/");
                _player.RecordRoll("5");
                // ... weitere Würfe für 10 Frames hinzufügen

                // Act
                int score = _player.CalculateTotalScore();

                // Assert
                // Erwarteter Wert basierend auf den Würfen
                Assert.AreEqual(150, score);
            }

            [Test]
            public void CalculateTotalScore_WithStrikes_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("X");
                _player.RecordRoll("5");
                _player.RecordRoll("4");
                // ... weitere Würfe für 10 Frames hinzufügen

                // Act
                int score = _player.CalculateTotalScore();

                // Assert
                // Erwarteter Wert basierend auf den Würfen
                Assert.AreEqual(190, score);
            }

            [Test]
            public void CalculateTotalScore_WithStrikesAndSpares_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("X");
                _player.RecordRoll("9");
                _player.RecordRoll("/");
                // ... weitere Würfe für 10 Frames hinzufügen

                // Act
                int score = _player.CalculateTotalScore();

                // Assert
                // Erwarteter Wert basierend auf den Würfen
                Assert.AreEqual(200, score);
            }

            [Test]
            public void CalculateTotalScore_PerfectGame_ReturnsCorrectScore()
            {
                // Arrange
                for (int i = 0; i < 12; i++) // 12 Strikes für ein perfektes Spiel
                {
                    _player.RecordRoll("X");
                }

                // Act
                int score = _player.CalculateTotalScore();

                // Assert
                Assert.AreEqual(300, score);
            }

            // CalculateStrikeScore
            [Test]
            public void CalculateStrikeScore_TwoBonusRolls_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("X");
                _player.RecordRoll("5");
                _player.RecordRoll("4");

                // Act
                int score = _player.CalculateStrikeScore(_player.GetRolls(), 0);

                // Assert
                Assert.AreEqual(19, score);
            }

            [Test]
            public void CalculateStrikeScore_FollowedByStrike_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("X");
                _player.RecordRoll("X");
                _player.RecordRoll("5");

                // Act
                int score = _player.CalculateStrikeScore(_player.GetRolls(), 0);

                // Assert
                Assert.AreEqual(25, score);
            }

            [Test]
            public void CalculateStrikeScore_TenthFrame_ReturnsCorrectScore()
            {
                // Arrange
                // Füge Würfe für 9 Frames hinzu
                for (int i = 0; i < 9 * 2; i++)
                {
                    _player.RecordRoll("0");
                }

                _player.RecordRoll("X");
                _player.RecordRoll("5");
                _player.RecordRoll("4");

                // Act
                int score = _player.CalculateStrikeScore(_player.GetRolls(), 18); // Index für den 10. Frame

                // Assert
                Assert.AreEqual(19, score);
            }

            // CalculateSpareScore
            [Test]
            public void CalculateSpareScore_OneBonusRoll_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("9");
                _player.RecordRoll("/");
                _player.RecordRoll("5");

                // Act
                int score = _player.CalculateSpareScore(_player.GetRolls(), 0);

                // Assert
                Assert.AreEqual(15, score);
            }

            [Test]
            public void CalculateSpareScore_TenthFrame_ReturnsCorrectScore()
            {
                // Arrange
                // Füge Würfe für 9 Frames hinzu
                for (int i = 0; i < 9 * 2; i++)
                {
                    _player.RecordRoll("0");
                }

                _player.RecordRoll("9");
                _player.RecordRoll("/");
                _player.RecordRoll("5");

                // Act
                int score = _player.CalculateSpareScore(_player.GetRolls(), 18); // Index für den 10. Frame

                // Assert
                Assert.AreEqual(15, score);
            }

            // CalculateOpenFrameScore
            [Test]
            public void CalculateOpenFrameScore_ReturnsCorrectScore()
            {
                // Arrange
                _player.RecordRoll("4");
                _player.RecordRoll("5");

                // Act
                int score = _player.CalculateOpenFrameScore(_player.GetRolls(), 0);

                // Assert
                Assert.AreEqual(9, score);
            }

            // CalculateFrameTenScore
            [Test]
            public void CalculateFrameTenScore_ThreeRolls_ReturnsCorrectScore()
            {
                // Arrange
                // Füge Würfe für 9 Frames hinzu
                for (int i = 0; i < 9 * 2; i++)
                {
                    _player.RecordRoll("0");
                }

                _player.RecordRoll("5");
                _player.RecordRoll("4");
                _player.RecordRoll("2");

                // Act
                int score = _player.CalculateFrameTenScore(_player.GetRolls(), 18); // Index für den 10. Frame

                // Assert
                Assert.AreEqual(11, score);
            }

            [Test]
            public void CalculateFrameTenScore_StrikeAndBonusRolls_ReturnsCorrectScore()
            {
                // Arrange
                // Füge Würfe für 9 Frames hinzu
                for (int i = 0; i < 9 * 2; i++)
                {
                    _player.RecordRoll("0");
                }

                _player.RecordRoll("X");
                _player.RecordRoll("5");
                _player.RecordRoll("4");

                // Act
                int score = _player.CalculateFrameTenScore(_player.GetRolls(), 18); // Index für den 10. Frame

                // Assert
                Assert.AreEqual(19, score);
            }

            [Test]
            public void CalculateFrameTenScore_SpareAndBonusRoll_ReturnsCorrectScore()
            {
                // Arrange
                // Füge Würfe für 9 Frames hinzu
                for (int i = 0; i < 9 * 2; i++)
                {
                    _player.RecordRoll("0");
                }

                _player.RecordRoll("9");
                _player.RecordRoll("/");
                _player.RecordRoll("5");

                // Act
                int score = _player.CalculateFrameTenScore(_player.GetRolls(), 18); // Index für den 10. Frame

                // Assert
                Assert.AreEqual(15, score);
            }

            // GetRollValue
            [Test]
            public void GetRollValue_Strike_ReturnsTen()
            {
                // Act
                int value = _player.GetRollValue(new List<string> { "X" }, 0);

                // Assert
                Assert.AreEqual(10, value);
            }

            [Test]
            public void GetRollValue_Spare_ReturnsCorrectValue()
            {
                // Act
                int value = _player.GetRollValue(new List<string> { "5", "/" }, 1);

                // Assert
                Assert.AreEqual(5, value);
            }

            [Test]
            public void GetRollValue_NormalRoll_ReturnsCorrectValue()
            {
                // Act
                int value = _player.GetRollValue(new List<string> { "4" }, 0);

                // Assert
                Assert.AreEqual(4, value);
            }

            [Test]
            public void GetRollValue_Gutterball_ReturnsZero()
            {
                // Act
                int value = _player.GetRollValue(new List<string> { "G" }, 0);

                // Assert
                Assert.AreEqual(0, value);
            }

            [Test]
            public void GetRollValue_Foul_ReturnsZero()
            {
                // Act
                int value = _player.GetRollValue(new List<string> { "F" }, 0);

                // Assert
                Assert.AreEqual(0, value);
            }

            [Test]
            public void GetRollValue_OutOfBoundsIndex_ReturnsZero()
            {
                // Act
                int value = _player.GetRollValue(new List<string> { "4" }, 1);

                // Assert
                Assert.AreEqual(0, value);
            }

            // IsStrike
            [Test]
            public void IsStrike_ValidStrike_ReturnsTrue()
            {
                // Act
                bool isStrike = _player.IsStrike(new List<string> { "X" }, 0);

                // Assert
                Assert.IsTrue(isStrike);
            }

            [Test]
            public void IsStrike_NotAStrike_ReturnsFalse()
            {
                // Act
                bool isStrike = _player.IsStrike(new List<string> { "4" }, 0);

                // Assert
                Assert.IsFalse(isStrike);
            }

            // IsSpare
            [Test]
            public void IsSpare_ValidSpare_ReturnsTrue()
            {
                // Act
                bool isSpare = _player.IsSpare(new List<string> { "5", "/" }, 0);

                // Assert
                Assert.IsTrue(isSpare);
            }

            [Test]
            public void IsSpare_NotASpare_ReturnsFalse()
            {
                // Act
                bool isSpare = _player.IsSpare(new List<string> { "4", "5" }, 0);

                // Assert
                Assert.IsFalse(isSpare);
            }

            [Test]
            public void GetSkill_ReturnsCorrectSkill()
            {
                // Assert
                Assert.AreEqual(80, _player.GetSkill());
            }
        }
    }
}
