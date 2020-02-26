using System; 
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $LogDate placeholder from the template string using log model.
    /// </summary>
    public class LogDatePlaceholderResolver : PlaceholderWithDateFormatResolverBase, IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.LogDatePlaceholderResolver</c>
        /// using default date format string.
        /// </summary>
        /// <param name="defaultDateFormat">Will be used when $LogDate placeholder is given without date format.</param>
        public LogDatePlaceholderResolver(string defaultDateFormat)
            : base(defaultDateFormat)
        {
            _mainRegex = new Regex(@"((\$LogDate\([dDmMyYsSHhtTfGMTK,\/\\':\.\-\s]+\))|(\$LogDate)).*?");
        }

        /// <summary>
        /// Finds all occurrences of $LogDate placeholder and replaces them with 
        /// the string representation of LogDate property of log model.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <param name="template">The given template.</param>
        public void Resolve(LogModel logModel, ref string template)
        {
            if (logModel == null)
                throw new ArgumentNullException("logModel");
            if (template == null)
                throw new ArgumentNullException("template");

            template = _mainRegex.Replace(template, (match) =>
            {
                return GetDateString(logModel.LogDate, match.Value);
            });
        }
    }
}
