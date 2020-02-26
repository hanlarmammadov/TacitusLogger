using NUnit.Framework;
using System;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogItemTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Init_Properties()
        {
            // Arrange
            var obj = new { };

            // Act
            LogItem logItem = new LogItem("name1", obj);

            // Assert
            Assert.AreEqual("name1", logItem.Name);
            Assert.AreEqual(obj, logItem.Value);
        }
        [Test]
        public void Ctor_When_Called_With_Nulls_Init_Properties()
        {
            // Act
            LogItem logItem = new LogItem(null as string, null as object);

            // Assert
            Assert.IsNull(logItem.Name);
            Assert.IsNull(logItem.Value);
        }

        #endregion

        #region Tests for FromObj method overloads

        [Test]
        public void FromObj_Taking_Value_When_Called_Creates_And_Returns_LogItem_With_Provided_Value()
        {
            // Arrange
            var obj = new { };

            // Act
            LogItem logItem = LogItem.FromObj(obj);

            // Assert
            Assert.AreEqual("Log item", logItem.Name);
            Assert.AreEqual(obj, logItem.Value);
        }
        [Test]
        public void FromObj_Taking_Value_When_Called_With_Null_Value_Creates_And_Returns_LogItem_With_Provided_Value()
        {
            // Act
            LogItem logItem = LogItem.FromObj(null as object);

            // Assert
            Assert.AreEqual("Log item", logItem.Name);
            Assert.AreEqual(null, logItem.Value);
        }
        [Test]
        public void FromObj_Taking_Name_And_Value_When_Called_Creates_And_Returns_LogItem_With_Provided_Name_And_Value()
        {
            // Arrange
            string name = "item1";
            var obj = new { };

            // Act
            LogItem logItem = LogItem.FromObj(name, obj);

            // Assert
            Assert.AreEqual(name, logItem.Name);
            Assert.AreEqual(obj, logItem.Value);
        }
        [Test]
        public void FromObj_Taking_Name_And_Value_When_Called_With_Null_Name_Creates_And_Returns_LogItem_With_Null_Name()
        {
            // Act
            LogItem logItem = LogItem.FromObj(null as string, new { });

            // Assert
            Assert.AreEqual(null, logItem.Name);
        }
        [Test]
        public void FromObj_Taking_Name_And_Value_When_Called_With_Null_Value_Creates_And_Returns_LogItem_With_Null_Value()
        {
            // Act
            LogItem logItem = LogItem.FromObj("item1", null as object);

            // Assert 
            Assert.AreEqual(null, logItem.Value);
        }

        #endregion

        #region Tests for FromEx method

        [Test]
        public void FromEx_Taking_Exception_When_Called_Creates_And_Returns_LogItem_With_Provided_Value()
        {
            // Arrange
            var ex = new Exception();

            // Act
            LogItem logItem = LogItem.FromEx(ex);

            // Assert
            Assert.AreEqual("Exception", logItem.Name);
            Assert.AreEqual(ex, logItem.Value);
        }
        [Test]
        public void FromEx_Taking_Exception_When_Called_With_Null_Creates_And_Returns_LogItem_With_Null_Value()
        {
            // Act
            LogItem logItem = LogItem.FromEx(null as Exception);

            // Assert
            Assert.AreEqual("Exception", logItem.Name);
            Assert.AreEqual(null, logItem.Value);
        }

        #endregion

        #region Tests for FromSeveral method

        [Test]
        public void FromSeveral_Taking_Objects_Array_When_Called_Returns_Log_Items_Array()
        {
            var obj1 = new { a = "a" };
            var obj2 = new { b = "b" };
            var obj3 = new { c = "c" };

            // Act
            LogItem[] logItems = LogItem.FromSeveral(obj1, obj2, obj3);

            // Assert
            Assert.AreEqual(3, logItems.Length);
            //
            Assert.AreEqual("Log item", logItems[0].Name);
            Assert.AreEqual(obj1, logItems[0].Value);
            //
            Assert.AreEqual("Log item", logItems[1].Name);
            Assert.AreEqual(obj2, logItems[1].Value);
            //
            Assert.AreEqual("Log item", logItems[2].Name);
            Assert.AreEqual(obj3, logItems[2].Value);
        }
        [Test]
        public void FromSeveral_Taking_Objects_Array_When_Called_With_One_Object_Returns_Log_Items_Array()
        {
            var obj = new { a = "a" };

            // Act
            LogItem[] logItems = LogItem.FromSeveral(obj);

            // Assert
            Assert.AreEqual(1, logItems.Length); 
            Assert.AreEqual("Log item", logItems[0].Name);
            Assert.AreEqual(obj, logItems[0].Value);
        }
        [Test]
        public void FromSeveral_Taking_Objects_Array_When_Called_With_Null_Array_Returns_Empty_Log_Items_Array()
        {
            // Act
            LogItem[] logItems = LogItem.FromSeveral(null as object[]);

            // Assert
            Assert.NotNull(logItems);
            Assert.AreEqual(0, logItems.Length);
        }
        [Test]
        public void FromSeveral_Taking_Objects_Array_When_Called_With_Empty_Array_Returns_Empty_Log_Items_Array()
        {
            // Act
            LogItem[] logItems = LogItem.FromSeveral(new object[0]);

            // Assert
            Assert.NotNull(logItems);
            Assert.AreEqual(0, logItems.Length);
        }

        #endregion 
    }
}
