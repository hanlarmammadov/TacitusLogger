using System;
using System.Collections.Generic;
using System.ComponentModel;
using TacitusLogger.Destinations.Console;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds the file log destination and adds it to own log group destination builder.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ConsoleDestinationBuilder : LogDestinationBuilderBase, IConsoleDestinationBuilder
    {
        private IDictionary<LogType, ConsoleColor> _colorScheme;
        private ILogSerializer _logSerializer;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.ConsoleDestinationBuilder</c> using its parent 
        /// log group destination builder.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder">Parent log group destinations builder that will be used to complete build process.</param>
        internal ConsoleDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
            : base(logGroupDestinationsBuilder)
        {

        }

        /// <summary>
        /// Gets the log serializer specified during the build.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the color scheme specified during the build.
        /// </summary>
        public IDictionary<LogType, ConsoleColor> ColorScheme => _colorScheme;

        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate log model text representation. If omitted, the default log serializer
        /// of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="logSerializer">Log serializer.</param>
        /// <returns>Self.</returns>
        public IConsoleDestinationBuilder WithLogSerializer(ILogSerializer logSerializer)
        {
            if (_logSerializer != null)
                throw new InvalidOperationException("Log serializer has already been set");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            return this;
        }
        /// <summary>
        /// Specifies the custom color scheme that will be used to paint log messages of different log types
        /// to different colors. If omitted, the default color scheme will be used.
        /// </summary>
        /// <param name="colorScheme">The color scheme containing console colors for each log type.</param>
        /// <returns>Self.</returns>
        public IConsoleDestinationBuilder WithCustomColors(IDictionary<LogType, ConsoleColor> colorScheme)
        {
            if (_colorScheme != null)
                throw new InvalidOperationException("Color scheme has already been set");
            _colorScheme = colorScheme ?? throw new ArgumentNullException("colorScheme");
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
            if (_colorScheme == null)
                _colorScheme = ConsoleDestination.GetDefaultColorScheme();

            return AddToLogGroup(new ConsoleDestination(_logSerializer, _colorScheme));
        }
    }
}
