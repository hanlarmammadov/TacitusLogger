using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.LogIdGenerators;

namespace TacitusLogger.UnitTests.LogIdGeneratorTests
{
    [TestFixture]
    public class GuidLogIdGeneratorTests
    {
        #region Ctors tests
        [Test]
        public void Constructor_WhenCalledWithDefaultParams_GuidFormatAndSubstringLengthSetDefaults()
        {
            // Act
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator();

            // Assert  
            Assert.AreEqual(guidLogIdGenerator.GuidFormat, "N");
            Assert.AreEqual(guidLogIdGenerator.SubstringLength, 0);
        }

        [Test]
        public void Constructor_WhenCalledWithParams_GuidFormatAndSubstringLengthAreSetRight()
        {
            // Act
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator("format1", 4);

            // Assert  
            Assert.AreEqual(guidLogIdGenerator.GuidFormat, "format1");
            Assert.AreEqual(guidLogIdGenerator.SubstringLength, 4);
        }

        #endregion

        #region Generate and GenerateAsync method tests

        [TestCase("N")]
        [TestCase("D")]
        [TestCase("B")]
        [TestCase("P")]
        [TestCase("X")]
        public async Task Generate_WhenCalled_ReturnsValidGuidInAccordingFormat(string guidFormat)
        {
            // Arrange
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator(guidFormat);

            // Act
            var guidString = guidLogIdGenerator.Generate(new LogModel());
            var guidStringAsync = await guidLogIdGenerator.GenerateAsync(new LogModel());

            // Assert   
            Assert.AreNotEqual(Guid.Empty, Guid.ParseExact(guidString, guidFormat));
            Assert.AreNotEqual(Guid.Empty, Guid.ParseExact(guidStringAsync, guidFormat));
        }

        [Test]
        public async Task Generate_WhenCalledWithNullLogModel_ReturnsValidGuid()
        {
            // Arrange
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator("N");

            // Act
            var guidString = guidLogIdGenerator.Generate(null);
            var guidStringAsync = await guidLogIdGenerator.GenerateAsync(null);

            // Assert   
            Assert.AreNotEqual(Guid.Empty, Guid.ParseExact(guidString, "N"));
            Assert.AreNotEqual(Guid.Empty, Guid.ParseExact(guidStringAsync, "N"));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(32)]
        public async Task Generate_WhenSubstringLengthIsNot0_ReturnsValidSubstring(int substringLength)
        {
            // Arrange
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator("N", substringLength);

            // Act
            var guidString = guidLogIdGenerator.Generate(new LogModel());
            var guidStringAsync = await guidLogIdGenerator.GenerateAsync(new LogModel());

            // Assert   
            if (substringLength == 0)
            {
                Assert.AreEqual(guidString.Length, 32);
                Assert.AreEqual(guidStringAsync.Length, 32);

            }
            else
            {
                Assert.AreEqual(guidString.Length, substringLength);
                Assert.AreEqual(guidStringAsync.Length, substringLength);
            }
        }

        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public void Generate_WhenSubstringLengthIsInvalid_ThrowsException(int substringLength)
        {
            // Arrange
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator("N", substringLength);

            // Assert   
            Assert.Catch<Exception>(() =>
            {
                // Act
                var guidString = guidLogIdGenerator.Generate(new LogModel());
            });
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                var guidString = await guidLogIdGenerator.GenerateAsync(new LogModel());
            });
        }
         
        [Test]
        public void GenerateAsync_When_Called_With_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange
            GuidLogIdGenerator guidLogIdGenerator = new GuidLogIdGenerator();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act 
                await guidLogIdGenerator.GenerateAsync(null, cancellationToken);
            });
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Log_Id_Generator()
        {
            // Arrange
            GuidLogIdGenerator logIdGenerator = new GuidLogIdGenerator("D", 7);

            // Act
            var result = logIdGenerator.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.LogIdGenerators.GuidLogIdGenerator"));
            Assert.IsTrue(result.Contains("Guid format: D"));
            Assert.IsTrue(result.Contains("Substring length: 7"));
        }

        #endregion
    }
}
