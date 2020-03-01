using System;
using System.Collections.Generic;
using System.ComponentModel;
using TacitusLogger.Destinations;
using TacitusLogger.Caching;
using TacitusLogger.Strategies.DestinationFeeding;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds a log group for the given logger builder.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LogGroupBuilder : ILogGroupBuilder
    {
        private readonly Func<LogGroupBase, ILoggerBuilder> _buildCallback;
        private LogModelFunc<bool> _rule;
        private string _name;
        private Setting<LogGroupStatus> _status;
        private ILogCache _logCache;
        private bool _cacheIsActive;
        private DestinationFeedingStrategyBase _destinationFeedingStrategy;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.LogGroupBuilder</c> using log group name
        /// and logger builder instance.
        /// </summary>
        /// <param name="logGroupName">Log group name</param>
        /// <param name="buildCallback">The build callback delegate used to complete the log group building process.</param>
        public LogGroupBuilder(string logGroupName, Func<LogGroupBase, ILoggerBuilder> buildCallback)
        {
            _name = logGroupName;
            _status = LogGroupStatus.Active;
            _cacheIsActive = false;
            _buildCallback = buildCallback;
        }

        /// <summary>
        /// Gets an instance of build callback specified during the initialization.
        /// </summary>
        public Func<LogGroupBase, ILoggerBuilder> BuildCallback => _buildCallback;
        /// <summary>
        /// Get the log group rule specified during the build.
        /// </summary>
        public LogModelFunc<bool> Rule => _rule;
        /// <summary>
        /// Gets the log group name specified during the initialization.
        /// </summary>
        public string Name => _name;
        /// <summary>
        /// Gets if the log group is marked an active or not.
        /// </summary>
        public LogGroupStatus LogGroupStatus => _status.Value;
        /// <summary>
        /// Gets the log cache specified during the build.
        /// </summary>
        public ILogCache LogCache => _logCache;
        /// <summary>
        /// 
        /// </summary>
        public bool CachingIsActive => _cacheIsActive;
        /// <summary>
        /// Gets the destination feeding strategy specified during the build.
        /// </summary>
        public DestinationFeedingStrategyBase DestinationFeedingStrategy => _destinationFeedingStrategy;

        /// <summary>
        /// Sets the log group rule predicate of type <c>TacitusLogger.LogModelPredicate</c> that will be used to 
        /// decide whether or not the given log is eligible to be send to this log group.
        /// </summary>
        /// <param name="rule">The log group rule predicate.</param>
        /// <returns>Log group destination builder that is used to setups the log destinations of the given log group.</returns>
        public ILogGroupDestinationsBuilder ForRule(LogModelFunc<bool> rule)
        {
            if (_rule != null)
                throw new InvalidOperationException("The rule was already been set during the build");
            if (rule == null)
                throw new ArgumentNullException("rule");
            _rule = rule;
            return new LogGroupDestinationsBuilder(CreateLogGroup);
        }
        /// <summary>
        /// Sets if the log group is active or not. Only active groups are used by logger to save 
        /// logs. If omitted, the log group is considered active by default.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>Self.</returns>
        public ILogGroupBuilder SetStatus(Setting<LogGroupStatus> status)
        {
            _status = status ?? throw new ArgumentNullException("status");
            return this;
        }
        /// <summary>
        /// Adds logs caching. When enabled, log cache suspends sending logs to destination until some time in future depending
        /// on its implementation logic.
        /// </summary>
        /// <param name="logCache">Log cache.</param>
        /// <returns>Self.</returns>
        public ILogGroupBuilder WithCaching(ILogCache logCache, bool isActive = true)
        {
            if (_logCache != null)
                throw new InvalidOperationException("Log cache has already been set during the build");
            if (logCache == null)
                throw new ArgumentNullException("logCache");
            _logCache = logCache;
            _cacheIsActive = isActive;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationFeedingStrategy"></param>
        /// <returns>Self.</returns>
        public ILogGroupBuilder WithDestinationFeeding(DestinationFeedingStrategyBase destinationFeedingStrategy)
        {
            if (_destinationFeedingStrategy != null)
                throw new InvalidOperationException("Destination feeding strategy has already been set for the building log group");
            _destinationFeedingStrategy = destinationFeedingStrategy ?? throw new ArgumentNullException("destinationFeedingStrategy");
            return this;
        }
        /// <summary>
        /// Creates the log group using specified log destinations, adds the created log 
        /// group to logger builder and returns it.
        /// </summary>
        /// <param name="logDestinations">Log destinations list.</param>
        /// <returns>Logger builder.</returns>
        internal ILoggerBuilder CreateLogGroup(List<ILogDestination> logDestinations)
        {
            LogModelFunc<bool> rule = (_rule != null) ? _rule : (ld) => true;
            logDestinations = (logDestinations != null) ? logDestinations : new List<ILogDestination>();
            if (_name == null)
                _name = LogGroup.GenerateGroupName();

            // Create the log group.  
            LogGroup logGroup = new LogGroup(_name, rule, logDestinations);

            // If destination feeding strategy has been specified, reset the default one in the log group.
            if (_destinationFeedingStrategy != null)
                logGroup.ResetDestinationFeedingStrategy(_destinationFeedingStrategy);
            // Set active status.
            logGroup.SetStatus(_status);
            // Set caching if specified.
            if (_logCache != null)
                logGroup.SetLogCache(_logCache, _cacheIsActive);
            // Call the build callback specified by parent.
            return _buildCallback(logGroup);
        }
    }
}
