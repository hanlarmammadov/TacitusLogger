using Newtonsoft.Json;
using System;
using System.ComponentModel;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.IBuilderWithLogTextSerialization<TBuilder></c> interface and its implementations.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IBuilderWithLogTextSerializationExtensions
    {
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c>
        /// with the specified log text template.
        /// </summary>
        /// <param name="template">String template that will be used to create log text from log model.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithSimpleTemplateLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, string template)
        {
            return self.WithCustomLogSerializer(new SimpleTemplateLogSerializer(template));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c><TacitusLogger.Serializers.SimpleTemplateLogSerializer</c>
        /// with the default log text template.
        /// </summary>
        /// <returns>Self.</returns>
        public static TBuilder WithSimpleTemplateLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self)
        {
            return self.WithCustomLogSerializer(new SimpleTemplateLogSerializer());
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c>
        /// with the specified log text template and JSON serializer settings.
        /// </summary>
        /// <param name="template">String template that will be used to create log text from log model.</param>
        /// <param name="jsonSerializerSettings">JSON serializer settings that will be used to serialize the logging object if present in the template.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithExtendedTemplateLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, string template, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithCustomLogSerializer(new ExtendedTemplateLogSerializer(template, jsonSerializerSettings));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c>
        /// with the specified log text template and default JSON serializer settings.
        /// </summary>
        /// <param name="template">String template that will be used to create log text from log model.</param> 
        /// <returns>Self.</returns>
        public static TBuilder WithExtendedTemplateLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, string template)
        {
            return self.WithCustomLogSerializer(new ExtendedTemplateLogSerializer(template));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c>
        /// with the default log text template and default JSON serializer settings.
        /// </summary>
        /// <returns>Self.</returns>
        public static TBuilder WithExtendedTemplateLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self)
        {
            return self.WithCustomLogSerializer(new ExtendedTemplateLogSerializer());
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.GeneratorFunctionLogSerializer</c>
        /// with the specified delegate of type <c>TacitusLogger.LogModelFunc<string></c>.
        /// </summary>
        /// <param name="deleg">Delegate that will be called to generate log text string from the log model.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithGeneratorFuncLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, LogModelFunc<string> generatorFunc)
        {
            return self.WithCustomLogSerializer(new GeneratorFunctionLogSerializer(generatorFunc));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.JsonLogSerializer</c>
        /// with the specified delegate of type <c>TacitusLogger.LogModelFunc<Object></c> and JSON serializer settings.
        /// This log serializer results in JSON string of arbitrary object as a log text.
        /// </summary>
        /// <param name="customObjectFactoryMethod">Used to generate a user defined object from the log model that will be used to build JSON log text.</param>
        /// <param name="jsonSerializerSettings">JSON serializer settings that will be used to serialize user defined object.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithJsonLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, LogModelFunc<Object> customObjectFactoryMethod, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithCustomLogSerializer(new JsonLogSerializer(customObjectFactoryMethod, jsonSerializerSettings));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.JsonLogSerializer</c>
        /// with the specified delegate of type <c>TacitusLogger.LogModelFunc<Object></c> and default JSON serializer settings.
        /// This log serializer results in JSON string of arbitrary object as a log text.
        /// </summary>
        /// <param name="customObjectFactoryMethod">Used to generate a user defined object from the log model that will be used to build JSON log text.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithJsonLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, LogModelFunc<Object> customObjectFactoryMethod)
        {
            return self.WithCustomLogSerializer(new JsonLogSerializer(customObjectFactoryMethod));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.JsonLogSerializer</c>
        /// with the specified JSON serializer settings.
        /// This log serializer results in JSON string of log model as log text.
        /// </summary>
        /// <param name="jsonSerializerSettings">JSON serializer settings that will be used to serialize log model.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithJsonLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithCustomLogSerializer(new JsonLogSerializer(jsonSerializerSettings));
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.JsonLogSerializer</c>
        /// with the default JSON serializer settings.
        /// This log serializer results in JSON string of log model as log text.
        /// </summary>
        /// <returns>Self.</returns>
        public static TBuilder WithJsonLogText<TBuilder>(this IBuilderWithLogTextSerialization<TBuilder> self)
        {
            return self.WithCustomLogSerializer(new JsonLogSerializer());
        }
    }
}
