using Moq;
using NUnit.Framework;
using System;
using System.IO; 
using System.Threading.Tasks;
using TacitusLogger.Destinations.Console; 

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class StandardColorConsoleFacadeTests : IntegrationTestBase
    {
        #region Tests for WriteLine method
         
        [Test]
        public void WriteLine_WhenCalled_CallsConsoleWriteLineWithText()
        {
            // Arrange
            string text = "Some text";
            var textWriterMock = new Mock<TextWriter>();
            Console.SetOut(textWriterMock.Object);
            StandardColorConsoleFacade standardColorConsoleFacade = new StandardColorConsoleFacade();

            // Act
            standardColorConsoleFacade.WriteLine(text, ConsoleColor.White);

            // Assert
            textWriterMock.Verify(x => x.WriteLine(text), Times.Once);
        }

        [Test]
        public void WriteLine_When_Called_With_Null_Text_Does_Not_Throw()
        {
            // Arrange
            StandardColorConsoleFacade standardColorConsoleFacade = new StandardColorConsoleFacade();

            // Act
            standardColorConsoleFacade.WriteLine(null, ConsoleColor.White);
        }
         
        #endregion

        #region Tests for WriteLineAsync method
         
        [Test]
        public async Task WriteLineAsync_WhenCalled_CallsConsoleWriteLineWithText()
        {
            // Arrange
            string text = "Some text";
            var textWriterMock = new Mock<TextWriter>();
            Console.SetOut(textWriterMock.Object);
            StandardColorConsoleFacade standardColorConsoleFacade = new StandardColorConsoleFacade();

            // Act
            await standardColorConsoleFacade.WriteLineAsync(text, ConsoleColor.White);

            // Assert
            textWriterMock.Verify(x => x.WriteLine(text), Times.Once);
        }

        [Test]
        public async Task WriteLineAsync_When_Called_With_Null_Params_Does_Not_Throw()
        {
            // Arrange
            StandardColorConsoleFacade standardColorConsoleFacade = new StandardColorConsoleFacade();
             
            // Act
            await standardColorConsoleFacade.WriteLineAsync(null, ConsoleColor.White);
        }

        #endregion
    }
}
