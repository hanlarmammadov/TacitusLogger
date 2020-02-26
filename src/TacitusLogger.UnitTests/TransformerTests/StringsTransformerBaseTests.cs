using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Transformers;

namespace TacitusLogger.UnitTests.TransformerTests
{
    [TestFixture]
    public class StringsTransformerBaseTests
    {
        public class TestStringsTransformer : StringsTransformerBase
        {
            private readonly Action<string> _testCallback;

            public TestStringsTransformer(Action<string> testCallback)
                : base("testName")
            {
                _testCallback = testCallback;
            }

            protected override void TransformString(ref string str)
            {
                _testCallback(str);
            }
        }

        #region Ctor tests

        [Test]
        public void Transform_When_Called_Sets_Name()
        {
            // Act
            StringsTransformerBase stringsTransformer = new Mock<StringsTransformerBase>("some name").Object;

            // Assert
            Assert.AreEqual("some name", stringsTransformer.Name);
        }

        #endregion

        #region Tests for Transform method

        [Test]
        public void Transform_When_Called_Sends_String_Properties_Of_Log_To_TransformString_Method()
        {
            // Arrange
            List<string> receivedStrings = new List<string>();
            var testStringsTransformer = new TestStringsTransformer((str) =>
            {
                receivedStrings.Add(str);
            });

            var logModel = new LogModel()
            {
                Context = "Context1",
                Description = "Description1",
                LogId = "LogId1",
                Source = "Source1"
            };

            // Act
            testStringsTransformer.Transform(logModel);

            // Assert
            Assert.IsTrue(receivedStrings.Contains(logModel.Context));
            Assert.IsTrue(receivedStrings.Contains(logModel.Description));
            Assert.IsTrue(receivedStrings.Contains(logModel.LogId));
            Assert.IsTrue(receivedStrings.Contains(logModel.Source));
        }
        [Test]
        public void Transform_When_Called_Sends_Tags_To_TransformString_Method()
        {
            // Arrange
            List<string> receivedStrings = new List<string>();
            var testStringsTransformer = new TestStringsTransformer((str) =>
            {
                receivedStrings.Add(str);
            });

            var logModel = new LogModel()
            {
                Tags = new string[] { "tag1", "tag2", "tag3" }
            };

            // Act
            testStringsTransformer.Transform(logModel);

            // Assert
            for (int i = 0; i < logModel.Tags.Length; i++)
                Assert.IsTrue(receivedStrings.Contains(logModel.Tags[i]));
        }
        [Test]
        public void Transform_When_Called_Sends_Log_Items_Names_To_TransformString_Method()
        {
            // Arrange
            List<string> receivedStrings = new List<string>();
            var testStringsTransformer = new TestStringsTransformer((str) =>
            {
                receivedStrings.Add(str);
            });
            var logModel = new LogModel()
            {
                LogItems = new LogItem[] { new LogItem("item1", new { }), new LogItem("item2", new { }), new LogItem("item3", new { }) }
            };

            // Act
            testStringsTransformer.Transform(logModel);

            // Assert
            for (int i = 0; i < logModel.LogItems.Length; i++)
                Assert.IsTrue(receivedStrings.Contains(logModel.LogItems[i].Name));
        }
        [Test]
        public void Transform_When_Called_Does_Not_Send_Log_Items_Values_To_TransformString_Method()
        {
            // Arrange
            List<string> receivedStrings = new List<string>();
            var testStringsTransformer = new TestStringsTransformer((str) =>
            {
                receivedStrings.Add(str);
            });
            var logModel = new LogModel()
            {
                LogItems = new LogItem[] { new LogItem("item1", "value1"), new LogItem("item2", "value2"), new LogItem("item3", "value3") }
            };

            // Act
            testStringsTransformer.Transform(logModel);

            // Assert
            for (int i = 0; i < logModel.LogItems.Length; i++)
                Assert.IsFalse(receivedStrings.Contains((string)logModel.LogItems[i].Value));
        }
        [Test]
        public void Transform_When_Called_Does_Not_Send_Nulls_To_TransformString_Method()
        {
            // Arrange
            List<string> receivedStrings = new List<string>();
            var testStringsTransformer = new TestStringsTransformer((str) =>
            {
                receivedStrings.Add(str);
            });
            var logModel = new LogModel()
            {
                Context = null,
                Description = null,
                Source = null,
                LogId = null
            };

            // Act
            testStringsTransformer.Transform(logModel);

            // Assert
            Assert.AreEqual(0, receivedStrings.Count);
        }

        #endregion

        #region Tests for TransformAsync

        [Test]
        public void TransformAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            // Arrange
            bool wasEverCalled = false;
            var testStringsTransformer = new TestStringsTransformer((str) =>
            {
                wasEverCalled = true;
            });
            var logModel = new LogModel()
            {
                Context = "Context1"
            };
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Act
            Task resultTask = testStringsTransformer.TransformAsync(logModel, cancellationToken);

            // Assert
            Assert.IsTrue(resultTask.IsCanceled);
            Assert.IsFalse(wasEverCalled);
        }

        #endregion
    }
}
