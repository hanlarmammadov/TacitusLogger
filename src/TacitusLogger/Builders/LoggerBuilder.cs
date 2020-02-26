using System;
using System.Collections.Generic;
using TacitusLogger.Destinations;
using TacitusLogger.Contributors;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;
using TacitusLogger.Transformers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds an instance of <c>TacitusLogger.Logger</c> which implements the <c>TacitusLogger.ILogger</c> interface.
    /// </summary>
    public class LoggerBuilder : ILoggerBuilder
    {
        private string _loggerName;
        private ILogIdGenerator _logIdGenerator;
        private List<LogGroupBase> _logGroups;
        private IList<LogContributorBase> _logContributors;
        private IList<LogTransformerBase> _logTransformers;
        private LogCreationStrategyBase _logCreationStrategy;
        private ExceptionHandlingStrategyBase _exceptionHandlingStrategy;
        private Setting<LogLevel> _logLevel;
        private ILogDestination _selfMonitoringDestination;
        private bool _recordConfigurationAfterBuild;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.LoggerBuilder</c>.
        /// </summary>
        protected LoggerBuilder(string name)
        {
            _logGroups = new List<LogGroupBase>();
            _logContributors = new List<LogContributorBase>();
            _logTransformers = new List<LogTransformerBase>();
            _loggerName = name ?? TacitusLogger.Logger.GenerateDefaultLoggerName();
            _recordConfigurationAfterBuild = true;
        }

        /// <summary>
        /// Gets the logger name specified during the build.
        /// </summary>
        public string LoggerName => _loggerName;
        /// <summary>
        /// Gets the log id generator specified during the build.
        /// </summary>
        public ILogIdGenerator LogIdGenerator => _logIdGenerator;
        /// <summary>
        /// Gets the log groups added to this logger builder.
        /// </summary>
        public List<LogGroupBase> LogGroups => _logGroups;
        /// <summary>
        /// Gets the log contributors added to this logger builder.
        /// </summary>
        public IList<LogContributorBase> LogContributors => _logContributors;
        /// <summary>
        /// Gets the log transformers added to this logger builder.
        /// </summary>
        public IList<LogTransformerBase> LogTransformers => _logTransformers;
        /// <summary>
        /// 
        /// </summary>
        public LogCreationStrategyBase LogCreationStrategy => _logCreationStrategy;
        /// <summary>
        /// 
        /// </summary>
        public ExceptionHandlingStrategyBase ExceptionHandlingStrategy => _exceptionHandlingStrategy;
        /// <summary>
        /// 
        /// </summary>
        public Setting<LogLevel> LogLevel => _logLevel;
        /// <summary>
        /// 
        /// </summary>
        public ILogDestination SelfMonitoringDestination => _selfMonitoringDestination;
        /// <summary>
        /// 
        /// </summary>
        public bool RecordConfigurationAfterBuild => _recordConfigurationAfterBuild;

        /// <summary>
        /// Begins the logger building steps by creating and returning a new logger builder instance.
        /// </summary>
        /// <returns>A new instance of <c>TacitusLogger.Builders.LoggerBuilder</c>.</returns>
        public static ILoggerBuilder Logger()
        {
            return new LoggerBuilder(name: null);
        }
        /// <summary>
        /// Begins the logger building steps by creating and returning a new logger builder instance with
        /// specified name. This name will appear in log model as Source property.
        /// </summary>
        /// <param name="name">Name for this logger.</param>
        /// <returns>A new instance of <c>TacitusLogger.Builders.LoggerBuilder</c>.</returns>
        public static ILoggerBuilder Logger(string name)
        {
            return new LoggerBuilder(name);
        }
        /// <summary>
        /// Sets the minimal log level. All logs that have log type
        /// less than this level will be ignored by the logger.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <returns></returns>
        public ILoggerBuilder WithLogLevel(Setting<LogLevel> logLevel)
        {
            _logLevel = logLevel ?? throw new ArgumentNullException("logLevel");
            return this;
        }
        /// <summary>
        /// Add a log id generator to the logger builder.
        /// </summary>
        /// <param name="logIdGenerator">Log id generator that will be used to generate ids for new logs.</param>
        /// <returns>Self.</returns>
        public ILoggerBuilder WithLogIdGenerator(ILogIdGenerator logIdGenerator)
        {
            _logIdGenerator = logIdGenerator ?? throw new ArgumentNullException("logIdGenerator");
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logCreationStrategy"></param>
        /// <returns>Self.</returns>
        public ILoggerBuilder WithLogCreation(LogCreationStrategyBase logCreationStrategy)
        {
            _logCreationStrategy = logCreationStrategy ?? throw new ArgumentNullException("logCreationStrategy");
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionHandlingStrategy"></param>
        /// <returns>Self.</returns>
        public ILoggerBuilder WithExceptionHandling(ExceptionHandlingStrategyBase exceptionHandlingStrategy)
        {
            _exceptionHandlingStrategy = exceptionHandlingStrategy ?? throw new ArgumentNullException("exceptionHandlingStrategy");
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selfMonitoringDestination"></param>
        /// <returns>Self.</returns>
        public ILoggerBuilder WithSelfMonitoring(ILogDestination selfMonitoringDestination)
        {
            if (_selfMonitoringDestination != null)
                throw new InvalidOperationException("Self monitoring destination has already been set during this build");
            if (selfMonitoringDestination == null)
                throw new ArgumentNullException("selfMonitoringDestination");
            _selfMonitoringDestination = selfMonitoringDestination;
            return this;
        }
        /// <summary>
        /// Begins log contributors building steps by creating and returning a new instance of <c>TacitusLogger.Builders.ILogContributorsBuilder</c>
        /// Log contributors are used to add arbitrary data in the form of log attachments to the log model.
        /// </summary>
        /// <returns>Log contributors builder.</returns>
        public ILogContributorsBuilder Contributors()
        {
            return new LogContributorsBuilder(LogContributorsBuildCallback);
        }
        /// <summary>
        /// Begins log transformers building steps by creating and returning a new instance of <c>TacitusLogger.Builders.ILogTransformersBuilder</c>
        /// Log transformers are used to modify log models before they are sent to log groups.
        /// </summary>
        /// <returns>Log transformers builder.</returns>
        public ILogTransformersBuilder Transformers()
        {
            return new LogTransformersBuilder(LogTransformersBuildCallback);
        }
        /// <summary>
        /// Begins log group building steps by creating and returning a new instance of <c>TacitusLogger.Builders.ILogGroupBuilder</c>
        /// with the specified log group name.
        /// </summary>
        /// <param name="logGroupName">The name of the building log group.</param>
        /// <returns>A new log group builder.</returns>
        public ILogGroupBuilder NewLogGroup(string logGroupName)
        {
            if (logGroupName == null)
                throw new ArgumentNullException("logGroupName");
            return new LogGroupBuilder(logGroupName, this.NewLogGroup);
        }
        /// <summary>
        /// Begins log group building steps by creating and returning a new instance of <c>ILogGroupBuilder.TacitusLogger.Builders</c>.
        /// </summary> 
        /// <returns></returns>
        public ILogGroupBuilder NewLogGroup()
        {
            return new LogGroupBuilder(null, this.NewLogGroup);
        }
        public ILoggerBuilder WriteLoggerConfigurationToDiagnostics(bool should)
        {
            _recordConfigurationAfterBuild = should;
            return this;
        }
        /// <summary>
        /// Finishes the build process by building a ready-to-use instance of the <c>TacitusLogger.ILogger</c> interface.
        /// </summary>
        /// <returns>Built logger.</returns>
        public virtual Logger BuildLogger()
        {
            // Execute pre-build action.
            PreBuildAction();

            // Set defaults if null.
            if (_logIdGenerator == null)
                _logIdGenerator = Defaults.LogIdGenerator;
            if (_logCreationStrategy == null)
                _logCreationStrategy = new LogCreationStrategyFactory().GetStrategy(Defaults.LogCreationStrategy);
            if (_exceptionHandlingStrategy == null)
                _exceptionHandlingStrategy = new ExceptionHandlingStrategyFactory().GetStrategy(Defaults.ExceptionHandlingStrategy);

            // Create logger class.
            Logger logger = new Logger(_loggerName, _logIdGenerator, new LoggerSettings()
            {
                LogLevel = _logLevel ?? new MutableSetting<LogLevel>(Defaults.LogLevel)
            });

            // Log creation strategy
            logger.ResetLogCreationStrategy(_logCreationStrategy);

            // Self monitoring destination
            if (_selfMonitoringDestination != null)
                logger.SetSelfMonitoringDestination(_selfMonitoringDestination);

            // Exception handling strategy
            logger.ResetExceptionHandlingStrategy(_exceptionHandlingStrategy);

            // Add log groups log the logger.
            for (int i = 0; i < _logGroups.Count; i++)
                logger.AddLogGroup(_logGroups[i]);

            // Add log contributors to the logger.
            for (int i = 0; i < _logContributors.Count; i++)
                logger.AddLogContributor(_logContributors[i]);

            // Add log transformers to the logger.
            for (int i = 0; i < _logTransformers.Count; i++)
                logger.AddLogTransformer(_logTransformers[i]);

            // Diagnostics manager

            // Execute post build action.
            PostBuildAction(logger);

            //
            if (_recordConfigurationAfterBuild)
                logger.WriteLoggerConfigurationToDiagnostics();

            // Return the created logger.
            return logger;
        }
        /// <summary>
        /// Used to add an instance of <c>TacitusLogger.ILogGroup</c> to the logger builder.
        /// </summary>
        /// <param name="logGroup">Log group.</param>
        public ILoggerBuilder NewLogGroup(LogGroupBase logGroup)
        {
            if (logGroup == null)
                throw new ArgumentNullException("logGroup");
            _logGroups.Add(logGroup);
            return this;
        }
        /// <summary>
        /// Finishes the log contributors building. 
        /// </summary>
        /// <param name="logContributors"></param>
        /// <returns></returns>
        internal ILoggerBuilder LogContributorsBuildCallback(IList<LogContributorBase> logContributors)
        {
            _logContributors = logContributors;
            return this;
        }
        /// <summary>
        /// Finishes the log transformers building. 
        /// </summary>
        /// <param name="logTransformers"></param>
        /// <returns></returns>
        internal ILoggerBuilder LogTransformersBuildCallback(IList<LogTransformerBase> logTransformers)
        {
            _logTransformers = logTransformers;
            return this;
        }
        protected virtual void PreBuildAction()
        {

        }
        protected virtual void PostBuildAction(Logger logger)
        {

        }

    }
}
