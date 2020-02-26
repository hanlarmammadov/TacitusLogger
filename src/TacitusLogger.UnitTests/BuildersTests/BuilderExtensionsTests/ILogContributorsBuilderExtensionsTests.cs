using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;
using TacitusLogger.Contributors;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class ILogContributorsBuilderExtensionsTests
    {
        #region Tests for StackTrace method

        [Test]
        public void StackTrace_When_Called_Calls_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logContributorsBuilderMock = new Mock<ILogContributorsBuilder>();
            Setting<bool> isActive = new Mock<Setting<bool>>().Object;

            // Act
            ILogContributorsBuilderExtensions.StackTrace(logContributorsBuilderMock.Object, isActive, "custom name");

            // Assert
            logContributorsBuilderMock.Verify(x => x.Custom(It.Is<StackTraceContributor>(c => c.Name == "custom name"), isActive), Times.Once);
        }
        [Test]
        public void StackTrace_When_Called_Without_Params_Calls_Custom_Method_Of_ILogContributorsBuilder_With_Defaults()
        {
            // Arrange
            var logContributorsBuilderMock = new Mock<ILogContributorsBuilder>();

            // Act
            ILogContributorsBuilderExtensions.StackTrace(logContributorsBuilderMock.Object);

            // Assert
            logContributorsBuilderMock.Verify(x => x.Custom(It.Is<StackTraceContributor>(c => c.Name == "Stack trace"), It.Is<Setting<bool>>(v => v.Value == true)), Times.Once);
        }
        [Test]
        public void StackTrace_When_Called_Returns_Result_Of_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logContributorsBuilderMock = new Mock<ILogContributorsBuilder>();
            logContributorsBuilderMock.Setup(x => x.Custom(It.IsAny<LogContributorBase>(), It.IsAny<Setting<bool>>())).Returns(logContributorsBuilderMock.Object);

            // Act
            ILogContributorsBuilder logContributorsBuilderReturned = ILogContributorsBuilderExtensions.StackTrace(logContributorsBuilderMock.Object);

            // Assert
            Assert.AreEqual(logContributorsBuilderMock.Object, logContributorsBuilderReturned);
        }

        #endregion

        #region Tests for Custom method

        [Test]
        public void Custom_Taking_LogContributor_And_IsActive_Bool_Flag_When_Called_Calls_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logContributorsBuilderMock = new Mock<ILogContributorsBuilder>();
            var logContributorBase = new Mock<LogContributorBase>("name1").Object;
            bool isActive = false;

            // Act
            ILogContributorsBuilderExtensions.Custom(logContributorsBuilderMock.Object, logContributorBase, isActive);

            // Assert
            logContributorsBuilderMock.Verify(x => x.Custom(logContributorBase, It.Is<Setting<bool>>(s => s.Value == isActive)), Times.Once);
        }

        [Test]
        public void Custom_Taking_LogContributor_And_IsActive_Bool_Flag_When_Called_Returns_Result_Of_Called_Custom_Method_Of_ILogContributorsBuilder()
        {
            // Arrange
            var logContributorsBuilderMock = new Mock<ILogContributorsBuilder>();
            var expectedLogContributorsBuilder = new Mock<ILogContributorsBuilder>().Object;
            logContributorsBuilderMock.Setup(x => x.Custom(It.IsAny<LogContributorBase>(), It.IsAny<Setting<bool>>())).Returns(expectedLogContributorsBuilder);

            // Act
            var returnedResult = ILogContributorsBuilderExtensions.Custom(logContributorsBuilderMock.Object, new Mock<LogContributorBase>("name1").Object, false);

            // Assert
            Assert.AreEqual(expectedLogContributorsBuilder, returnedResult);
        }

        #endregion 
    }
}
