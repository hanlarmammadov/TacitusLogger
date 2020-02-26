using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class IBuilderWithFilePathExtensionsTests
    {
        public class Builder
        {

        }

        [Test]
        public void WithPath_Taking_IBuilderWithFilePath_And_Template_When_Called_Calls_WithPath_Of_IBuilderWithFilePath()
        {
            // Arrange 
            var builderWithFilePathMock = new Mock<IBuilderWithFilePath<Builder>>();
            string pathTemplate = "pathTemplate";

            // Act 
            IBuilderWithFilePathExtensions.WithPath(builderWithFilePathMock.Object, pathTemplate);

            // Assert
            builderWithFilePathMock.Verify(x => x.WithPath(It.Is<FilePathTemplateLogSerializer>(g => g.Template == pathTemplate)), Times.Once);
        }

        [Test]
        public void WithPath_Taking_IBuilderWithFilePath_And_Delegate_When_Called_Calls_WithPath_Of_IBuilderWithFilePath()
        {
            // Arrange 
            var builderWithFilePathMock = new Mock<IBuilderWithFilePath<Builder>>();
            LogModelFunc<string> generatorFunc = (ld) => "";

            // Act 
            IBuilderWithFilePathExtensions.WithPath(builderWithFilePathMock.Object, generatorFunc);

            // Assert
            builderWithFilePathMock.Verify(x => x.WithPath(It.Is<GeneratorFunctionLogSerializer>(s => s.GeneratorFunction == generatorFunc)), Times.Once);
        }
    }
}
