using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using TacitusLogger.Destinations.File;

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class FileSystemFacadeTests : IntegrationTestBase
    {
        protected string _tempFolderName = "FileSystemFacadeIntTests";

        [OneTimeTearDown]
        public void DeleteTempFolder()
        {
            var folderPath = GetTempFolderPath(_tempFolderName);
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
        }

        #region Tests for AppendToFile method

        [Test]
        public void AppendToFile_WhenCalledGivenThatLogFileNotExsists_CreatesLogFileAndWritesToIt()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            var text = "some text";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            standardFileSystemFacade.AppendToFile(filePath, text);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual(text, lines[0]);
        }

        [Test]
        public void AppendToFile_WhenCalledWithNullLogText_ThrowsArgumentNullException()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                standardFileSystemFacade.AppendToFile(filePath, null);
            });
        }

        [Test]
        public void AppendToFile_WhenCalledWithNullFilePath_ThrowsArgumentNullException()
        {
            // Arrange 
            var text = "some text";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                standardFileSystemFacade.AppendToFile(null, text);
            });
        }

        [Test]
        public void AppendToFile_WhenCalledGivenThatLogFileExsists_AppendsToIt()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            var fs = File.Create(filePath);
            fs.Close();
            var text = "some text";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            standardFileSystemFacade.AppendToFile(filePath, text);

            // Assert 
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual(text, lines[0]);
        }

        [Test]
        public void AppendToFile_WhenCalledTwoTimes_AppendsToTheEndOfTheFile()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            var line1 = "first line";
            var line2 = "second line";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            standardFileSystemFacade.AppendToFile(filePath, line1 + Environment.NewLine);
            standardFileSystemFacade.AppendToFile(filePath, line2 + Environment.NewLine);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual(line1, lines[0]);
            Assert.AreEqual(line2, lines[1]);
        }

        [Test]
        public void AppendToFile_WhenCalledWithPathThatContainsNotExistingFolders_CreatesFoldersToo()
        {
            // Arrange
            var severalIncludedFoldersPath = $"{_tempFolderName}{Path.DirectorySeparatorChar}{GenerateRandomFolderName()}{Path.DirectorySeparatorChar}{GenerateRandomFolderName()}";
            var filePath = GenerateRandomTempFilePath(severalIncludedFoldersPath);
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            standardFileSystemFacade.AppendToFile(filePath, "Text");

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual("Text", lines[0]);
        }

        #endregion


        #region Tests for AppendToFileAsync method

        [Test]
        public async Task AppendToFileAsync_WhenCalledGivenThatLogFileNotExsists_CreatesLogFileAndWritesToIt()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            var text = "some text";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            await standardFileSystemFacade.AppendToFileAsync(filePath, text);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual(text, lines[0]);
        }

        [Test]
        public void AppendToFileAsync_WhenCalledWithNullLogText_ThrowsArgumentNullException()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Assert
            Assert.CatchAsync<ArgumentNullException>(async () =>
           {
               // Act
               await standardFileSystemFacade.AppendToFileAsync(filePath, null);
           });
        }

        [Test]
        public void AppendToFileAsync_WhenCalledWithNullFilePath_ThrowsArgumentNullException()
        {
            // Arrange 
            var text = "some text";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Assert
            Assert.CatchAsync<ArgumentNullException>(async () =>
            {
                // Act
                await standardFileSystemFacade.AppendToFileAsync(null, text);
            });
        }

        [Test]
        public async Task AppendToFileAsync_WhenCalledGivenThatLogFileExsists_AppendsToIt()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            var fs = File.Create(filePath);
            fs.Close();
            var text = "some text";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            await standardFileSystemFacade.AppendToFileAsync(filePath, text);

            // Assert 
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual(text, lines[0]);
        }

        [Test]
        public async Task AppendToFileAsync_WhenCalledTwoTimes_AppendsToTheEndOfTheFile()
        {
            // Arrange
            var filePath = GenerateRandomTempFilePath(_tempFolderName);
            var line1 = "first line";
            var line2 = "second line";
            StandardFileSystemFacade standardFileSystemFacade = new StandardFileSystemFacade();

            // Act
            await standardFileSystemFacade.AppendToFileAsync(filePath, line1 + Environment.NewLine);
            await standardFileSystemFacade.AppendToFileAsync(filePath, line2 + Environment.NewLine);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual(line1, lines[0]);
            Assert.AreEqual(line2, lines[1]);
        }

        #endregion
    }
}
