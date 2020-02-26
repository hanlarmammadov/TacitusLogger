using NUnit.Framework;
using System;
using System.Collections.Generic;
using TacitusLogger.Transformers;
using static TacitusLogger.Transformers.StringsManualTransformer;

namespace TacitusLogger.UnitTests.TransformerTests
{
    [TestFixture]
    public class StringsManualTransformerTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Delegate_And_Name_When_Called_Sets_Them()
        {
            // Arrange
            StringDelegate stringTransformerDelegate = (ref string s) => { };

            // Act
            StringsManualTransformer stringsManualTransformer = new StringsManualTransformer(stringTransformerDelegate, "some name");

            // Assert
            Assert.AreEqual(stringTransformerDelegate, stringsManualTransformer.TransformerDelegate);
            Assert.AreEqual("some name", stringsManualTransformer.Name);
        }
        [Test]
        public void Ctor_Taking_Delegate_And_Name_When_Called_With_Null_Delegate_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                StringsManualTransformer stringsManualTransformer = new StringsManualTransformer(null as StringDelegate, "some name");
            });
            Assert.AreEqual("transformerDelegate", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Delegate_And_Name_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                StringsManualTransformer stringsManualTransformer = new StringsManualTransformer((ref string s) => { }, null as string);
            });
            Assert.AreEqual("name", ex.ParamName);
        }

        #endregion

        #region Tests for Transform method

        [Test]
        public void Transform_When_Called_Sends_String_Properties_Of_Log_To_TransformString_Method()
        {
            // Arrange
            List<string> receivedStrings = new List<string>();
            var stringsManualTransformer = new StringsManualTransformer((ref string str) =>
            {
                receivedStrings.Add(str);
            });
            var logModel = new LogModel()
            {
                Context = "Context1",
                Description = "Description1",
                LogId = "LogId1",
                Source = "Source1",
                Tags = new string[] { "tag1", "tag2", "tag3" },
                LogItems = new LogItem[] { new LogItem("item1", new { }), new LogItem("item2", new { }), new LogItem("item3", new { }) }
            };

            // Act
            stringsManualTransformer.Transform(logModel);

            // Assert
            Assert.IsTrue(receivedStrings.Contains(logModel.Context));
            Assert.IsTrue(receivedStrings.Contains(logModel.Description));
            Assert.IsTrue(receivedStrings.Contains(logModel.LogId));
            Assert.IsTrue(receivedStrings.Contains(logModel.Source));
            for (int i = 0; i < logModel.Tags.Length; i++)
                Assert.IsTrue(receivedStrings.Contains(logModel.Tags[i]));
            for (int i = 0; i < logModel.LogItems.Length; i++)
                Assert.IsTrue(receivedStrings.Contains(logModel.LogItems[i].Name));
        }
     
        #endregion
    }
}
