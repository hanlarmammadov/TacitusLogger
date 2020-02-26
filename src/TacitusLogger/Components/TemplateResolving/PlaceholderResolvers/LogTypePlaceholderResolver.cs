using System; 
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $LogType placeholder from the template string using log model.
    /// </summary>
    public class LogTypePlaceholderResolver : PlaceholderWithSubstringResolverBase, IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.LogTypePlaceholderResolver</c>
        /// </summary>
        public LogTypePlaceholderResolver()
        {
            _mainRegex = new Regex(@"((\$LogType\([\d]+\))|(\$LogType)).*?");
        }

        /// <summary>
        /// Finds all occurrences of $LogType placeholder in the template and replaces them with 
        /// the text representation of LogType property of log model or with its substring.
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
                return GetFullStringOrSubstring(Enum.GetName(typeof(LogType), logModel.LogType), match.Value);
            });
        }
    }
}
