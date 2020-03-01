using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Destinations.TextWriter;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class TextWriterDestinationBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_LogGroupDestinationsBuilder()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, textWriterDestinationBuilder.LogGroupDestinationsBuilder);
        }

        [Test]
        public void Ctor_When_Called_ILogSerializer_And_ITextWriterProvider_Are_Null()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(logGroupDestinationsBuilder);

            // Assert
            Assert.IsNull(textWriterDestinationBuilder.LogSerializer);
            Assert.IsNull(textWriterDestinationBuilder.TextWriterProvider);
        }

        #endregion

        #region Tests for WithLogSerializer method

        [Test]
        public void WithLogSerializer_When_Called_Sets_LogSerializer_And_Returns_Self()
        {
            // Arrange 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            var returned = textWriterDestinationBuilder.WithLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(logSerializer, textWriterDestinationBuilder.LogSerializer);
            Assert.AreEqual(returned, textWriterDestinationBuilder);
        }

        [Test]
        public void WithLogSerializer_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(logGroupDestinationsBuilder);
            // Set first time.
            textWriterDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Tried to set second time.
                textWriterDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithLogSerializer_When_Called_With_Null_ILogSerializer_Throws_ArgumentNullException()
        {
            // Arrange 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                textWriterDestinationBuilder.WithLogSerializer(null as ILogSerializer);
            });
        }


        #endregion

        #region Tests for WithWriter method

        [Test]
        public void WithWriter_When_Called_Sets_TextWriterProvider_And_Returns_Self()
        {
            // Arrange 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var textWriterProvider = new Mock<ITextWriterProvider>().Object;

            // Act
            var returned = textWriterDestinationBuilder.WithWriter(textWriterProvider);

            // Assert
            Assert.AreEqual(textWriterProvider, textWriterDestinationBuilder.TextWriterProvider);
            Assert.AreEqual(returned, textWriterDestinationBuilder);
        }
         
        [Test]
        public void WithWriter_When_Called_With_Null_ITextWriterProvider_Throws_ArgumentNullException()
        {
            // Arrange 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                textWriterDestinationBuilder.WithWriter(null as ITextWriterProvider);
            });
        }

        [Test]
        public void WithWriter_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            // Sets ITextWriterProvider first time here.
            textWriterDestinationBuilder.WithWriter(new Mock<ITextWriterProvider>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                textWriterDestinationBuilder.WithWriter(new Mock<ITextWriterProvider>().Object);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_When_Called_Given_That_AddDependencies_Was_Set_Creates_New_TextWriterDestination_And_Passes_To_CustomDestination_Method_Of_LogGroupDestinationsBuilder()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>(); 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            var logSerializer = new Mock<ILogSerializer>().Object;
            var textWriterProvider = new Mock<ITextWriterProvider>().Object;
            textWriterDestinationBuilder.WithLogSerializer(logSerializer);
            textWriterDestinationBuilder.WithWriter(textWriterProvider);

            // Act
            textWriterDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<TextWriterDestination>(d => d.LogSerializer == logSerializer && d.TextWriterProvider == textWriterProvider)), Times.Once);
        }

        [Test]
        public void Add_When_Called_Given_That_TextWriterProvider_Was_Not_Set_Throws_InvalidOperationException()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(logGroupDestinationsBuilder);
            textWriterDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                textWriterDestinationBuilder.Add();
            });
        }

        [Test]
        public void Add_When_Called_Given_That_ILogSerializer_Was_Not_Set_Sets_Default_ILogSerializer()
        {
            // Arrange
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>(); 
            TextWriterDestinationBuilder textWriterDestinationBuilder = new TextWriterDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            var textWriterProvider = new Mock<ITextWriterProvider>().Object;
            textWriterDestinationBuilder.WithWriter(textWriterProvider);

            // Act
            textWriterDestinationBuilder.Add();

            // Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<TextWriterDestination>(d => d.LogSerializer is SimpleTemplateLogSerializer && d.TextWriterProvider == textWriterProvider)), Times.Once);
        }

        #endregion
    }
}
