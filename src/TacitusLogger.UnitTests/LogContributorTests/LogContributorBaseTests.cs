using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Contributors;

namespace TacitusLogger.UnitTests.LogContributorTests
{
    [TestFixture]
    public class LogContributorBaseTests
    {
        public class LogContributorTestSubclass : LogContributorBase
        {
            public LogContributorTestSubclass(string name)
                : base(name)
            {

            }

            protected override object GenerateLogItemData()
            {
                throw new NotImplementedException();
            }
            protected override Task<object> GenerateLogItemDataAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Name_And_IsActive_Properties_Correctly()
        {
            //Act 
            LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass("name1");

            //Assert
            Assert.AreEqual("name1", logContributorTestSubclass.Name);
            Assert.IsTrue(logContributorTestSubclass.IsActive);
        }

        [Test]
        public void Ctor_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act 
                LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass(name: null);
            });
        }

        #endregion

        #region Tests for SetActive method

        [Test]
        public void SetActive_When_Called_Sets_IsActive_Property_Correctly()
        {
            //Arrange
            LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass("name1");
            Setting<bool> isActive = Setting<bool>.From.Variable(true);

            //Act
            logContributorTestSubclass.SetActive(isActive);

            // Assert
            Assert.AreEqual(isActive, logContributorTestSubclass.IsActive);
        }
        [Test]
        public void SetActive_When_Called_With_Null_Throws_ArgumentNullException()
        {
            //Arrange
            LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass("name1");

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
             {
                 //Act
                 logContributorTestSubclass.SetActive(null as Setting<bool>);
             });
            Assert.AreEqual("isActive", ex.ParamName);
        }
        [Test]
        public void SetActive_When_Called_With_Bool_Sets_IsActive_Property_Correctly()
        {
            //Arrange
            LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass("name1");

            //Act
            logContributorTestSubclass.SetActive(false);
            Assert.IsFalse(logContributorTestSubclass.IsActive);
            logContributorTestSubclass.SetActive(true);
            Assert.IsTrue(logContributorTestSubclass.IsActive);
            logContributorTestSubclass.SetActive(false);
            Assert.IsFalse(logContributorTestSubclass.IsActive);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_String_Containing_Contributor_Info()
        {
            //Arrange
            LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass("name1");
     
            //Act
            var result = logContributorTestSubclass.ToString();

            // Assert 
            Assert.IsTrue(result.Contains(logContributorTestSubclass.GetType().FullName));
            Assert.IsTrue(result.Contains("name1")); 
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_IsActive_Setting()
        {
            //Arrange
            LogContributorTestSubclass logContributorTestSubclass = new LogContributorTestSubclass("name1");
            var isActiveSettingMock = new Mock<Setting<bool>>();
            var description = "description";
            isActiveSettingMock.Setup(x => x.ToString()).Returns(description);
            logContributorTestSubclass.SetActive(isActiveSettingMock.Object);

            //Act
            var result = logContributorTestSubclass.ToString();

            // Assert
            isActiveSettingMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(description));
        }

        #endregion
    }
}
