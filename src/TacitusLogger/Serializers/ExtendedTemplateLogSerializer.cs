using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Components.Json;
using TacitusLogger.Components.Strings;
using TacitusLogger.Components.TemplateResolving;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.Serializers
{
    /// <summary>
    /// ExtendedTemplateLogSerializer class implements log model serialization by means of a custom (or default) string template that has special keys-placeholders.
    /// </summary>
    public class ExtendedTemplateLogSerializer : TemplateLogSerializerBase
    {
        private static readonly string _defaultTemplate = Templates.Extended.Default;
        private static readonly string _defaultDateFormat = Templates.DateFormats.Default;
        private static readonly JsonSerializerSettings _defaultJsonSerializerSettings = Defaults.JsonSerializerSettings;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private ILogModelTemplateResolver _logModelTemplateResolver;
        private IJsonSerializerFacade _jsonSerializerFacade;


        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> with the specified template and
        /// JSON serializer settings.
        /// </summary>
        /// <exception cref="ArgumentNullException">If specified template is null.</exception>
        /// <param name="template">Template that will be used to serialize log model.</param>
        /// <param name="jsonSerializerSettings">JSON serializer settings.</param>
        public ExtendedTemplateLogSerializer(string template, JsonSerializerSettings jsonSerializerSettings)
            : base(template)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? throw new ArgumentNullException("jsonSerializerSettings");
            _jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();

            // Create placeholder resolvers list according to permitted placeholders for this serializer and pass it to LogModelTemplateResolver.
            List<IPlaceholderResolver> placeholderResolvers = new List<IPlaceholderResolver>() {
                new NewLinePlaceholderResolver(),
                new SourcePlaceholderResolver(),
                new ContextPlaceholderResolver(),
                new TagsPlaceholderResolver(),
                new LogIdPlaceholderResolver(),
                new LogTypePlaceholderResolver(), 
                new LogDatePlaceholderResolver(_defaultDateFormat),
                new DescriptionPlaceholderResolver(),
                new LogItemsPlaceholderResolver(ref _jsonSerializerFacade, _jsonSerializerSettings),
            };
            _logModelTemplateResolver = new LogModelTemplateResolver(placeholderResolvers);
        }
        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> with provided template and default JSON 
        /// serializer settings.
        /// </summary>
        /// <param name="template">Template that will be used to serialize log model.</param>
        public ExtendedTemplateLogSerializer(string template)
            : this(template, DefaultJsonSerializerSettings)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> with provided JSON serializer settings and 
        /// default template.
        /// </summary>
        /// <param name="jsonSerializerSettings">JSON serializer settings.</param>
        public ExtendedTemplateLogSerializer(JsonSerializerSettings jsonSerializerSettings)
            : this(_defaultTemplate, jsonSerializerSettings)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> with default template
        /// and JSON serializer settings.
        /// </summary>
        public ExtendedTemplateLogSerializer()
            : this(_defaultTemplate, DefaultJsonSerializerSettings)
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
        /// Gets the default Json serializer settings.
        /// </summary>
        public static JsonSerializerSettings DefaultJsonSerializerSettings => _defaultJsonSerializerSettings;
        /// <summary>
        /// Gets the Json serializer settings specified during the initialization.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings => _jsonSerializerSettings;
        /// <summary>
        /// Gets the log model template resolver that was set during initialization.
        /// </summary>
        public override ILogModelTemplateResolver LogModelTemplateResolver => _logModelTemplateResolver;
        /// <summary>
        /// Gets the JSON serializer facade that was specified during the initialization.
        /// </summary>
        public IJsonSerializerFacade JsonSerializerFacade => _jsonSerializerFacade;


        /// <summary>
        /// Replaces the default json serializer facade with the provided one.
        /// </summary>
        /// <param name="jsonSerializerFacade">New json serializer facade.</param>
        public void ResetJsonSerializerFacade(IJsonSerializerFacade jsonSerializerFacade)
        {
            _jsonSerializerFacade = jsonSerializerFacade ?? throw new ArgumentNullException("jsonSerializerFacade");
        }
        public override string ToString()
        {
            return new StringBuilder()
               .AppendLine(this.GetType().FullName)
               .AppendLine($"Template: {Template}")
               .AppendLine($"Default date format: {_defaultDateFormat}")
               .Append($"Json serializer settings: {_jsonSerializerSettings.ToString().AddIndentationToLines()}")
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
