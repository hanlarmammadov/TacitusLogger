using System; 
using System.Text;
using System.Text.RegularExpressions; 

namespace TacitusLogger.Components.TemplateResolving.PlaceholderResolvers
{
    /// <summary>
    /// Used to resolve $Tags placeholder from the template string using log model.
    /// </summary>
    public class TagsPlaceholderResolver : IPlaceholderResolver
    {
        private readonly Regex _mainRegex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.PlaceholderResolvers.TagsPlaceholderResolver</c>.
        /// </summary> 
        public TagsPlaceholderResolver()
        {
            _mainRegex = new Regex(@"(\$Tags).*?");
        }

        /// <summary>
        /// Gets the object representing regular expression used for this resolver. For testing purposes. 
        /// </summary>
        internal Regex MainRegex => _mainRegex;

        /// <summary>
        /// Finds all occurrences of $Tags placeholder and replaces them with space separated list of tags.
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
                    stringRepresenation = GetStringRepresentation(logModel.Tags, " ");
                return stringRepresenation;
            });
        }
        private string GetStringRepresentation(string[] tags, string separator)
        {
            if (tags == null || tags.Length == 0)
                return "";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(tags[0]);
            for (int i = 1; i < tags.Length; i++) 
                stringBuilder.Append(separator).Append(tags[i]);

            return stringBuilder.ToString();
        }
    }
}
