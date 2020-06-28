using System;
using TacitusLogger.Components.TemplateResolving;
using TacitusLogger.Exceptions;

namespace TacitusLogger.Serializers
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TemplateLogSerializerBase : ILogSerializer
    {
        private readonly string _template;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public TemplateLogSerializerBase(string template)
        {
            _template = template ?? throw new ArgumentNullException("template");
        }

             
        /// <summary>
        /// Gets the template specified during the initialization.
        /// </summary>
        public string Template => _template;
        /// <summary>
        /// Gets the log model template resolver that was set during initialization.
        /// </summary>
        public abstract ILogModelTemplateResolver LogModelTemplateResolver { get; }
 

        /// <summary>
        /// Serializes provided <paramref name="logModel"/> to string using specified template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If specified <paramref name="logModel"/> is null.</exception>
        /// <param name="logModel">Log model to be serialized.</param>
        /// <returns>Resulting string.</returns>
        public string Serialize(LogModel logModel)
        {
            try
            {
                return LogModelTemplateResolver.Resolve(logModel, _template);
            }
            catch(Exception ex)
            {
                throw new LogSerializerException("Error when resolving provided log model using specified template. See the inner exception.", ex);
            }
        }
        public virtual void Dispose()
        {

        }
    }
}
