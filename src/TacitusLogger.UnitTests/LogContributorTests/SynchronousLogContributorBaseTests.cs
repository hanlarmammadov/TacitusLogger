using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TacitusLogger.Contributors;

namespace TacitusLogger.UnitTests.LogContributorTests
{
    [TestFixture]
    public class SynchronousLogContributorBaseTests
    {
        public class TestContributor : SynchronousLogContributorBase
        {
            private readonly Func<object> _testCallback;

            public TestContributor(string name, Func<object> testCallback = null)
                : base(name)
            {
                _testCallback = testCallback;
            }

            protected override object GenerateLogItemData()
            {
                return _testCallback();
            }
        }

        #region Ctor tests 

        [Test]
        public void Ctor_When_Called_Sets_Contributors_Name()
        {
            // Act
            TestContributor testContributor = new TestContributor("Contributor1");

            // Assert
            Assert.AreEqual("Contributor1", testContributor.Name);
        }
        [Test]
        public void Ctor_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TestContributor testContributor = new TestContributor(null as string);
            });
            Assert.AreEqual("name", ex.ParamName);
        }

        #endregion

        #region Tests for GenerateLogItemDataAsync method

        [Test]
        public void GenerateLogItemDataAsync_When_Called_Calls_GenerateLogItemData_Synchronously_And_Returns_Its_Result_In_Completed_Task()
        {
            // Arrange
            var resultFromGenerateLogItemData = new { };
            TestContributor testContributor = new TestContributor("Contributor1", () =>
            {
                return resultFromGenerateLogItemData;
            });
            CancellationToken cancellationToken = new CancellationToken(canceled: false);

            // Act
            var resultTask = testContributor.ProduceLogItemAsync(cancellationToken);

            // Assert
            Assert.NotNull(resultTask);
            Assert.IsTrue(resultTask.IsCompleted);
            Assert.AreEqual(resultFromGenerateLogItemData, resultTask.Result.Value);
        }
        [Test]
        public void GenerateLogItemDataAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            // Arrange
            bool syncMethodWasCalled = false;
            TestContributor testContributor = new TestContributor("Contributor1", () =>
            {
                syncMethodWasCalled = true;
                return null;
            });
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Act
            var resultTask = testContributor.ProduceLogItemAsync(cancellationToken);

            // Assert
            Assert.NotNull(resultTask);
            Assert.IsTrue(resultTask.IsCanceled);
            Assert.IsFalse(syncMethodWasCalled); 
        } 

        #endregion

    }
}
