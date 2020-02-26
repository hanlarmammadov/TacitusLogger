using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used as a base class for all placeholders which could contain substring length.
    /// </summary>
    public abstract class PlaceholderWithSubstringResolverBase
    {
        private readonly Regex _numberInParFormatRegex = new Regex(@"(?:\(){1}([\d]+)(?:\)){1}");

        /// <summary>
        /// Extracts the substring length information from the template, and if exists, returns the substring of value
        /// of that length, otherwise, returns the full value string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueFromTemplate"></param>
        /// <returns></returns>
        protected string GetFullStringOrSubstring(string value, string valueFromTemplate)
        {
            if (value == null)
                return string.Empty;

            string result = null;
            string substringLengthStr = null;
            var substringLengthMatch = _numberInParFormatRegex.Match(valueFromTemplate);
            if (substringLengthMatch.Success)
            {
                substringLengthStr = substringLengthMatch.Value.Substring(1, substringLengthMatch.Value.Length - 2);
                int substringLength = int.Parse(substringLengthStr);
                result = (substringLength < value.Length) ? value.Substring(0, substringLength) : value;
            }
            else
                result = value;
            return result;
        }
    }
}
