using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Contributors;

namespace TacitusLogger.UnitTests.LogContributorTests
{
    [TestFixture]
    public class StackTraceContributorTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_IsActive_As_True()
        {
            //Act
            StackTraceContributor stackTraceContributor = new StackTraceContributor();

            //Assert 
            Assert.IsTrue(stackTraceContributor.IsActive);
        }

        [Test]
        public void Ctor_When_Called_Without_Name_Sets_Default()
        {
            //Act
            StackTraceContributor stackTraceContributor = new StackTraceContributor();

            //Assert
            Assert.AreEqual("Stack trace", stackTraceContributor.Name);
            Assert.IsTrue(stackTraceContributor.IsActive);
        }

        [Test]
        public void Ctor_When_Called_With_Custom_Name_Sets_Default()
        {
            //Act
            StackTraceContributor stackTraceContributor = new StackTraceContributor("Custom name");

            //Assert
            Assert.AreEqual("Custom name", stackTraceContributor.Name);
        }

        #endregion

        #region Tests for ProduceLogItem method

        [Test]
        public void ProduceLogItem_When_Called_Returns_LogItem_With_Data_In_It()
        {
            //Arrange
            StackTraceContributor stackTraceLogContributor = new StackTraceContributor();

            //Act
            LogItem logItem = stackTraceLogContributor.ProduceLogItem();

            //Assert
            Assert.NotNull(logItem);
            Assert.AreEqual("Stack trace", logItem.Name);
            Assert.NotNull(logItem.Value);
        }

        #endregion
         
        #region Tests for ProduceLogItemAsync method

        [Test]
        public async Task ProduceLogItemAsync_When_Called_Asynchronously_Returns_Log_Item_With_Data_In_It()
        {
            //Arrange
            StackTraceContributor stackTraceContributor = new StackTraceContributor();

            //Act
            LogItem logItem = await stackTraceContributor.ProduceLogItemAsync();

            //Assert
            Assert.NotNull(logItem);
            Assert.AreEqual("Stack trace", logItem.Name);
            Assert.NotNull(logItem.Value);
        }
        [Test]
        public void ProduceLogItemAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            //Arrange
            StackTraceContributor stackTraceContributor = new StackTraceContributor();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            //Act
            var resultTask = stackTraceContributor.ProduceLogItemAsync(cancellationToken);

            //Assert
            Assert.NotNull(resultTask);
            Assert.IsTrue(resultTask.IsCanceled);
        }

        #endregion
    }
}
