using System;
using System.Collections.Generic;
using System.ComponentModel; 
using TacitusLogger.Destinations;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Setups the log destinations of the given log group.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LogGroupDestinationsBuilder: ILogGroupDestinationsBuilder
    {
        private readonly Func<List<ILogDestination>, ILoggerBuilder> _buildCallback;
        private readonly List<ILogDestination> _logDestinations;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.LogGroupDestinationsBuilder</c> using specified
        /// log group builder.
        /// </summary>
        /// <param name="logGroupBuilder">the log group builder.</param>
        public LogGroupDestinationsBuilder(Func<List<ILogDestination>, ILoggerBuilder> buildCallback)
        {
            _logDestinations = new List<ILogDestination>();
            _buildCallback = buildCallback;
        }

        /// <summary>
        /// Gets the build callback that was specified during the initialization.
        /// </summary>
        public Func<List<ILogDestination>, ILoggerBuilder> BuildCallback => _buildCallback;
        /// <summary>
        /// Gets the list of log destinations that was added to this builder.
        /// </summary>
        public List<ILogDestination> LogDestinations => _logDestinations;

        /// <summary>
        /// Adds a custom log destination to the builder.
        /// </summary>
        /// <param name="logDestination">Log destination to add.</param>
        /// <returns></returns>
        public ILogGroupDestinationsBuilder CustomDestination(ILogDestination logDestination)
        {
            if (logDestination == null)
                throw new ArgumentNullException("logDestination");
            _logDestinations.Add(logDestination);
            return this;
        }
        /// <summary>
        /// Creates the log group an add it to the logger builder.
        /// </summary>
        /// <returns></returns>
        public ILoggerBuilder BuildLogGroup()
        {
            return _buildCallback(_logDestinations); 
        } 
    }
}
