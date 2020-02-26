using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Contributors;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class ExtensionsForLogTests
    {
        [Test]
        public void WithEx_Taking_LogBuilderBase_And_Exception_When_Called_Calls_LogBuilderBase_With_Method_And_Returns_Its_Result()
        {
            // Arrange
            Log log = new Log();
            var logBuilderBaseMock = new Mock<LogBuilderBase<Log>>();
            logBuilderBaseMock.Setup(x => x.With(It.IsAny<LogItem>())).Returns(log);
            Exception ex = new Exception();

            // Act
            var returnedLog = ExtensionsForLog.WithEx(logBuilderBaseMock.Object, ex);

            // Assert
            logBuilderBaseMock.Verify(x => x.With(It.Is<LogItem>(i => i.Name == "Exception" && i.Value == ex)), Times.Once);
            Assert.AreEqual(log, returnedLog);
        }
        [Test]
        public void WithStackTrace_Taking_LogBuilderBase_When_Called_Calls_LogBuilderBase_With_Method_And_Returns_Its_Result()
        {
            // Arrange
            Log log = new Log();
            var logBuilderBaseMock = new Mock<LogBuilderBase<Log>>();
            logBuilderBaseMock.Setup(x => x.With(It.IsAny<LogItem>())).Returns(log);

            // Act
            var returnedLog = ExtensionsForLog.WithStackTrace(logBuilderBaseMock.Object);

            // Assert 
            logBuilderBaseMock.Verify(x => x.With(It.Is<LogItem>(i => i.Name == "Stack trace" && i.Value != null)), Times.Once);
            Assert.AreEqual(log, returnedLog);
        }
    }
}
