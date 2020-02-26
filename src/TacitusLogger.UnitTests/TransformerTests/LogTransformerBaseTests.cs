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
    public class LogTransformerBaseTests
    {
        public class TestTransformer : LogTransformerBase
        {
            public TestTransformer(string name)
                : base(name)
            {

            }

            public override void Transform(LogModel logModel)
            {
                throw new NotImplementedException();
            }
            public override Task TransformAsync(LogModel logModel, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        #region Ctor tests

        [Test]
        public void Ctor_Taking_Name_When_Called_Sets_Name()
        {
            // Arrange
            string name = "some name";

            // Act 
            TestTransformer testTransformer = new TestTransformer(name);

            // Arrange
            Assert.AreEqual(name, testTransformer.Name);
        }
        [Test]
        public void Ctor_Taking_Name_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act 
                TestTransformer testTransformer = new TestTransformer(null as string);
            });
            Assert.AreEqual("name", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_When_Called_Sets_IsActive_Value_Provider_To_Const()
        {
            // Act 
            TestTransformer testTransformer = new TestTransformer("some name");

            // Arrange
            Assert.IsInstanceOf<Setting<bool>>(testTransformer.IsActive);
            Assert.AreEqual(true, testTransformer.IsActive.Value);
        }

        #endregion

        #region Tests SetActive method

        [Test]
        public void SetActive_Taking_IsActive_Status_Value_Provider_When_Called_Sets_Status_Value_Provider()
        {
            // Arrange
            Setting<bool> isActive = new Mock<Setting<bool>>().Object;
            TestTransformer testTransformer = new TestTransformer("");

            // Act 
            testTransformer.SetActive(isActive);

            // Arrange
            Assert.AreEqual(isActive, testTransformer.IsActive);
        }
        [Test]
        public void SetActive_Taking_IsActive_Status_Value_Provider_When_Called_With_Null_Throws_ArgumentNullException()
        {
            // Arrange 
            TestTransformer testTransformer = new TestTransformer("");

            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act 
                testTransformer.SetActive(null as Setting<bool>);
            });
            Assert.AreEqual("isActive", ex.ParamName);
        }

        #endregion
         
        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_String_Containing_Transformer_Info()
        {
            //Arrange
            TestTransformer testTransformer = new TestTransformer("name1");

            //Act
            var result = testTransformer.ToString();

            // Assert 
            Assert.IsTrue(result.Contains(testTransformer.GetType().FullName));
            Assert.IsTrue(result.Contains("name1"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_IsActive_Setting()
        {
            //Arrange
            TestTransformer testTransformer = new TestTransformer("name1");
            var isActiveSettingMock = new Mock<Setting<bool>>();
            var description = "description";
            isActiveSettingMock.Setup(x => x.ToString()).Returns(description);
            testTransformer.SetActive(isActiveSettingMock.Object);

            //Act
            var result = testTransformer.ToString();

            // Assert
            isActiveSettingMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(description));
        }

        #endregion
    }
}
