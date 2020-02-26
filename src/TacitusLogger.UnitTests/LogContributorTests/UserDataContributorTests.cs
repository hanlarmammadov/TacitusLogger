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
    public class UserDataContributorTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Name_IsActive_And_Custom_Data()
        {
            // Arrange
            string name = "name";
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[3];

            // Act
            UserDataContributor customContributor = new UserDataContributor(name, data);

            // Assert
            Assert.AreEqual(name, customContributor.Name);
            Assert.AreEqual(data, customContributor.Data);
            Assert.IsTrue(customContributor.IsActive);
        }

        [Test]
        public void Ctor_When_Called_With_Null_Data_Sets_Data_As_Null()
        {
            // Act
            UserDataContributor customContributor = new UserDataContributor("name", null as KeyValuePair<string, string>[]);

            // Assert
            Assert.AreEqual("name", customContributor.Name);
            Assert.IsNull(customContributor.Data);
        }

        #endregion

        #region Tests for ProduceLogItem method

        [Test]
        public void ProduceLogItem_When_Called_Returns_LogItem_With_Data_In_It()
        {
            // Arrange
            string name = "name";
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[3];
            UserDataContributor customContributor = new UserDataContributor(name, data);

            //Act
            LogItem logItem = customContributor.ProduceLogItem();

            //Assert
            Assert.NotNull(logItem);
            Assert.AreEqual(name, logItem.Name);
            Assert.AreEqual(data, logItem.Value);
        } 
        [Test]
        public void ProduceLogItem_When_Called_Given_That_Data_Is_Null_Returns_LogItem_With_Null_Data_In_It()
        {
            // Arrange
            string name = "name";
            UserDataContributor customContributor = new UserDataContributor(name, null as KeyValuePair<string, string>[]);

            //Act
            LogItem logItem = customContributor.ProduceLogItem();

            //Assert
            Assert.NotNull(logItem);
            Assert.AreEqual(name, logItem.Name);
            Assert.IsNull(logItem.Value);
        }

        #endregion

        #region Tests for ProduceLogItemAsync method

        [Test]
        public async Task ProduceLogItemAsync_When_Called_Asynchronously_Returns_LogItem_With_Data_In_It()
        {
            // Arrange
            string name = "name";
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[3];
            UserDataContributor customContributor = new UserDataContributor(name, data);

            //Act
            LogItem logItem = await customContributor.ProduceLogItemAsync();

            //Assert
            Assert.NotNull(logItem);
            Assert.AreEqual(name, logItem.Name);
            Assert.AreEqual(data, logItem.Value);
        } 
        [Test]
        public async Task ProduceLogItemAsync_When_Called_Given_That_Data_Is_Null_Asynchronously_Returns_LogItem_With_Null_Data_In_It()
        {
            // Arrange
            string name = "name";
            UserDataContributor customContributor = new UserDataContributor(name, null as KeyValuePair<string, string>[]);

            //Act
            LogItem logItem = await customContributor.ProduceLogItemAsync();

            //Assert
            Assert.NotNull(logItem);
            Assert.AreEqual(name, logItem.Name);
            Assert.IsNull(logItem.Value);
        } 
        [Test]
        public void ProduceLogItemAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            //Arrange
            UserDataContributor customContributor = new UserDataContributor("name", null as KeyValuePair<string, string>[]);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            //Act
            var resultTask = customContributor.ProduceLogItemAsync(cancellationToken);

            //Assert
            Assert.NotNull(resultTask);
            Assert.IsTrue(resultTask.IsCanceled);
        }

        #endregion
    }
}
