using Moq;
using NUnit.Framework;
using System.IO;
using TacitusLogger.Builders;
using TacitusLogger.Destinations.TextWriter;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class ITextWriterDestinationBuilderExtensionsTests
    {
        #region extension method calling WithWriter method

        [Test]
        public void WithWriter_Taking_ITextWriterDestinationBuilder_And_TextWriter_WhenCalled_Calls_WithWriter_Method_Of_ITextWriterDestinationBuilder()
        {
            // Arrange 
            var textWriterDestinationBuilderMock = new Mock<ITextWriterDestinationBuilder>();
            var textWriter = new Mock<TextWriter>().Object;

            // Act 
            ITextWriterDestinationBuilderExtensions.WithWriter(textWriterDestinationBuilderMock.Object, textWriter);

            // Assert
            textWriterDestinationBuilderMock.Verify(x => x.WithWriter(It.Is<TextWriterProvider>(g => g.TextWriter == textWriter)), Times.Once);
        }

        [Test]
        public void WithWriter_Taking_ITextWriterDestinationBuilder_And_FactoryMethod_WhenCalled_Calls_WithWriter_Method_Of_ITextWriterDestinationBuilder()
        {
            // Arrange 
            var textWriterDestinationBuilderMock = new Mock<ITextWriterDestinationBuilder>();
            LogModelFunc<TextWriter> factoryMethod = d => null;

            // Act 
            ITextWriterDestinationBuilderExtensions.WithWriter(textWriterDestinationBuilderMock.Object, factoryMethod);
            
            // Assert
            textWriterDestinationBuilderMock.Verify(x => x.WithWriter(It.Is<FactoryMethodTextWriterProvider>(g => g.FactoryMethod == factoryMethod)), Times.Once);
        }

        #endregion
    }
}
