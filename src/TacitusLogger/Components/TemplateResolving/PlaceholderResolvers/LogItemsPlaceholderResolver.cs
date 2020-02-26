using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
using TacitusLogger.Components.Json;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $LogItems placeholder from the template string using log model.
    /// </summary>
    public class LogItemsPlaceholderResolver : IPlaceholderResolver
    {
        private readonly Regex _mainRegex;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private IJsonSerializerFacade _jsonSerializerFacade;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.LogItemsPlaceholderResolver</c>
        /// using JSON serializer settings.
        /// </summary>
        /// <param name="jsonSerializerFacade">JSON serializer facade that will be used for serialization.</param>
        /// <param name="jsonSerializerSettings">JSON serializer settings.</param>
        public LogItemsPlaceholderResolver(ref IJsonSerializerFacade jsonSerializerFacade, JsonSerializerSettings jsonSerializerSettings)
        {
            _mainRegex = new Regex(@"(\$LogItems).*?");
            _jsonSerializerSettings = jsonSerializerSettings ?? throw new ArgumentNullException("jsonSerializerSettings");
            _jsonSerializerFacade = jsonSerializerFacade ?? throw new ArgumentNullException("jsonSerializerFacade");
        }

        /// <summary>
        /// Gets the JSON serializer settings specified during the initialization.
        /// </summary>
        internal JsonSerializerSettings JsonSerializerSettings => _jsonSerializerSettings;
        /// <summary>
        /// Gets the JSON serializer facade that was specified during the initialization.
        /// </summary>
        internal IJsonSerializerFacade JsonSerializerFacade => _jsonSerializerFacade;

        /// <summary>
        /// Finds all occurrences of $LoggingObject placeholder and replaces them with 
        /// the JSON representation of LoggingObject property of log model or with its substring.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <param name="template">The given template.</param>
        public void Resolve(LogModel logModel, ref string template)
        {
            if (logModel == null)
                throw new ArgumentNullException("logModel");
            if (template == null)
                throw new ArgumentNullException("template");
            string stringRepresenation = null;
            template = _mainRegex.Replace(template, (match) =>
            {
                if (stringRepresenation == null)
                    stringRepresenation = GetStringRepresentation(logModel.LogItems);
                return stringRepresenation;
            });
        }
        private string GetStringRepresentation(LogItem[] logItems)
        {
            if (logItems == null || logItems.Length == 0)
                return "";

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < logItems.Length; i++)
            {
                // Item value string representation.
                string itemValueStr = null;
                if (logItems[i].Value != null)
                    if (logItems[i].Value is string)
                        itemValueStr = (string)logItems[i].Value;
                    else
                        itemValueStr = _jsonSerializerFacade.Serialize(logItems[i].Value, _jsonSerializerSettings);
                else
                    itemValueStr = "null";
                // Item name string representation.
                string itemNameStr;
                if (logItems[i].Name != null)
                    itemNameStr = logItems[i].Name;
                else
                    itemNameStr = "Item";

                stringBuilder.AppendLine($"{itemNameStr}: {itemValueStr}");
            }
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - Environment.NewLine.Length, Environment.NewLine.Length);

            return stringBuilder.ToString();
        }
    }
}
