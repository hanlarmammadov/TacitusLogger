using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TacitusLogger.Components.TemplateResolving;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.Serializers
{
    /// <summary>
    /// Implements log model serialization by means of a custom (or default) string template that has special keys-placeholders.
    /// </summary>
    public class FilePathTemplateLogSerializer : TemplateLogSerializerBase
    { 
        private static readonly string _defaultTemplate = Templates.FilePath.Default;
        private static readonly string _defaultDateFormat = Templates.DateFormats.DefaultFileName;
        private ILogModelTemplateResolver _logModelTemplateResolver;


        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.Serializers.FilePathTemplateLogSerializer</c> with the specified template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If specified template is null.</exception>
        /// <param name="template">Custom template.</param>
        public FilePathTemplateLogSerializer(string template)
            :base(template)
        {
            // Create placeholder resolvers list according to permitted placeholders for this serializer and pass it to LogModelTemplateResolver.
            List<IPlaceholderResolver> placeholderResolvers = new List<IPlaceholderResolver>()
            {
                new SourcePlaceholderResolver(),
                new ContextPlaceholderResolver(),
                new LogIdPlaceholderResolver(),
                new LogTypePlaceholderResolver(),
                new DescriptionPlaceholderResolver(),
                new LogDatePlaceholderResolver(_defaultDateFormat)
            };
            _logModelTemplateResolver = new LogModelTemplateResolver(placeholderResolvers);
        }
        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.Serializers.FilePathTemplateLogSerializer</c> with the default template.
        /// </summary>
        public FilePathTemplateLogSerializer()
            : this(_defaultTemplate)
        {

        }


        /// <summary>
        /// Gets the default date format used for serialized representation of log date.
        /// </summary>
        public static string DefaultDateFormat => _defaultDateFormat;
        /// <summary>
        /// Gets the default template.
        /// </summary>
        public static string DefaultTemplate => _defaultTemplate;
        /// <summary>
        /// Gets the log model template resolver that was set during initialization. For testing purposes.
        /// </summary>
        public override ILogModelTemplateResolver LogModelTemplateResolver => _logModelTemplateResolver;

        public override string ToString()
        {
            return new StringBuilder()
               .AppendLine(this.GetType().FullName)
               .AppendLine($"Template: {_template}")
               .Append($"Default date format: {_defaultDateFormat}")
               .ToString();
        }
        /// <summary>
        /// Resets the log model template resolver that was set during initialization. For testing purposes.
        /// </summary>
        /// <param name="logModelTemplateResolver">New log model template resolver.</param>
        internal void ResetLogModelTemplateResolver(ILogModelTemplateResolver logModelTemplateResolver)
        {
            _logModelTemplateResolver = logModelTemplateResolver;
        }
    }
}
