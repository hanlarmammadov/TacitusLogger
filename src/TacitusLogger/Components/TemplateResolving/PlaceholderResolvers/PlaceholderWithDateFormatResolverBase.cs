using System;
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used as a base class for all placeholders which contain date format.
    /// </summary>
    public abstract class PlaceholderWithDateFormatResolverBase
    {
        private readonly Regex _dateFormatRegex;
        private readonly string _defaultDateFormat;

        /// <summary>
        /// Initializes the base class <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.PlaceholderWithDateFormatResolverBase</c>
        /// using default date format string.
        /// </summary>
        /// <param name="defaultDateFormat">Default date format.</param>
        protected PlaceholderWithDateFormatResolverBase(string defaultDateFormat)
        {
            _dateFormatRegex = new Regex(@"(?:\(){1}([dDmMyYsSHhtTfGMTK,\/\\':\.\-\s]+)(?:\)){1}");
            _defaultDateFormat = defaultDateFormat ?? throw new ArgumentNullException("defaultDateFormat");
        }

        /// <summary>
        /// Gets the default date format string specified during the initialization.
        /// </summary>
        internal string DefaultDateFormat => _defaultDateFormat;

        /// <summary>
        /// Extracts the date format from the template, and if exists returns the date string representation
        /// using that format, otherwise, uses the default date format.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueFromTemplate"></param>
        /// <returns></returns>
        protected string GetDateString(DateTime value, string valueFromTemplate)
        {
            string formatString = null;
            var dateFormatMatch = _dateFormatRegex.Match(valueFromTemplate);
            if (dateFormatMatch.Success)
                formatString = dateFormatMatch.Value.Substring(1, dateFormatMatch.Value.Length - 2);
            else
                formatString = _defaultDateFormat;

            return value.ToString(formatString);
        }
    }
}
