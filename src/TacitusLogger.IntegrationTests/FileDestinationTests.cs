using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using TacitusLogger.Builders;
using TacitusLogger.Destinations.File;

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class FileDestinationTests : IntegrationTestBase
    {
        protected string _tempFolderName = "FileDestinationIntTests";

        [OneTimeTearDown]
        public void DeleteTempFolder()
        {
            var folderPath = GetTempFolderPath(_tempFolderName);
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
        }

        #region Tests for Log method

        //Issue #4 - Null reference exception in FileDestination
        [Test]
        public void Log_When_Called_Sends_Log_Text_To_The_File()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);

            ILogger logger = LoggerBuilder.Logger()
                                          .WithExceptionHandling(ExceptionHandling.Rethrow)
                                          .ForAllLogs()
                                          .File().WithPath(filePath).Add()
                                          .BuildLogger();

            logger.Log(LogType.Info, "Some info");

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreNotEqual(0, lines.Length);
        }

        #endregion

        #region Tests for LogAsync method

        //Issue #4 - Null reference exception in FileDestination
        [Test]
        public async Task LogAsync_When_Called_Sends_Log_Text_To_The_File()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);

            ILogger logger = LoggerBuilder.Logger()
                                          .WithExceptionHandling(ExceptionHandling.Rethrow)
                                          .ForAllLogs()
                                          .File().WithPath(filePath).Add()
                                          .BuildLogger();

            await logger.LogAsync(LogType.Info, "Some info");

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreNotEqual(0, lines.Length);
        }

        #endregion


    }
}
