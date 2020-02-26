using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $Description placeholder from the template string using log model.
    /// </summary>
    public class DescriptionPlaceholderResolver : PlaceholderWithSubstringResolverBase, IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.DescriptionPlaceholderResolver</c>
        /// </summary>
        public DescriptionPlaceholderResolver()
        {
            _mainRegex = new Regex(@"((\$Description\([\d]+\))|(\$Description)).*?");
        }

        /// <summary>
        /// Finds all occurrences of $Description placeholder and replaces them with 
        /// the value of Description property of log model or with its substring.
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
                return GetFullStringOrSubstring(logModel.Description, match.Value);
            });
        }
    }
}
