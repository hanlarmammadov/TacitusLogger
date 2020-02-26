using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogModelTests
    {
        #region tests for default ctor

        [Test]
        public void Ctor_Default_When_Called_All_LogModel_Properties_Have_Default_Values()
        {
            // Act
            LogModel logModel = new LogModel();

            // Assert  
            Assert.AreEqual(default(string), logModel.LogId);
            Assert.AreEqual(default(string), logModel.Context);
            Assert.AreEqual(default(string[]), logModel.Tags);
            Assert.AreEqual(default(string), logModel.Source);
            Assert.AreEqual(default(LogType), logModel.LogType);
            Assert.AreEqual(default(string), logModel.Description);
            Assert.AreEqual(default(List<LogItem>), logModel.LogItems);
            Assert.AreEqual(default(DateTime), logModel.LogDate);
        }

        #endregion

        #region LogType tests

        [Test]
        public void LogType_Should_Have_7_Values()
        {
            Assert.AreEqual(7, Enum.GetValues(typeof(LogType)).Length);
        }

        [Test]
        public void LogType_Underlying_Values_Should_Be_Consistent()
        {
            Assert.AreEqual(10, (int)(LogType.Info));
            Assert.AreEqual(11, (int)(LogType.Success));
            Assert.AreEqual(12, (int)(LogType.Event));
            Assert.AreEqual(20, (int)(LogType.Warning));
            Assert.AreEqual(30, (int)(LogType.Error));
            Assert.AreEqual(31, (int)(LogType.Failure));
            Assert.AreEqual(32, (int)(LogType.Critical));
        }

        [Test]
        public void LogType_Names_Should_Be_Consistent()
        {
            var enumType = typeof(LogType);
            Assert.AreEqual("Info", Enum.GetName(enumType, LogType.Info));
            Assert.AreEqual("Success", Enum.GetName(enumType, LogType.Success));
            Assert.AreEqual("Event", Enum.GetName(enumType, LogType.Event));
            Assert.AreEqual("Warning", Enum.GetName(enumType, LogType.Warning));
            Assert.AreEqual("Failure", Enum.GetName(enumType, LogType.Failure));
            Assert.AreEqual("Error", Enum.GetName(enumType, LogType.Error));
            Assert.AreEqual("Critical", Enum.GetName(enumType, LogType.Critical));
        }

        #endregion

        #region Tests for LogTypeName property

        [TestCase(LogType.Info, "Info")]
        [TestCase(LogType.Success, "Success")]
        [TestCase(LogType.Event, "Event")]
        [TestCase(LogType.Warning, "Warning")]
        [TestCase(LogType.Failure, "Failure")]
        [TestCase(LogType.Error, "Error")]
        [TestCase(LogType.Critical, "Critical")]
        public void LogTypeName_Returns_Correct_Log_Type_Name(LogType logType, string expectedLogTypeName)
        {
            // Arrange
            LogModel logModel = new LogModel()
            {
                LogType = logType
            };

            // Assert
            Assert.AreEqual(expectedLogTypeName, logModel.LogTypeName);
        }

        #endregion

        #region Tests for IsXXX Properties

        [Test]
        public void IsInfo_Returns_True_If_Log_Type_Is_Info()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Info };

            // Assert
            Assert.IsTrue(logModel.IsInfo);
        }
        [Test]
        public void IsSuccess_Returns_True_If_Log_Type_Is_Success()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Success };

            // Assert
            Assert.IsTrue(logModel.IsSuccess);
        }
        [Test]
        public void IsEvent_Returns_True_If_Log_Type_Is_Event()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Event };

            // Assert
            Assert.IsTrue(logModel.IsEvent);
        }
        [Test]
        public void IsWarning_Returns_True_If_Log_Type_Is_Warning()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Warning };

            // Assert
            Assert.IsTrue(logModel.IsWarning);
        }
        [Test]
        public void IsFailure_Returns_True_If_Log_Type_Is_Failure()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Failure };

            // Assert
            Assert.IsTrue(logModel.IsFailure);
        }
        [Test]
        public void IsError_Returns_True_If_Log_Type_Is_Error()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Error };

            // Assert
            Assert.IsTrue(logModel.IsError);
        }
        [Test]
        public void IsCritical_Returns_True_If_Log_Type_Is_Critical()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Critical };

            // Assert
            Assert.IsTrue(logModel.IsCritical);
        }

        #endregion

        #region Tests for HasTags property

        [Test]
        public void HasTags_When_Called_Given_That_Tags_Array_Is_Not_Empty_Returns_True()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = new string[1] };

            // Assert
            Assert.IsTrue(logModel.HasTags);
        }
        [Test]
        public void HasTags_When_Called_Given_That_Tags_Array_Is_Empty_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = new string[0] };

            // Assert
            Assert.IsFalse(logModel.HasTags);
        }
        [Test]
        public void HasTags_When_Called_Given_That_Tags_Array_Is_Null_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = null };

            // Assert
            Assert.IsFalse(logModel.HasTags);
        }

        #endregion

        #region Tests for HasItems property

        [Test]
        public void HasItems_When_Called_Given_That_Items_Array_Is_Not_Empty_Returns_True()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = new LogItem[] { new LogItem("", "") } };

            // Assert
            Assert.IsTrue(logModel.HasItems);
        }
        [Test]
        public void HasItems_When_Called_Given_That_Items_Array_Is_Empty_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = new LogItem[0] };

            // Assert
            Assert.IsFalse(logModel.HasItems);
        }
        [Test]
        public void HasItems_When_Called_Given_That_Items_Array_Is_Null_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = null };

            // Assert
            Assert.IsFalse(logModel.HasItems);
        }

        #endregion

        #region Tests for HasTag method

        [Test]
        public void HasTag_When_Called_Given_That_Tags_Array_Is_Null_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = null };

            // Act
            var res = logModel.HasTag("tag1");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTag_When_Called_Given_That_Tags_Array_Is_Empty_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = new string[0] };

            // Act
            var res = logModel.HasTag("tag1");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTag_When_Called_Given_That_Tags_Array_Does_Not_Contain_Specified_Tag_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = new string[2] { "tag2", "tag3" } };

            // Act
            var res = logModel.HasTag("tag1");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTag_When_Called_Given_That_Tags_Array_Contains_Specified_Tag_Returns_True()
        {
            // Arrange
            LogModel logModel = new LogModel() { Tags = new string[2] { "tag1", "tag2" } };

            // Act
            var res = logModel.HasTag("tag1");

            // Assert
            Assert.IsTrue(res);
        }

        #endregion

        #region Tests for HasItem method

        [Test]
        public void HasItem_When_Called_Given_That_Items_Array_Is_Null_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = null };

            // Act
            var res = logModel.HasItem("item1");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasItem_When_Called_Given_That_Items_Array_Is_Empty_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = new LogItem[0] };

            // Act
            var res = logModel.HasItem("item1");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasItem_When_Called_Given_That_Items_Array_Does_Not_Contain_Item_With_Specified_Name_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = new LogItem[] { new LogItem("item2", ""), new LogItem("item3", "") } };

            // Act
            var res = logModel.HasItem("item1");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasItem_When_Called_Given_That_Items_Array_Contain_Item_With_Specified_Name_Returns_True()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogItems = new LogItem[] { new LogItem("item1", ""), new LogItem("item2", "") } };

            // Act
            var res = logModel.HasItem("item1");

            // Assert
            Assert.IsTrue(res);
        }

        #endregion

        #region Tests for LogTypeIsIn method

        [Test]
        public void LogTypeIsIn_When_Called_With_Null_Log_Type_Array_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Error };

            // Act
            var res = logModel.LogTypeIsIn(null as LogType[]);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void LogTypeIsIn_When_Called_With_Empty_Log_Type_Array_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Error };

            // Act
            var res = logModel.LogTypeIsIn(new LogType[0]);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void LogTypeIsIn_When_Called_With_Log_Type_Array_That_Does_Not_Contain_Actual_Log_Type_Returns_False()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Info };

            // Act
            var res = logModel.LogTypeIsIn(LogType.Error, LogType.Critical, LogType.Failure);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void LogTypeIsIn_When_Called_With_Log_Type_Array_That_Contains_Actual_Log_Type_Returns_True()
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = LogType.Warning };

            // Act
            var res = logModel.LogTypeIsIn(LogType.Info, LogType.Warning, LogType.Success);

            // Assert
            Assert.True(res);
        }
        [TestCase(LogType.Critical, false)]
        [TestCase(LogType.Error, true)]
        [TestCase(LogType.Event, false)]
        [TestCase(LogType.Failure, false)]
        [TestCase(LogType.Info, true)]
        [TestCase(LogType.Success, false)]
        [TestCase(LogType.Warning, true)]
        public void LogTypeIsIn_When_Called_With_Log_Type_Array_Returns_Bool_Depending_On_Contains_Or_Not(LogType actualLogType, bool expectedResult)
        {
            // Arrange
            LogModel logModel = new LogModel() { LogType = actualLogType };

            // Act
            var actualResult = logModel.LogTypeIsIn(LogType.Info, LogType.Error, LogType.Warning);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        #endregion

        #region Tests for HasTagsAll method

        [Test]
        public void HasTagsAll_When_Called_Given_That_Actual_Tags_Array_Is_Null_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = null };

            // Act
            bool res = logModel.HasTagsAll("tag");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Actual_Tags_Array_Is_Empty_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[0] };

            // Act
            bool res = logModel.HasTagsAll("tag");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Provided_Tags_Array_Is_Null_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[2] };

            // Act
            bool res = logModel.HasTagsAll(null as string[]);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Provided_Tags_Array_Is_Empty_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[2] };

            // Act
            bool res = logModel.HasTagsAll(new string[0]);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Actual_Tags_Array_Contains_None_Of_Provided_Tags_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2" } };

            // Act
            bool res = logModel.HasTagsAll("tag3", "tag4");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Actual_Tags_Array_Contains_Some_Of_Provided_Tags_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };

            // Act
            bool res = logModel.HasTagsAll("tag3", "tag4");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Actual_Tags_Array_Contains_All_Of_Provided_Tags_Returns_True()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };

            // Act
            bool res = logModel.HasTagsAll("tag2", "tag1");

            // Assert
            Assert.IsTrue(res);
        }
        [Test]
        public void HasTagsAll_When_Called_Given_That_Actual_Tags_Array_Contains_Nulls_Null_Is_Considered_A_Valid_Tag()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { null, "tag2", "tag3" } };

            // Act
            bool res = logModel.HasTagsAll("tag2", null);

            // Assert
            Assert.IsTrue(res);
        }

        #endregion

        #region Tests for HasTagsAny method

        [Test]
        public void HasTagsAny_When_Called_Given_That_Actual_Tags_Array_Is_Null_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = null };

            // Act
            bool res = logModel.HasTagsAny("tag");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Actual_Tags_Array_Is_Empty_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[0] };

            // Act
            bool res = logModel.HasTagsAny("tag");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Provided_Tags_Array_Is_Null_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[2] };

            // Act
            bool res = logModel.HasTagsAny(null as string[]);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Provided_Tags_Array_Is_Empty_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[2] };

            // Act
            bool res = logModel.HasTagsAny(new string[0]);

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Actual_Tags_Array_Contains_None_Of_Provided_Tags_Returns_False()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2" } };

            // Act
            bool res = logModel.HasTagsAny("tag3", "tag4");

            // Assert
            Assert.IsFalse(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Actual_Tags_Array_Contains_Some_Of_Provided_Tags_Returns_True()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };

            // Act
            bool res = logModel.HasTagsAny("tag3", "tag4");

            // Assert
            Assert.IsTrue(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Actual_Tags_Array_Contains_All_Of_Provided_Tags_Returns_True()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };

            // Act
            bool res = logModel.HasTagsAny("tag2", "tag1");

            // Assert
            Assert.IsTrue(res);
        }
        [Test]
        public void HasTagsAny_When_Called_Given_That_Actual_Tags_Array_Contains_Nulls_Null_Is_Considered_A_Valid_Tag()
        {
            // Arrage
            LogModel logModel = new LogModel() { Tags = new string[] { null, "tag2", "tag3" } };

            // Act
            bool res = logModel.HasTagsAny("tag4", null);

            // Assert
            Assert.IsTrue(res);
        }

        #endregion
    }
}
