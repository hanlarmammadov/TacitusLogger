using System; 
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $LogId placeholder from the template string using log model.
    /// </summary>
    public class LogIdPlaceholderResolver : PlaceholderWithSubstringResolverBase, IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.LogIdPlaceholderResolver</c>
        /// </summary>
        public LogIdPlaceholderResolver()
        {
            _mainRegex = new Regex(@"((\$LogId\([\d]+\))|(\$LogId)).*?");
        }

        /// <summary>
        /// Finds all occurrences of $LogId placeholder and replaces them with 
        /// the value of LogId property of log model or with its substring.
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
                return GetFullStringOrSubstring(logModel.LogId, match.Value);
            });
        }
    }
}
