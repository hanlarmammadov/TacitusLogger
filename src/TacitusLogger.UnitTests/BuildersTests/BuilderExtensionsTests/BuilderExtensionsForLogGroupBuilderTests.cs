using Moq;
using NUnit.Framework;
using System.Reflection;
using TacitusLogger.Builders;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class BuilderExtensionsForLogGroupBuilderTests
    {
        #region Tests for File extension method

        [Test]
        public void File_Taking_LogGroupDestinationsBuilder_WhenCalled_Returns_FileDestinationBuilder_With_LogGroupDestinationsBuilder_Set()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            IFileDestinationBuilder fileDestinationBuilder = BuilderExtensionsForLogGroupBuilder.File(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<FileDestinationBuilder>(fileDestinationBuilder);
            Assert.AreEqual(logGroupDestinationsBuilder, (fileDestinationBuilder as FileDestinationBuilder).LogGroupDestinationsBuilder);
        }
         
        #endregion

        #region Tests for Console extension method

        [Test]
        public void Console_Taking_LogGroupDestinationsBuilder_WhenCalled_Returns_ConsoleDestinationBuilder_With_LogGroupDestinationsBuilder_Set()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            IConsoleDestinationBuilder consoleDestinationBuilder = BuilderExtensionsForLogGroupBuilder.Console(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<ConsoleDestinationBuilder>(consoleDestinationBuilder);
            Assert.AreEqual(logGroupDestinationsBuilder, (consoleDestinationBuilder as ConsoleDestinationBuilder).LogGroupDestinationsBuilder);
        }
 
        #endregion

        #region Tests for Debug extension method

        [Test]
        public void Debug_Taking_LogGroupDestinationsBuilder_WhenCalled_Returns_DebugDestinationBuilder_With_LogGroupDestinationsBuilder_Set()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            IDebugDestinationBuilder debugDestinationBuilder = BuilderExtensionsForLogGroupBuilder.Debug(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<DebugDestinationBuilder>(debugDestinationBuilder);
            Assert.AreEqual(logGroupDestinationsBuilder, (debugDestinationBuilder as DebugDestinationBuilder).LogGroupDestinationsBuilder);
        }
         
        #endregion 

        #region Tests for TextWriter extension method

        [Test]
        public void TextWriter_Taking_LogGroupDestinationsBuilder_WhenCalled_Returns_TextWriterDestinationBuilder_With_LogGroupDestinationsBuilder_Set()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            ITextWriterDestinationBuilder textWriterDestinationBuilder = BuilderExtensionsForLogGroupBuilder.TextWriter(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<TextWriterDestinationBuilder>(textWriterDestinationBuilder);
            Assert.AreEqual(logGroupDestinationsBuilder, (textWriterDestinationBuilder as TextWriterDestinationBuilder).LogGroupDestinationsBuilder);
        }

        #endregion
    }
}
