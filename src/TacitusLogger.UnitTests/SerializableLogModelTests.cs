using NUnit.Framework;
using System;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class SerializableLogModelTests
    {
        [Test]
        public void Ctor_Taking_LogModel_When_Called_Sets_Fields()
        {
            // Arrange
            LogModel logModel = new LogModel()
            {
                Context = "Context1",
                Description = "Description1",
                LogId = "LogId1",
                Source = "Source1",
                LogType = LogType.Failure,
                Tags = new string[] { "tag1", "tag2", "tag3" },
                LogItems = new LogItem[] { new LogItem("item1", new { }), new LogItem("item2", new { }) },
                LogDate = DateTime.Now
            };

            // Act
            SerializableLogModel serializableLogModel = new SerializableLogModel(logModel);

            // Assert
            Assert.AreEqual(logModel.Context, serializableLogModel.Context);
            Assert.AreEqual(logModel.Description, serializableLogModel.Description);
            Assert.AreEqual(logModel.LogId, serializableLogModel.LogId);
            Assert.AreEqual(logModel.Source, serializableLogModel.Source);
            Assert.AreEqual(logModel.LogType, serializableLogModel.LogType);
            Assert.AreEqual(logModel.Tags, serializableLogModel.Tags);
            Assert.AreEqual(logModel.LogItems, serializableLogModel.LogItems);
            Assert.AreEqual(logModel.LogDate, serializableLogModel.LogDate);
        }
        [Test]
        public void Ctor_Taking_LogModel_When_Called_With_Null_Log_Model_Sets_Defaults()
        {
            // Act
            SerializableLogModel serializableLogModel = new SerializableLogModel(null as LogModel);

            // Assert
            Assert.IsNull(serializableLogModel.Context);
            Assert.IsNull(serializableLogModel.Description);
            Assert.IsNull(serializableLogModel.LogId);
            Assert.IsNull(serializableLogModel.Source);
            Assert.AreEqual(default(LogType), serializableLogModel.LogType);
            Assert.IsNull(serializableLogModel.Tags);
            Assert.IsNull(serializableLogModel.LogItems);
            Assert.AreEqual(default(DateTime), serializableLogModel.LogDate);
        }
    }
}
