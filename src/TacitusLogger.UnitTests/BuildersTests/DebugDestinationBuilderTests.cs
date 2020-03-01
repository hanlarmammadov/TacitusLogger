using Moq;
using NUnit.Framework;
using System; 
using TacitusLogger.Builders;
using TacitusLogger.Destinations.Debug;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class DebugDestinationBuilderTests
    {
        #region Tests for Ctor

        [Test]
        public void Ctor_WithLogGroupDestinationsBuilder_WhenCalled_InitializesLogGroupDestinationsBuilder()
        {
            // Arrange
            var loggerBuilder = LoggerBuilder.Logger();
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = loggerBuilder.NewLogGroup().ForAllLogs();

            // Act
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, debugDestinationBuilder.LogGroupDestinationsBuilder);
        }

        #endregion

        #region Tests for WithLogSerializer method

        [Test]
        public void WithLogSerializer_WhenCalled_SetsLogSerializer()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            debugDestinationBuilder.WithLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(logSerializer, debugDestinationBuilder.LogSerializer);
        }

        [Test]
        public void WithLogSerializer_WhenCalled_ReturnsDebugDestinationBuilder()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilder);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            IDebugDestinationBuilder returned = debugDestinationBuilder.WithLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(debugDestinationBuilder, returned);
        }

        [Test]
        public void WithLogSerializer_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilder);
            // Set first time.
            debugDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Tried to set second time.
                debugDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithLogSerializer_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                debugDestinationBuilder.WithLogSerializer(null);
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
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilder);

            // Act
            ILogGroupDestinationsBuilder logGroupDestinationsBuilderReturned = debugDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, logGroupDestinationsBuilderReturned);
        }
         
        [Test]
        public void Add_When_Called_Creates_New_DebugDestination_And_Passes_It_To_CustomDestination_Method_Of_LogGroupDestinationsBuilder()
        {
            // Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>(); 
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            // Act
            debugDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.IsNotNull<DebugDestination>()), Times.Once);
        }

        [Test]
        public void Add_When_Called_Given_That_Custom_LogSerializer_Was_Provided_Returns_Creates_DebugDestinationBuilder_With_That_LogSerializer()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            DebugDestinationBuilder debugDestinationBuilder   = new DebugDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;
            debugDestinationBuilder.WithLogSerializer(logSerializer);

            // Act
            debugDestinationBuilder.Add();

            // Assert 
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<DebugDestination>(d=>d.LogSerializer== logSerializer)), Times.Once);
        }

        [Test]
        public void Add_WhenCalledGivenThatLogSerializerWasNotProvided_ReturnsLogGroupDestinationsBuilderWithDefaultLogSerializer()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            DebugDestinationBuilder debugDestinationBuilder = new DebugDestinationBuilder(logGroupDestinationsBuilderMock.Object); 

            // Act
            debugDestinationBuilder.Add();

            // Assert 
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<DebugDestination>(d => d.LogSerializer is SimpleTemplateLogSerializer)), Times.Once);
        } 

        #endregion
    }
}
