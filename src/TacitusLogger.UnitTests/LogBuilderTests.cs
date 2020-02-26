using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Logger_LogType_Description_When_Called_Sets_Dependencies()
        {
            // Arrange
            ILogger logger = new Mock<ILogger>().Object;
            LogType logType = LogType.Event;
            string description = "description1";

            // Act
            LogBuilder logBuilder = new LogBuilder(logger, logType, description);

            // Assert
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.NotNull(logBuilder.BuiltLog);
            Assert.AreEqual(logType, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
            Assert.IsNull(logBuilder.BuiltLog.Context);
            Assert.AreEqual(0, logBuilder.BuiltLog.Tags.Count);
            Assert.NotNull(logBuilder.BuiltLog.Items);
            Assert.AreEqual(0, logBuilder.BuiltLog.Items.Count);
        }
        [Test]
        public void Ctor_Taking_Logger_LogType_Description_When_Called_With_Null_Logger_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                LogBuilder logBuilder = new LogBuilder(null as ILogger, LogType.Event, "description1");
            });
        }
        [Test]
        public void Ctor_Taking_Logger_LogType_Description_When_Called_With_Null_Description_Sets_Null_Description()
        {
            // Act
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, null as string);

            // Assert
            Assert.NotNull(logBuilder.BuiltLog);
            Assert.IsNull(logBuilder.BuiltLog.Description);
        }

        #endregion

        #region Tests for Tagged method

        [Test]
        public void Tagged_When_Called_With_One_Tag_Sets_Log_Tags()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act 
            logBuilder.Tagged("tag1");

            // Assert 
            Assert.AreEqual(1, logBuilder.BuiltLog.Tags.Count);
            Assert.AreEqual("tag1", logBuilder.BuiltLog.Tags[0]); 
        }
        [Test]
        public void Tagged_When_Called_Returns_LogBuilder()
        {
            // Arrange
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act
            var returned = logBuilder.Tagged(null as string[]);

            // Assert
            Assert.AreEqual(logBuilder, returned);
        }
        [Test]
        public void Tagged_When_Called_Several_Times_Adds_Tags_Each_Time_To_Log_Tags()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act 
            logBuilder.Tagged("tag1", "tag2");
            logBuilder.Tagged("tag3", "tag4");

            // Assert 
            Assert.AreEqual(4, logBuilder.BuiltLog.Tags.Count);
            Assert.AreEqual("tag1", logBuilder.BuiltLog.Tags[0]);
            Assert.AreEqual("tag2", logBuilder.BuiltLog.Tags[1]);
            Assert.AreEqual("tag3", logBuilder.BuiltLog.Tags[2]);
            Assert.AreEqual("tag4", logBuilder.BuiltLog.Tags[3]);
        }
        [Test]
        public void Tagged_When_Called_With_Several_Tags_Sets_Log_Tags()
        {
            // Arrange  
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act 
            logBuilder.Tagged("tag1", "tag2", "tag3");

            // Assert 
            Assert.AreEqual(3, logBuilder.BuiltLog.Tags.Count);
            Assert.AreEqual("tag1", logBuilder.BuiltLog.Tags[0]);
            Assert.AreEqual("tag2", logBuilder.BuiltLog.Tags[1]);
            Assert.AreEqual("tag3", logBuilder.BuiltLog.Tags[2]);
        }
        [Test]
        public void Tagged_When_Called_With_Null_Tags_Array_Does_Not_Change_Tags_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act 
            logBuilder.Tagged(null as string[]);

            // Assert 
            Assert.NotNull(logBuilder.BuiltLog.Tags);
            Assert.AreEqual(0, logBuilder.BuiltLog.Tags.Count);
        }
        [Test]
        public void Tagged_When_Called_With_Several_Tags_Containing_Nulls_Does_Not_Add_Nulls_To_Tags_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act 
            logBuilder.Tagged("tag1", null, "tag3");

            // Assert 
            Assert.AreEqual(2, logBuilder.BuiltLog.Tags.Count);
            Assert.AreEqual("tag1", logBuilder.BuiltLog.Tags[0]);
            Assert.AreEqual("tag3", logBuilder.BuiltLog.Tags[1]);
        }
        [Test]
        public void Tagged_When_Called_With_Empty_Strings_Adds_Empty_Strings_To_Tags_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Critical, "description");

            // Act 
            logBuilder.Tagged("", "");

            // Assert 
            Assert.AreEqual(2, logBuilder.BuiltLog.Tags.Count);
            Assert.AreEqual("", logBuilder.BuiltLog.Tags[0]);
            Assert.AreEqual("", logBuilder.BuiltLog.Tags[1]);
        }

        #endregion

        #region Tests for From method overloads

        [Test]
        public void Ctor_Taking_String_Context_When_Called_Sets_Context_To_Provided()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.From("context1");

            // Assert
            Assert.AreEqual("context1", logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_String_Context_When_Called_With_Null_Context_Sets_Context_To_Null()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.From(null as string);

            // Assert
            Assert.AreEqual(null, logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_String_Context_When_Called_Several_Times_Reset_Context_Each_Time()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.From("context1");
            logBuilder.From("context2");

            // Assert
            Assert.AreEqual("context2", logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_String_Context_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.From("context1");

            // Assert
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }
        [Test]
        public void Ctor_Taking_Object_Context_When_Called_Sets_Context_To_Provided_Objects_Class_FullName()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var customType = new { a = "b" };

            // Act
            logBuilder.From(customType);

            // Assert
            Assert.AreEqual(customType.GetType().FullName, logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_Object_Context_When_Called_With_Null_Context_Sets_Context_To_Null()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.From(null as object);

            // Assert
            Assert.IsNull(logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_Object_Context_When_Called_Several_Times_Reset_Context_Each_Time()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var customType1 = new { a1 = "a1", b1 = "b1" };
            var customType2 = new { a2 = 2 };

            // Act
            logBuilder.From(customType1);
            logBuilder.From(customType2);

            // Assert
            Assert.AreEqual(customType2.GetType().FullName, logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_Object_Context_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.From(this);

            // Assert
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }
        [Test]
        public void Ctor_Taking_TContext_Type_Parameter_When_Called_Sets_Context_To_Type_Parameter_FullName()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.From<LogBuilderTests>();

            // Assert
            Assert.AreEqual(typeof(LogBuilderTests).FullName, logBuilder.BuiltLog.Context);
        }
        [Test]
        public void Ctor_Taking_TContext_Type_Parameter_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.From<LogBuilderTests>();

            // Assert
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }

        #endregion

        #region Tests for With method overloads

        [Test]
        public void With_Taking_LogItem_When_Called_Adds_Provided_LogItem_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            LogItem logItem = new LogItem("name", "value");

            // Act
            logBuilder.With(logItem);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(logItem, logBuilder.BuiltLog.Items[0]);
        }
        [Test]
        public void With_Taking_LogItem_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.With(new LogItem("name", "value"));

            // Assert
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }
        [Test]
        public void With_Taking_LogItem_When_Called_With_Null_LogItem_Does_Not_Adds_It()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.With(null as LogItem);

            // Assert
            Assert.AreEqual(0, logBuilder.BuiltLog.Items.Count);
        }
        [Test]
        public void With_Taking_Item_Name_And_Value_When_Called_Adds_New_Log_Item_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var itemValue = new { };

            // Act
            logBuilder.With("name1", itemValue);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual("name1", logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(itemValue, logBuilder.BuiltLog.Items[0].Value);
        }
        [Test]
        public void With_Taking_Item_Name_And_Value_When_Called_With_Null_Name_And_Value_Adds_New_Log_Item_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var itemValue = new { };

            // Act
            logBuilder.With(null as string, null as object);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Value);
        }
        [Test]
        public void With_Taking_Item_Name_And_Value_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.With("", new { });

            // Assert 
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }
        [Test]
        public void With_Taking_Item_Value_When_Called_Adds_New_Log_Item_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var itemValue = new { };

            // Act
            logBuilder.With(itemValue);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(itemValue.GetType().FullName, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(itemValue, logBuilder.BuiltLog.Items[0].Value);
        }
        [Test]
        public void With_Taking_Item_Value_When_Called_With_Null_Value_Adds_New_Log_Item_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.With(null as object);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Value);
        }
        [Test]
        public void With_Taking_Item_Value_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.With(new { });

            // Assert 
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }

        #endregion

        #region Tests for WithMany method overloads

        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_With_One_Value_Object_Adds_New_Log_Item_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var itemValue = new { };

            // Act
            logBuilder.WithMany(itemValue);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(itemValue.GetType().FullName, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(itemValue, logBuilder.BuiltLog.Items[0].Value);
        }
        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_With_Several_Value_Objects_Adds_New_Log_Items_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var itemValue1 = new { };
            var itemValue2 = new { };

            // Act
            logBuilder.WithMany(itemValue1, itemValue2);

            // Assert
            Assert.AreEqual(2, logBuilder.BuiltLog.Items.Count);
            // Item1
            Assert.AreEqual(itemValue1.GetType().FullName, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(itemValue1, logBuilder.BuiltLog.Items[0].Value);
            // Item2
            Assert.AreEqual(itemValue2.GetType().FullName, logBuilder.BuiltLog.Items[1].Name);
            Assert.AreEqual(itemValue2, logBuilder.BuiltLog.Items[1].Value);
        }
        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_With_One_Null_Value_Object_Adds_New_Log_Item_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.WithMany(null as object);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Value);
        }
        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_With_Several_Null_Value_Objects_Adds_New_Log_Items_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.WithMany(null as object, null as object);

            // Assert
            Assert.AreEqual(2, logBuilder.BuiltLog.Items.Count);
            // First item
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Value);
            // Second item
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[1].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[1].Value);
        }
        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_With_Several_Value_Objects_Containing_Nulls_Adds_New_Log_Items_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var itemValue2 = new { };
            // Act
            logBuilder.WithMany(null as object, itemValue2, null as object);

            // Assert
            Assert.AreEqual(3, logBuilder.BuiltLog.Items.Count);
            // First item
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[0].Value);
            // Second item
            Assert.AreEqual(itemValue2.GetType().FullName, logBuilder.BuiltLog.Items[1].Name);
            Assert.AreEqual(itemValue2, logBuilder.BuiltLog.Items[1].Value);
            // Third item
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[2].Name);
            Assert.AreEqual(null, logBuilder.BuiltLog.Items[2].Value);
        }
        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_With_Null_Value_Objects_Array_Does_Not_Change_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.WithMany(null as object[]);

            // Assert
            Assert.AreEqual(0, logBuilder.BuiltLog.Items.Count);
        }
        [Test]
        public void WithMany_Taking_Item_Value_Objects_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.WithMany(new { }, new { }, new { });

            // Assert 
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_With_One_Item_Adds_It_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var item = new LogItem("name1", "value1");

            // Act
            logBuilder.WithMany(item);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(item, logBuilder.BuiltLog.Items[0]);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_With_Several_Items_Adds_Them_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var item1 = new LogItem("name1", "value1");
            var item2 = new LogItem("name2", "value2");

            // Act
            logBuilder.WithMany(item1, item2);

            // Assert
            Assert.AreEqual(2, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(item1, logBuilder.BuiltLog.Items[0]);
            Assert.AreEqual(item2, logBuilder.BuiltLog.Items[1]);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_With_One_Null_Item_Does_Not_Add_It_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.WithMany(null as LogItem);

            // Assert
            Assert.AreEqual(0, logBuilder.BuiltLog.Items.Count);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_With_Several_Items_Does_Not_Add_Them_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.WithMany(null as LogItem, null as LogItem);

            // Assert
            Assert.AreEqual(0, logBuilder.BuiltLog.Items.Count);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_With_Several_Value_Objects_Containing_Nulls_Does_Not_Add_Nulls_To_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");
            var item = new LogItem("name2", "value2");
            // Act
            logBuilder.WithMany(null as LogItem, item, null as LogItem);

            // Assert
            Assert.AreEqual(1, logBuilder.BuiltLog.Items.Count);
            Assert.AreEqual(item, logBuilder.BuiltLog.Items[0]);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_With_Null_Items_Array_Does_Not_Change_Items_List()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            logBuilder.WithMany(null as LogItem[]);

            // Assert
            Assert.AreEqual(0, logBuilder.BuiltLog.Items.Count);
        }
        [Test]
        public void WithMany_Taking_Log_Items_When_Called_Returns_LogBuilder()
        {
            // Arrange 
            LogBuilder logBuilder = new LogBuilder(new Mock<ILogger>().Object, LogType.Event, "description1");

            // Act
            var logBuilderReturned = logBuilder.WithMany(new LogItem("name1", "value1"), new LogItem("name2", "value2"));

            // Assert 
            Assert.AreEqual(logBuilder, logBuilderReturned);
        }

        #endregion

        #region Tests for Log method

        [Test]
        public void Log_When_Called_Calls_Loggers_Log_Method_Passing_Self_To_It_And_Returning_Its_Result()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<Log>())).Returns("Result from Log method");
            LogBuilder logBuilder = new LogBuilder(loggerMock.Object, LogType.Event, "description1");

            // Act
            string returned = logBuilder.Log();

            // Assert
            loggerMock.Verify(x => x.Log(logBuilder.BuiltLog), Times.Once);
            loggerMock.Verify(x => x.LogAsync(logBuilder.BuiltLog, It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual("Result from Log method", returned);
        }

        #endregion

        #region Tests for LogAsync method

        [Test]
        public async Task LogAsync_When_Called_Calls_Loggers_LogAsync_Method_Passing_Self_To_It_And_Returning_Its_Result()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.LogAsync(It.IsAny<Log>(), It.IsAny<CancellationToken>())).ReturnsAsync("Result from Log method");
            LogBuilder logBuilder = new LogBuilder(loggerMock.Object, LogType.Event, "description1");
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            string returned = await logBuilder.LogAsync(cancellationToken);

            // Assert
            loggerMock.Verify(x => x.LogAsync(logBuilder.BuiltLog, cancellationToken), Times.Once);
            loggerMock.Verify(x => x.Log(logBuilder.BuiltLog), Times.Never);
            Assert.AreEqual("Result from Log method", returned);
        }

        #endregion
    }
}
