using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Destinations.File;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class FileDestinationBuilderTests
    {
        #region Tests for Ctor

        [Test]
        public void Ctor_WithLogGroupDestinationsBuilder_WhenCalled_InitializesLogGroupDestinationsBuilder()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();

            // Act
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, fileDestinationBuilder.LogGroupDestinationsBuilder);
        }

        #endregion

        #region Tests for WithCustomLogSerializer method

        [Test]
        public void WithCustomLogSerializer_WhenCalled_SetsLogSerializer()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            fileDestinationBuilder.WithCustomLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(logSerializer, fileDestinationBuilder.LogSerializer);
        }

        [Test]
        public void WithCustomLogSerializer_WhenCalled_ReturnsFileDestinationBuilder()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            IFileDestinationBuilder returned = fileDestinationBuilder.WithCustomLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(fileDestinationBuilder, returned);
        }

        [Test]
        public void WithCustomLogSerializer_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);
            // Set first time.
            fileDestinationBuilder.WithCustomLogSerializer(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Tried to set second time.
                fileDestinationBuilder.WithCustomLogSerializer(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithCustomLogSerializer_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                fileDestinationBuilder.WithCustomLogSerializer(null);
            });
        }

        #endregion

        #region Tests for WithPath method

        [Test]
        public void WithPath_WhenCalled_SetsFilePathGenerator()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Act
            fileDestinationBuilder.WithPath(filePathGenerator);

            // Assert
            Assert.AreEqual(filePathGenerator, fileDestinationBuilder.LogFilePathGenerator);
        }

        [Test]
        public void WithPath_WhenCalled_ReturnsFileDestinationBuilder()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Act
            IFileDestinationBuilder returned = fileDestinationBuilder.WithPath(filePathGenerator);

            // Assert
            Assert.AreEqual(fileDestinationBuilder, returned);
        }

        [Test]
        public void WithPath_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange 
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);
            // Was set the first time.
            fileDestinationBuilder.WithPath(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Tried to set second time.
                fileDestinationBuilder.WithPath(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithPath_WhenCalledWithNullFilePathGenerator_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                fileDestinationBuilder.WithPath(null);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_When_Called_Returns_LogGroupDestinationsBuilder()
        {
            // Arrange  
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilder);

            // Act
            ILogGroupDestinationsBuilder logGroupDestinationsBuilderReturned = fileDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, logGroupDestinationsBuilderReturned);
        }

        [Test]
        public void Add_When_Called_Creates_New_FileDestination_And_Passes_It_To_CustomDestination_Method_Of_LogGroupDestinationsBuilder()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>(); 
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            // Act
            fileDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.IsNotNull<FileDestination>()), Times.Once);
        }

        [Test]
        public void Add_When_Called_Given_That_Both_LogSerializer_And_LogFilePathGenerator_Was_Provided_Uses_Them_To_Create_FileDestination()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            fileDestinationBuilder.WithCustomLogSerializer(logSerializer)
                                  .WithPath(filePathGenerator);

            // Act
            fileDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<FileDestination>(d => d.LogSerializer == logSerializer && d.LogFilePathGenerator == filePathGenerator)), Times.Once);
        }

        [Test]
        public void Add_When_Called_Given_That_LogSerializer_Was_Not_Provided_Uses_ExtendedTemplateLogSerializer_As_Default()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object; 
            fileDestinationBuilder.WithPath(filePathGenerator);
             
            // Act
            fileDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<FileDestination>(d => d.LogSerializer is ExtendedTemplateLogSerializer && d.LogFilePathGenerator == filePathGenerator)), Times.Once);
        }

        [Test]
        public void Add_When_Called_Given_That_LogFilePathGenerator_Was_Not_Provided_Uses_FilePathTemplateLogSerializer_As_Default()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;
            fileDestinationBuilder.WithCustomLogSerializer(logSerializer);
             
            // Act
            fileDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<FileDestination>(d => d.LogSerializer == logSerializer && d.LogFilePathGenerator is FilePathTemplateLogSerializer)), Times.Once);
        }

        [Test]
        public void Add_When_Called_Given_That_Both_LogSerializer_And_LogFilePathGenerator_Was_Not_Provided_Uses_ExtendedTemplateLogSerializer_And_FilePathTemplateLogSerializer_As_Defaults()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            FileDestinationBuilder fileDestinationBuilder = new FileDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            // Act
            fileDestinationBuilder.Add();

            // Assert 
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<FileDestination>(d => d.LogSerializer is ExtendedTemplateLogSerializer && d.LogFilePathGenerator is FilePathTemplateLogSerializer)), Times.Once);
        }

        #endregion
    }
}
