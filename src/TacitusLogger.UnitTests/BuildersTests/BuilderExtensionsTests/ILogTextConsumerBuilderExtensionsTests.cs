using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class IBuilderWithLogTextSerializationExtensionsTests
    {
        public class Builder
        {

        }

        [Test]
        public void WithSimpleTemplateLogText_Taking_IBuilderWithLogTextSerialization_And_Template_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange 
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            string template = "template";

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithSimpleTemplateLogText(builderWithLogTextSerializationMock.Object, template);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<SimpleTemplateLogSerializer>(g => g.Template == template)), Times.Once);
        }

        [Test]
        public void WithSimpleTemplateLogText_Taking_IBuilderWithLogTextSerialization_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange 
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithSimpleTemplateLogText(builderWithLogTextSerializationMock.Object);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<SimpleTemplateLogSerializer>(g => g.Template == SimpleTemplateLogSerializer.DefaultTemplate)), Times.Once);
        }

        [Test]
        public void WithExtendedTemplateLogText_Taking_IBuilderWithLogTextSerialization_Template_And_JsonSettings_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange 
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            string template = "template";
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithExtendedTemplateLogText(builderWithLogTextSerializationMock.Object, template, jsonSerializerSettings);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(g => g.Template == template && g.JsonSerializerSettings == jsonSerializerSettings)), Times.Once);
        }

        [Test]
        public void WithExtendedTemplateLogText_Taking_IBuilderWithLogTextSerialization_And_Template_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange 
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            string template = "template";

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithExtendedTemplateLogText(builderWithLogTextSerializationMock.Object, template);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(g => g.Template == template && g.JsonSerializerSettings == ExtendedTemplateLogSerializer.DefaultJsonSerializerSettings)), Times.Once);
        }

        [Test]
        public void WithExtendedTemplateLogText_Taking_IBuilderWithLogTextSerialization_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange 
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithExtendedTemplateLogText(builderWithLogTextSerializationMock.Object);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(g => g.Template == ExtendedTemplateLogSerializer.DefaultTemplate && g.JsonSerializerSettings == ExtendedTemplateLogSerializer.DefaultJsonSerializerSettings)), Times.Once);
        }

        [Test]
        public void WithGeneratorFuncLogText_Taking_IBuilderWithLogTextSerialization_And_LogModelFunc_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange  
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            LogModelFunc<string> generatorFunc = (ld) => "";

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithGeneratorFuncLogText(builderWithLogTextSerializationMock.Object, generatorFunc);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<GeneratorFunctionLogSerializer>(s => s.GeneratorFunction == generatorFunc)), Times.Once);
        }

        [Test]
        public void WithJsonLogText_Taking_IConsoleDestinationBuilder_CustomObjectFactoryMethod_And_JsonSettings_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange  
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            LogModelFunc<Object> customObjectFactoryMethod = (ld) => new { };
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithJsonLogText(builderWithLogTextSerializationMock.Object, customObjectFactoryMethod, jsonSerializerSettings);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.Converter == customObjectFactoryMethod && s.JsonSerializerSettings == jsonSerializerSettings)), Times.Once);
        }

        [Test]
        public void WithJsonLogText_Taking_IConsoleDestinationBuilder_And_CustomObjectFactoryMethod_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange  
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            LogModelFunc<Object> customObjectFactoryMethod = (ld) => new { };

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithJsonLogText(builderWithLogTextSerializationMock.Object, customObjectFactoryMethod);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.Converter == customObjectFactoryMethod && s.JsonSerializerSettings == JsonLogSerializer.DefaultJsonSerializerSettings)), Times.Once);
        }

        [Test]
        public void WithJsonLogText_Taking_IConsoleDestinationBuilder_And_JsonSettings_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange  
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithJsonLogText(builderWithLogTextSerializationMock.Object, jsonSerializerSettings);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.JsonSerializerSettings == jsonSerializerSettings)), Times.Once);
        }

        [Test]
        public void WithJsonLogText_Taking_IConsoleDestinationBuilder_When_Called_Calls_WithLogSerializer_Method_Of_IBuilderWithLogTextSerialization()
        {
            // Arrange  
            var builderWithLogTextSerializationMock = new Mock<IBuilderWithLogTextSerialization<Builder>>();

            // Act 
            IBuilderWithLogTextSerializationExtensions.WithJsonLogText(builderWithLogTextSerializationMock.Object);

            // Assert
            builderWithLogTextSerializationMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.JsonSerializerSettings == JsonLogSerializer.DefaultJsonSerializerSettings)), Times.Once);
        }
    }
}
