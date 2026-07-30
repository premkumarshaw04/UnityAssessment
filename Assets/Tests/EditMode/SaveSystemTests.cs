using NUnit.Framework;
using EndlessRunner.Save;

namespace EndlessRunner.Tests.EditMode
{
    public class SaveSystemTests
    {
        [Test]
        public void SaveManager_StoresAndRetrieves_HighScoreCorrectly()
        {
            // Arrange
            SaveManager saveManager = new SaveManager();
            int expectedHighScore = 1250;

            // Act
            saveManager.Save("HighScore", expectedHighScore);
            int retrievedHighScore = saveManager.Load("HighScore", 0);

            // Assert
            Assert.AreEqual(expectedHighScore, retrievedHighScore);
        }
    }
}