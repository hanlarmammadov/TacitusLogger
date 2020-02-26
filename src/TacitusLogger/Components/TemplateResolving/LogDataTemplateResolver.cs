using System;
using System.Collections.Generic;

namespace TacitusLogger.Components.TemplateResolving
{
    /// <summary>
    /// Used to create strings representation of log model using the template string
    /// and the list of placeholder resolvers of type <c>TacitusLogger.Components.TemplateResolving.IPlaceholderResolver</c>
    /// </summary>
    public class LogModelTemplateResolver : ILogModelTemplateResolver
    {
        private readonly IList<IPlaceholderResolver> _placeholderResolvers;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Components.TemplateResolving.LogModelTemplateResolver</c> using
        /// specified list of placeholder resolvers.
        /// </summary>
        /// <param name="placeholderResolvers">The list of placeholder resolvers.</param>
        public LogModelTemplateResolver(IList<IPlaceholderResolver> placeholderResolvers)
        {
            _placeholderResolvers = placeholderResolvers ?? throw new ArgumentNullException("placeholderResolvers");
        }

        /// <summary>
        /// Gets placeholder resolvers list specified during initialization.
        /// </summary>
        public IList<IPlaceholderResolver> PlaceholderResolvers => _placeholderResolvers;

        /// <summary>
        /// Creates a serialized string using provided log model and template string.
        /// </summary>
        /// <param name="logModel">Log model</param>
        /// <param name="template">Template string</param>
        /// <returns>Created string.</returns>
        public string Resolve(LogModel logModel, string template)
        {
            for (int i = 0; i < _placeholderResolvers.Count; i++)
                _placeholderResolvers[i].Resolve(logModel, ref template);
            return template;
        } 
    }
}
