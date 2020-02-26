using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic; 
using TacitusLogger.Builders;
using TacitusLogger.Destinations;
using TacitusLogger.Destinations.Console;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class ConsoleDestinationBuilderTests
    {
        #region Tests for Ctor

        [Test]
        public void Ctor_WithLogGroupDestinationsBuilder_WhenCalled_InitializesLogGroupDestinationsBuilder()
        {
            // Arrange 
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, consoleDestinationBuilder.LogGroupDestinationsBuilder);
        }

        #endregion

        #region Tests for WithCustomLogSerializer method
         
        [Test]
        public void WithCustomLogSerializer_WhenCalled_SetsLogSerializer()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            consoleDestinationBuilder.WithCustomLogSerializer(logSerializer);
             
            // Assert
            Assert.AreEqual(logSerializer, consoleDestinationBuilder.LogSerializer);
        }

        [Test]
        public void WithCustomLogSerializer_WhenCalled_ReturnsConsoleDestinationBuilder()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            IConsoleDestinationBuilder returned = consoleDestinationBuilder.WithCustomLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(consoleDestinationBuilder, returned);
        }

        [Test]
        public void WithCustomLogSerializer_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            // Set first time.
            consoleDestinationBuilder.WithCustomLogSerializer(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Tried to set second time.
                consoleDestinationBuilder.WithCustomLogSerializer(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithCustomLogSerializer_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            { 
                // Act
                consoleDestinationBuilder.WithCustomLogSerializer(null);
            }); 
        }

        #endregion

        #region Tests for WithCustomColors method

        [Test]
        public void WithCustomColors_WhenCalled_SetsColorScheme()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            Dictionary<LogType, ConsoleColor> colorScheme = new Mock<Dictionary<LogType, ConsoleColor>>().Object;

            // Act
            consoleDestinationBuilder.WithCustomColors(colorScheme);

            // Assert
            Assert.AreEqual(colorScheme, consoleDestinationBuilder.ColorScheme);
        }

        [Test]
        public void WithCustomColors_WhenCalled_ReturnsConsoleDestinationBuilder()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            Dictionary<LogType, ConsoleColor> colorScheme = new Mock<Dictionary<LogType, ConsoleColor>>().Object;

            // Act
            IConsoleDestinationBuilder returned = consoleDestinationBuilder.WithCustomColors(colorScheme);

            // Assert
            Assert.AreEqual(consoleDestinationBuilder, returned);
        }

        [Test]
        public void WithCustomColors_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder); 
            // Added first time
            consoleDestinationBuilder.WithCustomColors(new Mock<Dictionary<LogType, ConsoleColor>>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                consoleDestinationBuilder.WithCustomColors(new Mock<Dictionary<LogType, ConsoleColor>>().Object);
            });
        }

        [Test]
        public void WithCustomColors_WhenCalledWithNullColorScheme_ThrowsArgumentNullException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder); 

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                consoleDestinationBuilder.WithCustomColors(null);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_WhenCalled_ReturnsLogGroupDestinationsBuilder()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);

            // Act
            ILogGroupDestinationsBuilder logGroupDestinationsBuilderReturned = consoleDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, logGroupDestinationsBuilderReturned);
        }


        [Test]
        public void Add_When_Called_AddsANewConsoleDestinationToLogGroupDestinationsBuilder()
        {
            // Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            logGroupDestinationsBuilderMock.Setup(x => x.CustomDestination(It.IsAny<ILogDestination>())).Returns(logGroupDestinationsBuilderMock.Object);
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            // Act
            consoleDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.IsNotNull<ConsoleDestination>()), Times.Once);
        }

        [Test]
        public void Add_WhenCalledGivenThatBothLogSerializerAndColorSchemeWasProvided_ReturnsLogGroupDestinationsBuilderWithSetLogSerializerAndColorScheme()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;
            Dictionary<LogType, ConsoleColor> colorScheme = new Mock<Dictionary<LogType, ConsoleColor>>().Object;
            consoleDestinationBuilder.WithCustomLogSerializer(logSerializer)
                                        .WithCustomColors(colorScheme);

            // Act
            consoleDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logSerializer, consoleDestinationBuilder.LogSerializer);
            Assert.AreEqual(colorScheme, consoleDestinationBuilder.ColorScheme);
        }

        [Test]
        public void Add_WhenCalledGivenThatLogSerializerWasNotProvided_ReturnsLogGroupDestinationsBuilderWithDefaultLogSerializer()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            Dictionary<LogType, ConsoleColor> colorScheme = new Mock<Dictionary<LogType, ConsoleColor>>().Object;
            consoleDestinationBuilder.WithCustomColors(colorScheme);


            // Act
            consoleDestinationBuilder.Add();

            // Assert
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(consoleDestinationBuilder.LogSerializer);
            Assert.AreEqual(colorScheme, consoleDestinationBuilder.ColorScheme);
        }

        [Test]
        public void Add_WhenCalledGivenThatColorSchemeWasNotProvided_ReturnsLogGroupDestinationsBuilderWithDefaultColorScheme()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;
            consoleDestinationBuilder.WithCustomLogSerializer(logSerializer);


            // Act
            consoleDestinationBuilder.Add();

            // Assert
            Assert.NotNull(consoleDestinationBuilder.ColorScheme);
            Assert.AreEqual(logSerializer, consoleDestinationBuilder.LogSerializer); 
        }


        [Test]
        public void Add_WhenCalledGivenThatBothLogSerializerAndColorSchemeWasNotProvided_ReturnsLogGroupDestinationsBuilderWithDefaultLogSerializerAndColorScheme()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();
            ConsoleDestinationBuilder consoleDestinationBuilder = new ConsoleDestinationBuilder(logGroupDestinationsBuilder); 

            // Act
            consoleDestinationBuilder.Add();

            // Assert 
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(consoleDestinationBuilder.LogSerializer);
            Assert.NotNull(consoleDestinationBuilder.ColorScheme);
        }

        #endregion
    }
}
