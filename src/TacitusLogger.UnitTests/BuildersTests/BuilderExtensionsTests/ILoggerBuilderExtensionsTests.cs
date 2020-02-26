using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class ILoggerBuilderExtensionsTests
    {
        #region Tests for WithGuidLogId extension method

        [Test]
        public void WithGuidLogId_Taking_ILoggerBuilder_When_Called_Calls_WithLogIdGenerator_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();

            // Act 
            ILoggerBuilderExtensions.WithGuidLogId(loggerBuilderMock.Object);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogIdGenerator(It.IsNotNull<GuidLogIdGenerator>()), Times.Once);
        }
        [Test]
        public void WithGuidLogId_Taking_ILoggerBuilder_And_GuidFormat_WhenCalled_Calls_WithLogIdGenerator_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            string guidFormat = "guidFormat";

            // Act 
            ILoggerBuilderExtensions.WithGuidLogId(loggerBuilderMock.Object, guidFormat);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogIdGenerator(It.Is<GuidLogIdGenerator>(g => g.GuidFormat == guidFormat)), Times.Once);
        }
        [Test]
        public void WithGuidLogId_Taking_ILoggerBuilder_And_GuidSubstring_Length_When_Called_Calls_WithLogIdGenerator_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            int guidSubstringLength = 4;

            // Act 
            ILoggerBuilderExtensions.WithGuidLogId(loggerBuilderMock.Object, guidSubstringLength);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogIdGenerator(It.Is<GuidLogIdGenerator>(g => g.SubstringLength == guidSubstringLength)), Times.Once);
        }
        [Test]
        public void WithGuidLogId_Taking_ILoggerBuilder_And_GuidFormatGuid_And_Substring_Length_When_Called_Calls_WithLogIdGenerator_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            string guidFormat = "guidFormat";
            int guidSubstringLength = 4;

            // Act 
            ILoggerBuilderExtensions.WithGuidLogId(loggerBuilderMock.Object, guidFormat, guidSubstringLength);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogIdGenerator(It.Is<GuidLogIdGenerator>(g => g.SubstringLength == guidSubstringLength && g.GuidFormat == guidFormat)), Times.Once);
        }

        #endregion

        #region Tests for WithNullLogId extension method

        [Test]
        public void WithNullLogId_Taking_ILoggerBuilder_When_Called_Calls_WithLogIdGenerator_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();

            // Act 
            ILoggerBuilderExtensions.WithNullLogId(loggerBuilderMock.Object);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogIdGenerator(It.IsNotNull<NullLogIdGenerator>()), Times.Once);
        }

        #endregion

        #region Tests for WithLogCreation extension method

        [Test]
        public void WithLogCreation_Taking_LogCreationStrategy_And_UseUtcTime_Flag_When_Called_Calls_WithLogCreation_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            LogCreation logCreationStrategyEnum = LogCreation.Standard;
            bool useUtcTime = true;

            // Act 
            ILoggerBuilderExtensions.WithLogCreation(loggerBuilderMock.Object, logCreationStrategyEnum, useUtcTime);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogCreation(It.Is<StandardLogCreationStrategy>(s => s.UseUtcTime == useUtcTime)), Times.Once);
        }
        [Test]
        public void WithLogCreation_Taking_LogCreationStrategy_And_UseUtcTime_Flag_When_Called_Without_Utc_Time_Flag_Sets_Flag_To_False()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            LogCreation logCreationStrategyEnum = LogCreation.Standard;

            // Act 
            ILoggerBuilderExtensions.WithLogCreation(loggerBuilderMock.Object, logCreationStrategyEnum);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogCreation(It.Is<StandardLogCreationStrategy>(s => s.UseUtcTime == false)), Times.Once);
        }
        [Test]
        public void WithLogCreation_Taking_LogCreationStrategy_And_UseUtcTime_Flag_When_Called_Returns_The_Result_Of_WithLogCreation_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            var loggerBuilderReturned = new Mock<ILoggerBuilder>().Object;
            loggerBuilderMock.Setup(x => x.WithLogCreation(It.IsAny<LogCreationStrategyBase>())).Returns(loggerBuilderReturned);

            // Act 
            var returned = ILoggerBuilderExtensions.WithLogCreation(loggerBuilderMock.Object, LogCreation.Standard, true);

            // Assert
            Assert.AreEqual(loggerBuilderReturned, returned);
        }

        #endregion

        #region Tests for WithExceptionHandling extension method

        [Test]
        public void WithExceptionHandling_Taking_ExceptionHandlingStrategy_When_Called_Calls_WithExceptionHandling_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            ExceptionHandling exceptionHandlingStrategyEnum = ExceptionHandling.Log;

            // Act 
            ILoggerBuilderExtensions.WithExceptionHandling(loggerBuilderMock.Object, exceptionHandlingStrategyEnum);

            // Assert
            loggerBuilderMock.Verify(x => x.WithExceptionHandling(It.IsNotNull<LogExceptionHandlingStrategy>()), Times.Once);
        }
        [Test]
        public void WithExceptionHandling_Taking_ExceptionHandlingStrategy_When_Called_Without_Utc_Time_Flag_Sets_Flag_To_False()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            ExceptionHandling exceptionHandlingStrategyEnum = ExceptionHandling.Log;

            // Act 
            ILoggerBuilderExtensions.WithExceptionHandling(loggerBuilderMock.Object, exceptionHandlingStrategyEnum);

            // Assert
            loggerBuilderMock.Verify(x => x.WithExceptionHandling(It.IsNotNull<LogExceptionHandlingStrategy>()), Times.Once);
        }
        [Test]
        public void WithExceptionHandling_Taking_ExceptionHandlingStrategy_When_Called_Returns_The_Result_Of_WithExceptionHandling_Method_Of_ILoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            var loggerBuilderReturned = new Mock<ILoggerBuilder>().Object;
            loggerBuilderMock.Setup(x => x.WithExceptionHandling(It.IsAny<ExceptionHandlingStrategyBase>())).Returns(loggerBuilderReturned);

            // Act 
            var returned = ILoggerBuilderExtensions.WithExceptionHandling(loggerBuilderMock.Object, ExceptionHandling.Rethrow);

            // Assert
            Assert.AreEqual(loggerBuilderReturned, returned);
        }

        #endregion

        #region Tests for WithLogLevel method

        [Test]
        public void WithLogLevel_When_Called_Calls_WithLogLevelMethod_Of_LoggerBuilder()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            LogLevel logLevel = LogLevel.Warning;

            // Act 
            ILoggerBuilderExtensions.WithLogLevel(loggerBuilderMock.Object, logLevel);

            // Assert
            loggerBuilderMock.Verify(x => x.WithLogLevel(It.Is<MutableSetting<LogLevel>>(s => s.Value == logLevel)), Times.Once);
        }

        [Test]
        public void WithLogLevel_When_Called_Calls_WithLogLevel_Method_Of_Logger_Builder_And_Returns_Its_Result()
        {
            // Arrange 
            var loggerBuilderMock = new Mock<ILoggerBuilder>();
            var expectedLoggerBuilder = new Mock<ILoggerBuilder>().Object;
            loggerBuilderMock.Setup(x => x.WithLogLevel(It.IsAny<Setting<LogLevel>>())).Returns(expectedLoggerBuilder);

            // Act 
            var returnedResult = ILoggerBuilderExtensions.WithLogLevel(loggerBuilderMock.Object, LogLevel.Warning);

            // Assert
            Assert.AreEqual(expectedLoggerBuilder, returnedResult);
        }

        #endregion
    }
}
