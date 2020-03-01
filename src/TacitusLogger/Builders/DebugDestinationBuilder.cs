using System;
using System.ComponentModel;
using TacitusLogger.Destinations.Debug;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds the debug log destination and adds it to own log group destination builder.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DebugDestinationBuilder : LogDestinationBuilderBase, IDebugDestinationBuilder
    {
        private ILogSerializer _logSerializer;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.DebugDestinationBuilder</c> using its parent 
        /// log group destination builder.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder">Parent log group destinations builder that will be used to complete build process.</param>
        internal DebugDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
            : base(logGroupDestinationsBuilder)
        {

        }

        /// <summary>
        /// Gets the log serializer specified during the build.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;

        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate log model text representation. If omitted, the default log serializer
        /// of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="logSerializer">Log serializer.</param>
        /// <returns>Self.</returns>
        public IDebugDestinationBuilder WithLogSerializer(ILogSerializer logSerializer)
        {
            if (_logSerializer != null)
                throw new InvalidOperationException("Log serializer has already been set");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            return this;
        }
        /// <summary>
        /// Completes log destination build process by adding it to the parent log group destination builder.
        /// </summary>
        /// <returns>The parent log group destination builder.</returns>
        public ILogGroupDestinationsBuilder Add()
        {
            //Set defaults if nulls
            if (_logSerializer == null)
                _logSerializer = new SimpleTemplateLogSerializer();

            return AddToLogGroup(new DebugDestination(_logSerializer));
        }
    }
}
