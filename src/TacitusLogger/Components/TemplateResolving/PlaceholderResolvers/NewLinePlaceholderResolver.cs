using System; 
using System.Text.RegularExpressions;

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $NewLine placeholder from the template string using log model.
    /// </summary>
    public class NewLinePlaceholderResolver : IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.NewLinePlaceholderResolver</c>
        /// </summary>
        public NewLinePlaceholderResolver()
        {
            _mainRegex = new Regex(@"(\$NewLine).*?");
        }

        /// <summary>
        /// Finds all occurrences of $NewLine placeholder in the template and replaces them with 
        /// the platform-specific new line string.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <param name="template">The given template.</param>
        public void Resolve(LogModel logModel, ref string template)
        { 
            if (template == null)
                throw new ArgumentNullException("template");

            template = _mainRegex.Replace(template, (match) =>
            {
                return Environment.NewLine;
            });
        }
    }
}
