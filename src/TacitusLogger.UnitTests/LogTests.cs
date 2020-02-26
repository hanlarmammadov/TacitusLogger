using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogTests
    {
        private void AssertThatPropertiesAreInitialized(Log log, LogType expectedLogType, string expectedDesciption)
        {
            Assert.AreEqual(expectedLogType, log.Type);
            Assert.AreEqual(expectedDesciption, log.Description);
            Assert.Null(log.Context);
            Assert.AreEqual(0, log.Tags.Count);
            Assert.NotNull(log.Items);
            Assert.AreEqual(0, log.Items.Count);
        }

        #region Ctor

        public void Ctor_Taking_Log_Type_Context_Tags_Description_And_Log_Items_When_Called_Init_Properties()
        {
            // Arrange 
            LogType logType = LogType.Warning;
            string context = "context1";
            string description = "description1";
            List<LogItem> logItems = new List<LogItem>() { new LogItem("", "") };
            List<string> tags = new List<string>();

            // Act 
            Log log = new Log(context, logType, description, tags, logItems);

            // Assert  
            Assert.AreEqual(logType, log.Type);
            Assert.AreEqual(context, log.Context);
            Assert.AreEqual(tags, log.Tags);
            Assert.AreEqual(description, log.Description);
            Assert.AreEqual(logItems, log.Items);
        }

        public void Ctor_Taking_Log_Type_Context_Tags_Description_And_Log_Items_When_Called_With_All_Nulls_Init_Properties()
        {
            // Act 
            Log log = new Log(type: default(LogType), context: null, tags: null, description: null, logItems: null);

            // Assert  
            Assert.AreEqual(default(LogType), log.Type);
            Assert.IsNull(log.Context);
            Assert.NotNull(log.Tags);
            Assert.AreEqual(0, log.Tags.Count);
            Assert.IsNull(log.Description);
            Assert.NotNull(log.Items);
            Assert.AreEqual(0, log.Items.Count);
        }


        [TestCase(LogType.Success)]
        [TestCase(LogType.Info)]
        [TestCase(LogType.Event)]
        [TestCase(LogType.Warning)]
        [TestCase(LogType.Failure)]
        [TestCase(LogType.Error)]
        [TestCase(LogType.Critical)]
        public void Ctor_Taking_Log_Type_And_Description_When_Called_Init_Properties(LogType logType)
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = new Log(logType, description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: logType, expectedDesciption: description);
        }

        [Test]
        public void Ctor_Default_When_Called_Init_Items()
        {
            // Act 
            Log log = new Log();

            // Assert 
            Assert.AreEqual(default(LogType), log.Type);
            Assert.IsNull(log.Context);
            Assert.AreEqual(0, log.Tags.Count);
            Assert.IsNull(log.Description);
            Assert.NotNull(log.Items);
            Assert.AreEqual(0, log.Items.Count);
        }

        #endregion

        #region Tests for Entry points 

        [Test]
        public void Success_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Success(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Success, expectedDesciption: description);
        }

        [Test]
        public void Info_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Info(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Info, expectedDesciption: description);
        }

        [Test]
        public void Event_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Event(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Event, expectedDesciption: description);
        }

        [Test]
        public void Warning_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Warning(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Warning, expectedDesciption: description);
        }

        [Test]
        public void Failure_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Failure(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Failure, expectedDesciption: description);
        }

        [Test]
        public void Error_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Error(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Error, expectedDesciption: description);
        }

        [Test]
        public void Critical_Taking_Description_When_Called_Sets_Log_Type_And_Description()
        {
            // Arrange 
            string description = "description1";

            // Act 
            Log log = Log.Critical(description);

            // Assert 
            AssertThatPropertiesAreInitialized(log, expectedLogType: LogType.Critical, expectedDesciption: description);
        }

        #endregion

        #region Tests for Tagged method

        [Test]
        public void Tagged_When_Called_With_One_Tag_Sets_Log_Tags()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.Tagged("tag1");

            // Assert 
            Assert.AreEqual(1, log.Tags.Count);
            Assert.AreEqual("tag1", log.Tags[0]);
        }

        [Test]
        public void Tagged_When_Called_Returns_Log()
        {
            // Arrange
            Log log = new Log();

            // Act
            var returned = log.Tagged(null as string[]);

            // Assert
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void Tagged_When_Called_Several_Times_Adds_Tags_Each_Time_To_Log_Tags()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.Tagged("tag1", "tag2");
            log.Tagged("tag3", "tag4");

            // Assert 
            Assert.AreEqual(4, log.Tags.Count);
            Assert.AreEqual("tag1", log.Tags[0]);
            Assert.AreEqual("tag2", log.Tags[1]);
            Assert.AreEqual("tag3", log.Tags[2]);
            Assert.AreEqual("tag4", log.Tags[3]);
        }

        [Test]
        public void Tagged_When_Called_With_Several_Tags_Sets_Log_Tags()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            var returned = log.Tagged("tag1", "tag2", "tag3");

            // Assert 
            Assert.AreEqual(3, log.Tags.Count);
            Assert.AreEqual("tag1", log.Tags[0]);
            Assert.AreEqual("tag2", log.Tags[1]);
            Assert.AreEqual("tag3", log.Tags[2]);
        }

        [Test]
        public void Tagged_When_Called_With_Null_Tags_Array_Does_Not_Change_Tags_List()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            var returned = log.Tagged(null as string[]);

            // Assert 
            Assert.NotNull(log.Tags);
            Assert.AreEqual(0, log.Tags.Count);
        }

        [Test]
        public void Tagged_When_Called_With_Several_Tags_Containing_Nulls_Does_Not_Add_Nulls_To_Tags_List()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            var returned = log.Tagged("tag1", null, "tag3");

            // Assert 
            Assert.AreEqual(2, log.Tags.Count);
            Assert.AreEqual("tag1", log.Tags[0]);
            Assert.AreEqual("tag3", log.Tags[1]);
        }
        [Test]
        public void Tagged_When_Called_With_Empty_Strings_Adds_Empty_Strings_To_Tags_List()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            var returned = log.Tagged("", "");

            // Assert 
            Assert.AreEqual(2, log.Tags.Count);
            Assert.AreEqual("", log.Tags[0]);
            Assert.AreEqual("", log.Tags[1]);
        }

        #endregion

        #region Tests for From method overloads 

        [Test]
        public void From_Taking_String_Context_When_Called_Sets_Context_To_Provided_String_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();
            string context = "context1";

            // Act 
            Log returned = log.From(context);

            // Assert 
            Assert.AreEqual(context, log.Context);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void From_Taking_String_Context_When_Called_With_Null_String_Sets_Context_To_Provided_String()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.From(null as string);

            // Assert 
            Assert.IsNull(log.Context);
        }

        [Test]
        public void From_Taking_Generic_Type_When_Called_Sets_Context_To_Full_Name_Of_Provided_Type_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            Log returned = log.From<LogTests>();

            // Assert 
            Assert.AreEqual("TacitusLogger.UnitTests.LogTests", log.Context);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void From_Taking_Object_When_Called_Sets_Context_To_Full_Name_Of_Provided_Object_Type_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            Log returned = log.From(this);

            // Assert 
            Assert.AreEqual("TacitusLogger.UnitTests.LogTests", log.Context);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void From_Taking_Object_When_Called_With_Null_Object_Does_Not_Change_Context()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.From(null as object);

            // Assert 
            Assert.IsNull(log.Context);
        }

        #endregion

        #region Tests for With method overloads 

        [Test]
        public void With_Taking_Item_Name_And_Item_When_Called_Adds_That_Object_To_Items_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();
            string itemName = "item name";
            Object item = new { };

            // Act 
            Log returned = log.With(itemName, item);

            // Assert 
            Assert.AreEqual(1, log.Items.Count);
            Assert.AreEqual(itemName, log.Items[0].Name);
            Assert.AreEqual(item, log.Items[0].Value);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void With_Taking_Item_Name_And_Item_When_Called_With_Nulls_Adds_That_Object_To_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.With(null as string, null as object);

            // Assert 
            Assert.AreEqual(1, log.Items.Count);
            Assert.IsNull(log.Items[0].Name);
            Assert.IsNull(log.Items[0].Value);
        }

        [Test]
        public void With_Taking_Object_When_Called_Adds_That_Object_To_Items_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();
            Object item = new { };

            // Act 
            Log returned = log.With(item);

            // Assert 
            Assert.AreEqual(1, log.Items.Count);
            Assert.AreEqual(item, log.Items[0].Value);
            Assert.AreEqual(item.GetType().FullName, log.Items[0].Name);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void With_Taking_Object_When_Called_With_Null_Object_Adds_That_Object_To_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.With(null as object);

            // Assert 
            Assert.AreEqual(1, log.Items.Count);
            Assert.IsNull(log.Items[0].Name);
            Assert.IsNull(log.Items[0].Value);
        }

        [Test]
        public void With_Taking_LogItem_When_Called_Adds_That_Object_To_Items_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();
            LogItem item = new LogItem("name", "value");

            // Act 
            Log returned = log.With(item);

            // Assert 
            Assert.AreEqual(1, log.Items.Count);
            Assert.AreEqual(item, log.Items[0]);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void With_Taking_LogItem_When_Called_With_Null_LogItem_Adds_That_Object_To_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.With(null as LogItem);

            // Assert 
            Assert.AreEqual(0, log.Items.Count);
        }

        [Test]
        public void WithMany_Taking_Objects_Array_When_Called_Adds_Objects_To_Items_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();
            Object item1 = new { };
            Object item2 = new { };
            Object item3 = new { };

            // Act 
            Log returned = log.WithMany(item1, item2, item3);

            // Assert 
            Assert.AreEqual(3, log.Items.Count);
            Assert.AreEqual(item1, log.Items[0].Value);
            Assert.AreEqual(item1.GetType().FullName, log.Items[0].Name);
            Assert.AreEqual(item2, log.Items[1].Value);
            Assert.AreEqual(item2.GetType().FullName, log.Items[1].Name);
            Assert.AreEqual(item3, log.Items[2].Value);
            Assert.AreEqual(item3.GetType().FullName, log.Items[2].Name);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void WithMany_Taking_Objects_Array_When_Called_With_Array_That_Contains_Nulls_Adds_Objects_To_Items()
        {
            // Arrange 
            Log log = new Log();
            Object item1 = null;
            Object item2 = null;

            // Act 
            log.WithMany(item1, item2);

            // Assert 
            Assert.AreEqual(2, log.Items.Count);
            Assert.IsNull(log.Items[0].Value);
            Assert.IsNull(log.Items[0].Name);
            Assert.IsNull(log.Items[1].Value);
            Assert.IsNull(log.Items[1].Name);
        }

        [Test]
        public void WithMany_Taking_Objects_Array_When_Called_With_Empty_Array_Does_Not_Change_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.WithMany(new Object[0]);

            // Assert 
            Assert.AreEqual(0, log.Items.Count);
        }

        [Test]
        public void WithMany_Taking_Objects_Array_When_Called_With_Null_Array_Does_Not_Change_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.WithMany(null as Object[]);

            // Assert 
            Assert.AreEqual(0, log.Items.Count);
        }

        [Test]
        public void WithMany_Taking_LogItems_Array_When_Called_Adds_Objects_To_Items_And_Returns_Self()
        {
            // Arrange 
            Log log = new Log();
            LogItem item1 = new LogItem("name", "value");
            LogItem item2 = new LogItem("name", "value");
            LogItem item3 = new LogItem("name", "value");

            // Act 
            Log returned = log.WithMany(item1, item2, item3);

            // Assert 
            Assert.AreEqual(3, log.Items.Count);
            Assert.AreEqual(item1, log.Items[0]);
            Assert.AreEqual(item2, log.Items[1]);
            Assert.AreEqual(item3, log.Items[2]);
            Assert.AreEqual(log, returned);
        }

        [Test]
        public void WithMany_Taking_LogItems_Array_When_Called_With_Array_That_Contains_Nulls_Does_Not_Adds_Nulls_To_Items()
        {
            // Arrange 
            Log log = new Log();
            LogItem item1 = null;
            LogItem item2 = null;

            // Act 
            log.WithMany(item1, item2);

            // Assert 
            Assert.AreEqual(0, log.Items.Count);
        }

        [Test]
        public void WithMany_Taking_LogItems_Array_When_Called_With_Empty_Array_Does_Not_Change_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.WithMany(new LogItem[0]);

            // Assert 
            Assert.AreEqual(0, log.Items.Count);
        }

        [Test]
        public void WithMany_Taking_LogItems_Array_When_Called_With_Null_Array_Does_Not_Change_Items()
        {
            // Arrange 
            Log log = new Log();

            // Act 
            log.WithMany(null as LogItem[]);

            // Assert 
            Assert.AreEqual(0, log.Items.Count);
        }

        #endregion

        #region Tests for To and ToAsync methods 

        [Test]
        public void To_Taking_Logger_When_Called_Calls_Logger_Log_Method_Passing_Log_Object()
        {
            // Arrange 
            var loggerMock = new Mock<ILogger>();
            Log log = new Log();

            // Act 
            log.To(loggerMock.Object);

            // Assert 
            loggerMock.Verify(x => x.Log(log), Times.Once);
        }

        [Test]
        public void To_Taking_Logger_When_Called_With_Null_Logger_Throws_Argument_Null_Exception()
        {
            // Arrange  
            Log log = new Log();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act 
                log.To(null as ILogger);
            });
        }

        [Test]
        public async Task ToAsync_Taking_Logger_And_Cancellation_Token_When_Called_Calls_Logger_LogAsync_Method_Passing_Log_Object()
        {
            // Arrange 
            var loggerMock = new Mock<ILogger>();
            Log log = new Log();
            CancellationToken cancellationToken = new CancellationToken();

            // Act 
            await log.ToAsync(loggerMock.Object, cancellationToken);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(log, cancellationToken), Times.Once);
        }

        [Test]
        public async Task ToAsync_Taking_Logger_And_Cancellation_Token_When_Called_Without_Cancellation_Token_Uses_Default()
        {
            // Arrange 
            var loggerMock = new Mock<ILogger>();
            Log log = new Log();

            // Act 
            await log.ToAsync(loggerMock.Object);

            // Assert 
            loggerMock.Verify(x => x.LogAsync(log, default(CancellationToken)), Times.Once);
        }

        [Test]
        public void ToAsync_Taking_Logger_When_Called_With_Null_Logger_Throws_Argument_Null_Exception()
        {
            // Arrange  
            Log log = new Log();

            Assert.CatchAsync<ArgumentNullException>(async () =>
            {
                // Act 
                await log.ToAsync(null as ILogger);
            });
        }

        #endregion
    }
}
