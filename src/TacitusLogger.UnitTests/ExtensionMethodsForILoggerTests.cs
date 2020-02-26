using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class ExtensionMethodsForILoggerTests
    {
        private static readonly string _context = "context";
        private static readonly string _description = "description";
        private static readonly string[] _tags = new string[] { "tag1", "tag2" };
        private static readonly LogItem[] _logItems = new LogItem[] { new LogItem("item1", new { }), new LogItem("item2", new { }), };
        private static readonly LogItem _logItem = new LogItem("item", new { });
        private static readonly Object _logItemObj = new { a = "a" };

        private Mock<ILogger> LoggerWithLogMethodThatReturnsNotNullLogId(string logId)
        {
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<Log>())).Returns(logId);
            return loggerMock;
        }
        private Mock<ILogger> LoggerWithLogAsyncMethodThatReturnsNotNullLogId(string logId)
        {
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.LogAsync(It.IsAny<Log>(), It.IsAny<CancellationToken>())).ReturnsAsync(logId);
            return loggerMock;
        }

        #region Test for Log method overloads

        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_Context_LogType_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, _context, LogType.Error, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_LogType_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, LogType.Error, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_LogType_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, LogType.Error, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_LogType_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, LogType.Error, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void Log_Taking_ILogger_LogType_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.Log(loggerMock.Object, LogType.Error, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogInfo method overloads

        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogInfo_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogInfo(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogSuccess method overloads

        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogSuccess_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogSuccess(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogEvent method overloads

        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogEvent_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogEvent(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogWarning method overloads

        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogWarning_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogWarning(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Warning && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogError method overloads

        [Test]
        public void LogError_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogError_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogError(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogFailure method overloads

        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogFailure_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogFailure(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogCritical method overloads

        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _tags, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _tags, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _tags, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _tags);

            // Assert 
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _context, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _description, _logItems);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _description, _logItem);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _description, _logItemObj);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj)), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public void LogCritical_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogMethodThatReturnsNotNullLogId("logid");

            // Act
            var logId = ExtensionMethodsForILogger.LogCritical(loggerMock.Object, _description);

            // Assert
            loggerMock.Verify(x => x.Log(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0)), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogAsync method overloads

        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_Tags_LogItems_Array_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_Tags_LogItem_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_Tags_LogItem_Object_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_Tags_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_LogItems_Array_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_LogItem_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_LogItem_Object_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_Context_LogType_Description_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, _context, LogType.Error, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_LogType_Description_LogItems_Array_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, LogType.Error, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_LogType_Description_LogItem_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, LogType.Error, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_LogType_Description_LogItem_Object_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, LogType.Error, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogAsync_Taking_ILogger_LogType_Description_When_Called_Calls_LogAsync_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogAsync(loggerMock.Object, LogType.Error, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogInfoAsync method overloads

        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _context, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogInfoAsync_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogInfoAsync(loggerMock.Object, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Info && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogSuccessAsync method overloads

        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _context, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogSuccessAsync_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogSuccessAsync(loggerMock.Object, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Success && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogEventAsync method overloads

        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _context, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogEventAsync_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogEventAsync(loggerMock.Object, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Event && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogErrorAsync method overloads

        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _context, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogErrorAsync_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogErrorAsync(loggerMock.Object, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Error && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogFailureAsync method overloads

        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _context, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogFailureAsync_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogFailureAsync(loggerMock.Object, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Failure && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion

        #region Test for LogCriticalAsync method overloads

        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_Tags_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _tags, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_Tags_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _tags, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_Tags_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _tags, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_Tags_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _tags, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags == _tags), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Context_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _context, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == _context && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Description_LogItems_Array_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _description, _logItems, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items == _logItems), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Description_LogItem_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _description, _logItem, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0] == _logItem), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Description_LogItem_Object_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _description, _logItemObj, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items[0].Value == _logItemObj), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }
        [Test]
        public async Task LogCriticalAsync_Taking_ILogger_Description_When_Called_Calls_Log_Method_Of_ILogger()
        {
            // Arrange
            var loggerMock = LoggerWithLogAsyncMethodThatReturnsNotNullLogId("logid");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            var logId = await ExtensionMethodsForILogger.LogCriticalAsync(loggerMock.Object, _description, cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(It.Is<Log>(l => l.Context == null && l.Type == LogType.Critical && l.Description == _description && l.Tags.Count == 0 && l.Items.Count == 0), cancellationToken), Times.Once);
            Assert.AreEqual("logid", logId);
        }

        #endregion
    }
}
