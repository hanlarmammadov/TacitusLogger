using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;
using TacitusLogger.Contributors;
using TacitusLogger.Transformers;
using static TacitusLogger.Transformers.StringsManualTransformer;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class ILogTransformersBuilderExtensionsTests
    {
        #region Tests for StringsManual method

        [Test]
        public void StringsManual_Taking_StringDelegate_IsActive_Setting_And_Name_When_Called_Calls_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logTransformersBuilderMock = new Mock<ILogTransformersBuilder>();
            StringDelegate stringDelegate = (ref string str) => { };
            Setting<bool> isActive = new Mock<Setting<bool>>().Object;
            string name = "custom name";

            // Act
            ILogTransformersBuilderExtensions.StringsManual(logTransformersBuilderMock.Object, stringDelegate, isActive, name);

            // Assert
            logTransformersBuilderMock.Verify(x => x.Custom(It.Is<StringsManualTransformer>(c => c.TransformerDelegate == stringDelegate && c.Name == name), isActive), Times.Once);
        }
        [Test]
        public void StringsManual_Taking_StringDelegate_IsActive_Setting_And_Name_When_Called_Returns_Result_Of_Called_Custom_Method()
        {
            // Arrange
            var logTransformersBuilderMock = new Mock<ILogTransformersBuilder>();
            var logTransformersBuilderReturnedFromCustomMethod = new Mock<ILogTransformersBuilder>().Object;
            logTransformersBuilderMock.Setup(x => x.Custom(It.IsAny<LogTransformerBase>(), It.IsAny<Setting<bool>>())).Returns(logTransformersBuilderReturnedFromCustomMethod);

            // Act
            var returnedResult = ILogTransformersBuilderExtensions.StringsManual(logTransformersBuilderMock.Object, (ref string str) => { }, new Mock<Setting<bool>>().Object, "custom name");

            // Assert
            Assert.AreEqual(logTransformersBuilderReturnedFromCustomMethod, returnedResult);
        }
        [Test]
        public void StringsManual_Taking_StringDelegate_IsActive_Bool_Flag_And_Name_When_Called_Calls_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logTransformersBuilderMock = new Mock<ILogTransformersBuilder>();
            StringDelegate stringDelegate = (ref string str) => { };
            bool isActive = false;
            string name = "custom name";

            // Act
            ILogTransformersBuilderExtensions.StringsManual(logTransformersBuilderMock.Object, stringDelegate, isActive, name);

            // Assert
            logTransformersBuilderMock.Verify(x => x.Custom(It.Is<StringsManualTransformer>(c => c.TransformerDelegate == stringDelegate && c.Name == "custom name"), It.Is<Setting<bool>>(s => s.Value == isActive)), Times.Once);
        }
        [Test]
        public void StringsManual_Taking_StringDelegate_IsActive_Bool_Flag_And_Name_When_Called_Returns_Result_Of_Called_Custom_Method()
        {
            // Arrange
            var logTransformersBuilderMock = new Mock<ILogTransformersBuilder>();
            var logTransformersBuilderReturnedFromCustomMethod = new Mock<ILogTransformersBuilder>().Object;
            logTransformersBuilderMock.Setup(x => x.Custom(It.IsAny<LogTransformerBase>(), It.IsAny<Setting<bool>>())).Returns(logTransformersBuilderReturnedFromCustomMethod);

            // Act
            var returnedResult = ILogTransformersBuilderExtensions.StringsManual(logTransformersBuilderMock.Object, (ref string str) => { }, false, "custom name");

            // Assert
            Assert.AreEqual(logTransformersBuilderReturnedFromCustomMethod, returnedResult);
        }

        #endregion

        #region Tests for Custom method

        [Test]
        public void Custom_Taking_LogTransformer_And_IsActive_Bool_Flag_When_Called_Calls_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logTransformersBuilderMock = new Mock<ILogTransformersBuilder>();
            var logTransformerBase = new Mock<LogTransformerBase>("name1").Object;
            bool isActive = false;

            // Act
            ILogTransformersBuilderExtensions.Custom(logTransformersBuilderMock.Object, logTransformerBase, isActive);

            // Assert
            logTransformersBuilderMock.Verify(x => x.Custom(logTransformerBase, It.Is<Setting<bool>>(s => s.Value == isActive)), Times.Once);
        }

        [Test]
        public void Custom_Taking_LogTransformer_And_IsActive_Bool_Flag_When_Called_Returns_Result_Of_Called_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logTransformersBuilderMock = new Mock<ILogTransformersBuilder>();
            var expectedLogTransformersBuilder = new Mock<ILogTransformersBuilder>().Object;
            logTransformersBuilderMock.Setup(x => x.Custom(It.IsAny<LogTransformerBase>(), It.IsAny<Setting<bool>>())).Returns(expectedLogTransformersBuilder);

            // Act
            var returnedResult = ILogTransformersBuilderExtensions.Custom(logTransformersBuilderMock.Object, new Mock<LogTransformerBase>("name1").Object, false);

            // Assert
            Assert.AreEqual(expectedLogTransformersBuilder, returnedResult);
        }

        #endregion 
    }
}
