using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TacitusLogger.Destinations.Debug;

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class DebugConsoleFacadeTests : IntegrationTestBase
    {
        #region Tests for WriteLine method

        [Test]
        public void WriteLine_WhenCalledWithNotNullParams_DoesNotThrowsException()
        {
            // Arrange
            DebugConsoleFacade debugConsoleFacade = new DebugConsoleFacade();

            // Act
            debugConsoleFacade.WriteLine("Some text");
        }
        [Test]
        public void WriteLine_When_Called_With_Null_Text_Does_Not_Throws()
        {
            // Arrange
            DebugConsoleFacade debugConsoleFacade = new DebugConsoleFacade();

            // Act
            debugConsoleFacade.WriteLine(null);
        }

        #endregion

        #region Tests for WriteLineAsync method

        [Test]
        public async Task WriteLineAsync_WhenCalledWithNotNullParams_DoesNotThrowsException()
        {
            // Arrange
            DebugConsoleFacade debugConsoleFacade = new DebugConsoleFacade();

            // Act
            await debugConsoleFacade.WriteLineAsync("Some text");
        }
        [Test]
        public void WriteLineAsync_WhenCalledWithNullText_ThrowsArgumentNullException()
        {
            // Arrange
            DebugConsoleFacade debugConsoleFacade = new DebugConsoleFacade();

            Assert.CatchAsync<ArgumentNullException>(async () =>
            {
                // Act
                await debugConsoleFacade.WriteLineAsync(null);
            });
        }

        #endregion
    }
}
