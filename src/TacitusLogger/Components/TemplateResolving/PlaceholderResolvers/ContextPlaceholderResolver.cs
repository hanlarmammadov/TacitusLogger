using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $Context placeholder from the template string using log model.
    /// </summary>
    public class ContextPlaceholderResolver : PlaceholderWithSubstringResolverBase, IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.ContextPlaceholderResolver</c>
        /// </summary>
        public ContextPlaceholderResolver()
        {
            _mainRegex = new Regex(@"((\$Context\([\d]+\))|(\$Context)).*?");
        }

        /// <summary>
        /// Finds all occurrences of $Context placeholder and replaces them with 
        /// the value of Context property of log model or with its substring.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <param name="template">The given template.</param>
        public void Resolve(LogModel logModel, ref string template)
        {
            if (logModel == null)
                throw new ArgumentNullException("logModel");
            if (template==null)
                throw new ArgumentNullException("template");

            template = _mainRegex.Replace(template, (match) =>
            {
                return GetFullStringOrSubstring(logModel.Context, match.Value);
            });
        }
    }
}
