using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations.TextWriter;
using TacitusLogger.Serializers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.DestinationTests
{
    [TestFixture]
    public class TextWriterDestinationTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_LogSerializer_And_TextWriterProvider_When_Called_Sets_LogSerializer_And_TextWriterProvider()
        {
            // Arrange
            var logSerializer = new Mock<ILogSerializer>().Object;
            var textWriterProvider = new Mock<ITextWriterProvider>().Object;

            // Act
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializer, textWriterProvider);

            // Assert
            Assert.AreEqual(logSerializer, textWriterDestination.LogSerializer);
            Assert.AreEqual(textWriterProvider, textWriterDestination.TextWriterProvider);

        }

        [Test]
        public void Ctor_Taking_LogSerializer_And_TextWriterProvider_When_Called_With_Null_LogSerializer_Throws_ArgumentNullException()
        {
            // Arrange 
            var textWriterProvider = new Mock<ITextWriterProvider>().Object;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TextWriterDestination textWriterDestination = new TextWriterDestination(null as ILogSerializer, textWriterProvider);
            });
        }

        [Test]
        public void Ctor_Taking_LogSerializer_And_TextWriterProvider_When_Called_With_Null_TextWriterProvider_Throws_ArgumentNullException()
        {
            // Arrange
            var logSerializer = new Mock<ILogSerializer>().Object;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializer, null as ITextWriterProvider);
            });
        }

        #endregion

        #region Tests for Send method

        [Test]
        public void Send_When_Called_Generate_And_Send_Log_String_To_TextWriters_Write_Method()
        {
            // Arrange
            LogModel logModel = new LogModel();
            // ILogSerializer
            var logSerializerMock = new Mock<ILogSerializer>();
            string serialized = "serialized log string";
            logSerializerMock.Setup(x => x.Serialize(logModel)).Returns(serialized);
            // ITextWriterProvider
            var textWriterMock = new Mock<TextWriter>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            textWriterProviderMock.Setup(x => x.GetTextWriter(logModel)).Returns(textWriterMock.Object);
            // TextWriterDestination 
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            textWriterDestination.Send(new LogModel[] { logModel });

            // Assert
            textWriterMock.Verify(x => x.Write(serialized + Environment.NewLine), Times.Once);
            textWriterMock.Verify(x => x.Flush(), Times.Once);
        }

        [Test]
        public void Send_When_TextWriterProvider_Returns_Null_TextWriter_Throws_Exception()
        {
            // Arrange
            LogModel logModel = new LogModel();
            // ILogSerializer
            var logSerializerMock = new Mock<ILogSerializer>();
            string serialized = "serialized log string";
            logSerializerMock.Setup(x => x.Serialize(logModel)).Returns(serialized);
            // ITextWriterProvider 
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            textWriterProviderMock.Setup(x => x.GetTextWriter(logModel)).Returns(null as TextWriter);
            // TextWriterDestination 
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Assert
            Assert.Catch<Exception>(() =>
            {
                // Act
                textWriterDestination.Send(new LogModel[] { logModel });
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_N_Different_LogModel_Which_Correspond_To_N_Different_TextWriters_Calls_Each_TextWriter_Method_One_Time(int N)
        {
            // Arrange 
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();

            var dict = new Dictionary<LogModel, Mock<TextWriter>>();
            for (int i = 0; i < N; i++)
                dict.Add(new LogModel() { Description = $"logText{i}" }, new Mock<TextWriter>());

            textWriterProviderMock.Setup(x => x.GetTextWriter(It.IsAny<LogModel>())).Returns((LogModel x) => dict[x].Object);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);

            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            textWriterDestination.Send(dict.Keys.ToArray());

            // Assert  
            foreach (LogModel logModel in dict.Keys)
            {
                dict[logModel].Verify(x => x.Write(logModel.Description + Environment.NewLine), Times.Once);
                dict[logModel].Verify(x => x.Flush(), Times.Once);
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_N_Different_LogModel_That_Corresponds_To_The_Same_TextWriter_Calls_TextWriter_Method_Once_And_Passes_Concatenated_Log_Texts(int N)
        {
            // Arrange 
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            var textWriterMock = new Mock<TextWriter>();
            LogModel[] logs = new LogModel[N];
            StringBuilder concatedLogTexts = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Description = $"logText{i}" };
                concatedLogTexts.AppendLine($"logText{i}");
            }
            textWriterProviderMock.Setup(x => x.GetTextWriter(It.IsAny<LogModel>())).Returns(textWriterMock.Object);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            textWriterDestination.Send(logs);

            // Assert  
            string concatenatedStr = concatedLogTexts.ToString();
            textWriterMock.Verify(x => x.Write(concatenatedStr), Times.Once);
            textWriterMock.Verify(x => x.Flush(), Times.Once);
        }

        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(11)]
        [TestCase(15)]
        public void Send_When_Called_With_N_Logs_Resulting_In_M_TextWriter_Destinations_Calls_Each_TextWriter_Method_M_Times_With_Concatenated_Log_Texts(int N)
        {
            // Arrange 
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            LogModel[] logs = new LogModel[N];
            Mock<TextWriter>[] textWriterMocks = new Mock<TextWriter>[5];
            for (int j = 0; j < 5; j++)
                textWriterMocks[j] = new Mock<TextWriter>();
            StringBuilder[] concatedLogTexts = new StringBuilder[5];
            for (int j = 0; j < 5; j++)
                concatedLogTexts[j] = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Description = $"logText{i}" };
                concatedLogTexts[i % 5].AppendLine($"logText{i}");
            }
            textWriterProviderMock.Setup(x => x.GetTextWriter(It.IsAny<LogModel>())).Returns((LogModel x) => textWriterMocks[logs.ToList().IndexOf(x) % 5].Object);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            textWriterDestination.Send(logs);

            // Assert
            for (int j = 0; j < 5; j++)
            {
                string expectedStr = concatedLogTexts[j].ToString();
                textWriterMocks[j].Verify(x => x.Write(expectedStr), Times.Once);
                textWriterMocks[j].Verify(x => x.Flush(), Times.Once);
            }
        }

        #endregion

        #region Tests for SendAsync method

        [Test]
        public async Task SendAsync_When_Called_Generate_And_Send_Log_String_To_TextWriters_WriteAsync_Method()
        {
            // Arrange
            LogModel logModel = new LogModel();
            // ILogSerializer
            var logSerializerMock = new Mock<ILogSerializer>();
            string serialized = "serialized log string";
            logSerializerMock.Setup(x => x.Serialize(logModel)).Returns(serialized);
            // ITextWriterProvider
            var textWriterMock = new Mock<TextWriter>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            textWriterProviderMock.Setup(x => x.GetTextWriter(logModel)).Returns(textWriterMock.Object);
            // TextWriterDestination 
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            await textWriterDestination.SendAsync(new LogModel[] { logModel });

            // Assert
            textWriterMock.Verify(x => x.WriteAsync(serialized + Environment.NewLine), Times.Once);
            textWriterMock.Verify(x => x.FlushAsync(), Times.Once);
        }

        [Test]
        public void SendAsync_When_TextWriterProvider_Returns_Null_TextWriter_Throws_Exception()
        {
            // Arrange
            LogModel logModel = new LogModel();
            // ILogSerializer
            var logSerializerMock = new Mock<ILogSerializer>();
            string serialized = "serialized log string";
            logSerializerMock.Setup(x => x.Serialize(logModel)).Returns(serialized);
            // ITextWriterProvider 
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            textWriterProviderMock.Setup(x => x.GetTextWriter(logModel)).Returns(null as TextWriter);
            // TextWriterDestination 
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Assert
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                await textWriterDestination.SendAsync(new LogModel[] { logModel });
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_N_Different_LogModel_That_Corresponds_To_N_Different_TextWriters_Calls_Each_TextWriter_Method_One_Time(int N)
        {
            // Arrange 
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();

            var dict = new Dictionary<LogModel, Mock<TextWriter>>();
            for (int i = 0; i < N; i++)
                dict.Add(new LogModel() { Description = $"logText{i}" }, new Mock<TextWriter>());

            textWriterProviderMock.Setup(x => x.GetTextWriter(It.IsAny<LogModel>())).Returns((LogModel x) => dict[x].Object);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);

            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            await textWriterDestination.SendAsync(dict.Keys.ToArray());

            // Assert  
            foreach (LogModel logModel in dict.Keys)
            {
                dict[logModel].Verify(x => x.WriteAsync(logModel.Description + Environment.NewLine), Times.Once);
                dict[logModel].Verify(x => x.FlushAsync(), Times.Once);
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_N_Different_LogModel_Which_Correspond_To_The_Same_TextWriter_Calls_TextWriter_Method_Once_And_Passes_Concatenated_Log_Texts(int N)
        {
            // Arrange 
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            var textWriterMock = new Mock<TextWriter>();
            LogModel[] logs = new LogModel[N];
            StringBuilder concatedLogTexts = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Description = $"logText{i}" };
                concatedLogTexts.AppendLine($"logText{i}");
            }
            textWriterProviderMock.Setup(x => x.GetTextWriter(It.IsAny<LogModel>())).Returns(textWriterMock.Object);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            await textWriterDestination.SendAsync(logs);

            // Assert  
            string concatenatedStr = concatedLogTexts.ToString();
            textWriterMock.Verify(x => x.WriteAsync(concatenatedStr), Times.Once);
            textWriterMock.Verify(x => x.FlushAsync(), Times.Once);
        }

        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(11)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_N_Logs_Resulting_In_M_TextWriter_Destinations_Calls_Each_TextWriter_Method_M_Times_With_Concatenated_Log_Texts(int N)
        {
            // Arrange 
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            LogModel[] logs = new LogModel[N];
            Mock<TextWriter>[] textWriterMocks = new Mock<TextWriter>[5];
            for (int j = 0; j < 5; j++)
                textWriterMocks[j] = new Mock<TextWriter>();
            StringBuilder[] concatedLogTexts = new StringBuilder[5];
            for (int j = 0; j < 5; j++)
                concatedLogTexts[j] = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Description = $"logText{i}" };
                concatedLogTexts[i % 5].AppendLine($"logText{i}");
            }
            textWriterProviderMock.Setup(x => x.GetTextWriter(It.IsAny<LogModel>())).Returns((LogModel x) => textWriterMocks[logs.ToList().IndexOf(x) % 5].Object);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            await textWriterDestination.SendAsync(logs);

            // Assert
            for (int j = 0; j < 5; j++)
            {
                string expectedStr = concatedLogTexts[j].ToString();
                textWriterMocks[j].Verify(x => x.WriteAsync(expectedStr), Times.Once);
                textWriterMocks[j].Verify(x => x.FlushAsync(), Times.Once);
            }
        }

        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            var logSerializer = new Mock<ILogSerializer>();
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializer.Object, textWriterProviderMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert  
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await textWriterDestination.SendAsync(new LogModel[] { Samples.LogModels.Standard(LogType.Info) }, cancellationToken);
            });
            logSerializer.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            textWriterProviderMock.Verify(x => x.GetTextWriter(It.IsAny<LogModel>()), Times.Never);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Destination()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, textWriterProviderMock.Object);

            // Act
            var result = textWriterDestination.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Destinations.TextWriter.TextWriterDestination"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Serializer()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var logSerializerDescription = "logSerializerDescription";
            logSerializerMock.Setup(x => x.ToString()).Returns(logSerializerDescription);
            TextWriterDestination textWriterDestination = new TextWriterDestination(logSerializerMock.Object, new Mock<ITextWriterProvider>().Object);

            // Act
            var result = textWriterDestination.ToString();

            // Arrange
            logSerializerMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logSerializerDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_TextWriter_Provider()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var textWriterProviderMock = new Mock<ITextWriterProvider>();
            var textWriterProviderDescription = "textWriterProviderDescription";
            textWriterProviderMock.Setup(x => x.ToString()).Returns(textWriterProviderDescription);
            TextWriterDestination textWriterDestination = new TextWriterDestination(new Mock<ILogSerializer>().Object, textWriterProviderMock.Object);

            // Act
            var result = textWriterDestination.ToString();

            // Arrange
            textWriterProviderMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(textWriterProviderDescription));
        }

        #endregion
    }
}
