using Moq;
using NUnit.Framework;
using System;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class ExtensionsForLogBuilderTests
    {
        [Test]
        public void WithEx_Taking_LogBuilderBase_And_Exception_When_Called_Calls_LogBuilderBase_With_Method_And_Returns_Its_Result()
        {
            // Arrange
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Error, "");
            var logBuilderBaseMock = new Mock<LogBuilderBase<LogBuilder>>();
            logBuilderBaseMock.Setup(x => x.With(It.IsAny<LogItem>())).Returns(logBuilder);
            Exception ex = new Exception();

            // Act
            var returnedLog = ExtensionsForLogBuilder.WithEx(logBuilderBaseMock.Object, ex);

            // Assert
            logBuilderBaseMock.Verify(x => x.With(It.Is<LogItem>(i => i.Name == "Exception" && i.Value == ex)), Times.Once);
            Assert.AreEqual(logBuilder, returnedLog);
        }
        [Test]
        public void WithStackTrace_Taking_LogBuilderBase_When_Called_Calls_LogBuilderBase_With_Method_And_Returns_Its_Result()
        {
            // Arrange
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Error, "");
            var logBuilderBaseMock = new Mock<LogBuilderBase<LogBuilder>>();
            logBuilderBaseMock.Setup(x => x.With(It.IsAny<LogItem>())).Returns(logBuilder);

            // Act
            var returnedLog = ExtensionsForLogBuilder.WithStackTrace(logBuilderBaseMock.Object);

            // Assert 
            logBuilderBaseMock.Verify(x => x.With(It.Is<LogItem>(i => i.Name == "Stack trace" && i.Value != null)), Times.Once);
            Assert.AreEqual(logBuilder, returnedLog);
        }
    }
}
