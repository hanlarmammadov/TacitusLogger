using Newtonsoft.Json;
using System;
using System.ComponentModel;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.IBuilderWithFilePath<TBuilder></c> interface and its implementations.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IBuilderWithFilePathExtensions
    {

        #region Extension methods calling WithPath method

        /// <summary>
        /// Adds a custom file path generator of type <c>TacitusLogger.Serializers.FilePathTemplateLogSerializer</c>
        /// with the specified path template.
        /// </summary>
        /// <param name="pathTemplate">Path template string that will be used to generate file path using log model.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithPath<TBuilder>(this IBuilderWithFilePath<TBuilder> self, string pathTemplate)
        {
            return self.WithPath(new FilePathTemplateLogSerializer(pathTemplate));
        }
        /// <summary>
        /// Adds a custom file path generator of type <c>TacitusLogger.Serializers.GeneratorFunctionLogSerializer</c>
        /// with the specified delegate of type <c>LogModelFunc<string></c>.
        /// </summary>
        /// <param name="generatorFunc">Delegate that will be used to generate file path using log model.</param>
        /// <returns>Self.</returns>
        public static TBuilder WithPath<TBuilder>(this IBuilderWithFilePath<TBuilder> self, LogModelFunc<string> generatorFunc)
        {
            return self.WithPath(new GeneratorFunctionLogSerializer(generatorFunc));
        }

        #endregion
    }
}
