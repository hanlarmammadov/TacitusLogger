using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.LogIdGenerators;

namespace TacitusLogger.UnitTests.LogIdGeneratorTests
{
    [TestFixture]
    public class NullLogIdGeneratorTests
    {
        [Test]
        public void Generate_When_Called_Returns_Null_String()
        {
            // Arrange
            NullLogIdGenerator nullLogIdGenerator = new NullLogIdGenerator();

            // Act
            string resultId = nullLogIdGenerator.Generate(new LogModel());

            // Assert
            Assert.IsNull(resultId);
        }
        [Test]
        public void Generate_When_Called_With_Null_Log_Model_Returns_Null_String()
        {
            // Arrange
            NullLogIdGenerator nullLogIdGenerator = new NullLogIdGenerator();

            // Act
            string resultId = nullLogIdGenerator.Generate(null as LogModel);

            // Assert
            Assert.IsNull(resultId);
        }
        [Test]
        public async Task GenerateAsync_When_Called_Returns_Null_String()
        {
            // Arrange
            NullLogIdGenerator nullLogIdGenerator = new NullLogIdGenerator();

            // Act
            string resultId = await nullLogIdGenerator.GenerateAsync(new LogModel());

            // Assert
            Assert.IsNull(resultId);
        }
        [Test]
        public async Task GenerateAsync_When_Called_With_Null_Log_Model_Returns_Null_String()
        {
            // Arrange
            NullLogIdGenerator nullLogIdGenerator = new NullLogIdGenerator();

            // Act
            string resultId = await nullLogIdGenerator.GenerateAsync(null as LogModel);

            // Assert
            Assert.IsNull(resultId);
        }
        [Test]
        public void GenerateAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            // Arrange
            NullLogIdGenerator nullLogIdGenerator = new NullLogIdGenerator();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Act
            Task resultTask = nullLogIdGenerator.GenerateAsync(new LogModel(), cancellationToken);

            // Assert
            Assert.IsTrue(resultTask.IsCanceled);
        }
    }
}
