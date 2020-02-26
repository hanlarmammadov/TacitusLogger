using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Contributors;
using System.Threading;
using TacitusLogger.Destinations;
using TacitusLogger.Exceptions;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;
using TacitusLogger.Transformers;
using System.Text;
using TacitusLogger.Components.Strings;
using TacitusLogger.Diagnostics;

namespace TacitusLogger
{
    /// <summary>
    /// Class <c>Logger</c> represents the default implementation of ILogger interface and the main logging class.
    /// </summary>
    public class Logger : ILogger, ILogDestination, IDisposable
    {
        private readonly List<LogGroupBase> _logGroups;
        private readonly string _loggerName;
        private ILogIdGenerator _logIdGenerator;
        private IList<LogContributorBase> _logContributors;
        private LogCreationStrategyBase _logCreationStrategy;
        private ExceptionHandlingStrategyBase _exceptionHandlingStrategy;
        private LoggerSettings _loggerSettings;
        private ILogDestination _diagnosticsDestination;
        private Setting<LogLevel> _logLevel;
        private IList<LogTransformerBase> _logTransformers;
        private DiagnosticsManagerBase _diagnosticsManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logIdGenerator"></param>
        /// <param name="loggerSettings"></param>
        public Logger(string name, ILogIdGenerator logIdGenerator, LoggerSettings loggerSettings)
        {
            _loggerName = name;
            _logIdGenerator = logIdGenerator ?? throw new ArgumentNullException("logIdGenerator");
            _loggerSettings = loggerSettings ?? throw new ArgumentNullException("loggerSettings");
            _logGroups = new List<LogGroupBase>(50);
            _logContributors = new List<LogContributorBase>(50);
            _logTransformers = new List<LogTransformerBase>(50);
            ResetDiagnosticsManager(new DiagnosticsManager());
            ResetLogCreationStrategy(new LogCreationStrategyFactory().GetStrategy(loggerSettings.LogCreation));
            ResetExceptionHandlingStrategy(new ExceptionHandlingStrategyFactory().GetStrategy(loggerSettings.ExceptionHandling));
            _logLevel = loggerSettings.LogLevel ?? new MutableSetting<LogLevel>(Defaults.LogLevel);
        }
        /// <summary>
        /// Creates a new instance of the Logger class with the provided logger name and ILogIdGenerator instance.
        /// </summary>
        /// <param name="name">Name for this logger. It will appear in log models as Source property.</param>
        /// <param name="logIdGenerator">Will be used to generate ids for created logs</param>
        public Logger(string name, ILogIdGenerator logIdGenerator)
            : this(name, logIdGenerator, Defaults.LoggerSettings)
        {

        }
        /// <summary> 
        /// Creates a new instance of the Logger class with the provided logger name and default log id generator.  
        /// </summary>
        /// <param name="name">Name for this logger. It will appear in log models as Source property.</param>
        /// <param name="loggerSettings"></param>
        public Logger(string name, LoggerSettings loggerSettings)
           : this(name, new GuidLogIdGenerator(), loggerSettings)
        {

        }
        /// <summary> 
        /// Creates a new instance of the Logger class with the provided logger name and default log id generator.  
        /// </summary>
        /// <param name="name">Name for this logger. It will appear in log models as Source property.</param>
        public Logger(string name)
           : this(name, new GuidLogIdGenerator(), Defaults.LoggerSettings)
        {

        }
        /// <summary>
        /// Creates a new instance of Logger class with default settings.
        /// </summary>
        public Logger()
            : this(GenerateDefaultLoggerName(), new GuidLogIdGenerator(), Defaults.LoggerSettings)
        {

        }

        /// <summary>
        /// Implementation of ILogIdGenerator registered in this logger.
        /// </summary>
        public ILogIdGenerator LogIdGenerator => _logIdGenerator;
        /// <summary>
        /// Logger name specified during the initialization. It will appear in log models as Source property. 
        /// </summary>
        public string Name => _loggerName;
        /// <summary>
        ///  Returns all LogGroup instances registered in this logger.
        /// </summary>
        public IEnumerable<LogGroupBase> LogGroups => _logGroups;
        /// <summary>
        /// Gets the collection of all log contributors registered with this logger.
        /// </summary>
        public IEnumerable<LogContributorBase> LogContributors => _logContributors;
        /// <summary>
        /// Gets the collection of all log transformers registered with this logger.
        /// </summary>
        public IEnumerable<LogTransformerBase> LogTransformers => _logTransformers;
        /// <summary>
        /// Gets the log creation strategy that was specified during the initialization.
        /// </summary>
        public LogCreationStrategyBase LogCreationStrategy => _logCreationStrategy;
        /// <summary>
        /// Gets the exception handling strategy that was specified during the initialization.
        /// </summary>
        public ExceptionHandlingStrategyBase ExceptionHandlingStrategy => _exceptionHandlingStrategy;
        /// <summary>
        /// 
        /// </summary>
        public ILogDestination DiagnosticsDestination => _diagnosticsDestination;
        /// <summary>
        /// 
        /// </summary>
        public DiagnosticsManagerBase DiagnosticsManager => _diagnosticsManager;
        /// <summary>
        /// 
        /// </summary>
        public Setting<LogLevel> LogLevel => _logLevel;
        /// <summary>
        /// Gets the logger settings that was specified during the initialization.
        /// </summary>
        internal LoggerSettings LoggerSettings => _loggerSettings;

        /// <summary>
        /// Adds a new log group to the current logger.
        /// </summary>
        /// <param name="logGroup">log group implementing <c>TacitusLogger.ILogGroup</c> interface</param>
        public void AddLogGroup(LogGroupBase logGroup)
        {
            if (logGroup == null)
                throw new ArgumentNullException("logGroup");

            // Logger name should be unique
            if (_logGroups.SingleOrDefault(x => x.Name == logGroup.Name) != null)
                throw new InvalidOperationException("Log group with this name already added to the logger");

            // Add group to internal collection
            _logGroups.Add(logGroup);
        }
        /// <summary>
        /// Adds a new LogGroup to the current logger.
        /// </summary>
        /// <param name="rule">LogModelFunc<bool> delegate that define rule for this log group.</param>
        /// <returns>Created and added LogGroup instance.</returns>
        public LogGroup NewLogGroup(LogModelFunc<bool> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");
            var logGroup = new LogGroup(rule);
            AddLogGroup(logGroup);
            return logGroup;
        }
        /// <summary>
        /// Finds the log group with the specified name. Returns null if not found.
        /// </summary>
        /// <param name="name">Log group name.</param>
        /// <returns>Log group or null if not found. </returns>
        public LogGroupBase GetLogGroup(string name)
        {
            return _logGroups.FirstOrDefault(x => x.Name == name);
        }
        /// <summary>
        /// Provides the possibility to reset the default log creation strategy with the custom one.
        /// </summary>
        /// <param name="logCreationStrategy">New log creation strategy.</param>
        public void ResetLogCreationStrategy(LogCreationStrategyBase logCreationStrategy)
        {
            if (logCreationStrategy == null)
                throw new ArgumentNullException("logCreationStrategy");
            logCreationStrategy.InitStrategy(_logIdGenerator, _logContributors, _exceptionHandlingStrategy);
            _logCreationStrategy = logCreationStrategy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionHandlingStrategy"></param>
        public void ResetExceptionHandlingStrategy(ExceptionHandlingStrategyBase exceptionHandlingStrategy)
        {
            _exceptionHandlingStrategy = exceptionHandlingStrategy ?? throw new ArgumentNullException("exceptionHandlingStrategy");
            _exceptionHandlingStrategy.SetDiagnosticsManager(_diagnosticsManager);
            // Reset log creation strategy as well.
            _logCreationStrategy.InitStrategy(_logIdGenerator, _logContributors, _exceptionHandlingStrategy);
        }
        /// <summary>
        /// Adds a log contributor of type <c>TacitusLogger.Contributors.LogContributorBase</c> to the logger.
        /// </summary>
        /// <param name="logContributor">Log contributor.</param>
        public void AddLogContributor(LogContributorBase logContributor)
        {
            if (logContributor == null)
                throw new ArgumentNullException("logContributor");
            _logContributors.Add(logContributor);
        }
        /// <summary>
        /// Adds a log transformer of type <c>TacitusLogger.Transformers.LogTransformerBase</c> to the logger.
        /// </summary>
        /// <param name="logContributor">Log transformer.</param>
        public void AddLogTransformer(LogTransformerBase logTransformer)
        {
            if (logTransformer == null)
                throw new ArgumentNullException("logTransformer");
            _logTransformers.Add(logTransformer);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="logDestination"></param>
        public void SetDiagnosticsDestination(ILogDestination diagnosticsDestination)
        {
            _diagnosticsDestination = diagnosticsDestination ?? throw new ArgumentNullException("diagnosticsDestination");
            _diagnosticsManager.SetDependencies(_diagnosticsDestination, _loggerName);
        }
        /// <summary>
        /// Writes a new log. 
        /// </summary>
        /// <param name="context">Log context</param>
        /// <param name="type">Log type</param>
        /// <param name="description">Log description</param>
        /// <param name="loggingObject">An object that contains additional data to saved in log</param>
        /// <returns>Generated log id</returns>
        public virtual string Log(Log log)
        {
            try
            {
                // Check log level
                if ((int)log.Type < (int)_logLevel.Value)
                    return null;

                // Create log model using the specified strategy.
                LogModel logModel = _logCreationStrategy.CreateLogModel(log, _loggerName);

                //Apply active log transformers
                if (_logTransformers.Count != 0)
                    ApplyLogTransformers(logModel);

                // Send to log groups.
                SendLogToLogGroups(logModel);

                // Return log id.
                return logModel.LogId;
            }
            catch (Exception ex)
            {
                var finalEx = new LoggerException("Error when writing log. See the inner exception", ex);
                try
                {
                    _exceptionHandlingStrategy.HandleException(finalEx, "Log method");
                }
                catch { }
                if (_exceptionHandlingStrategy.ShouldRethrow)
                    throw finalEx;
                else return null;
            }
        }
        /// <summary>
        /// Asynchronously writes a new log. 
        /// </summary>
        /// <param name="context">Log context</param>
        /// <param name="type">Log type</param>
        /// <param name="description">Log description</param>
        /// <param name="loggingObject">An object that contains additional data to saved in log</param>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult represents generated log id.</returns>
        public virtual async Task<string> LogAsync(Log log, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Check if operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    await Task<string>.FromCanceled(cancellationToken);

                // Check log level
                if ((int)log.Type < (int)_logLevel.Value)
                    return null;

                // Create log model using the specified strategy.
                LogModel logModel = await _logCreationStrategy.CreateLogModelAsync(log, _loggerName);

                //Apply active log transformers asynchronously.
                if (_logTransformers.Count != 0)
                    await ApplyLogTransformersAsync(logModel);

                // Send to log groups asynchronously.
                await SendLogToLogGroupsAsync(logModel, cancellationToken);

                // Return log id.
                return logModel.LogId;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var finalEx = new LoggerException("Error when writing log. See the inner exception", ex);
                try
                {
                    await _exceptionHandlingStrategy.HandleExceptionAsync(finalEx, "LogAsync method");
                }
                catch { }
                if (_exceptionHandlingStrategy.ShouldRethrow)
                    throw finalEx;
                else return await Task<string>.FromResult(null as string);
            }
        }
        public void Send(LogModel[] logs)
        {
            try
            {
                // Send to log groups.
                for (int i = 0; i < logs.Length; i++)
                    SendLogToLogGroups(logs[i]);
            }
            catch (Exception ex)
            {
                throw new LogDestinationException("Error when sending the log to destination. See the inner exception.", ex);
            }
        }
        public Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled(cancellationToken);
                // Send to log groups asynchronously. 
                List<Task> tasks = new List<Task>(logs.Length);
                for (int i = 0; i < logs.Length; i++)
                    tasks.Add(SendLogToLogGroupsAsync(logs[i], cancellationToken));
                return Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogDestinationException("Error when sending the log to destination. See the inner exception.", ex);
            }
        }
        public void WriteLoggerConfigurationToDiagnostics()
        { 
            var config = this.ToString();
            var log = TacitusLogger.Log.Info("Logger has been configured. See the log item.").With("Configuration", config);
            _diagnosticsManager.WriteToDiagnostics(log);
        }
        public void ResetDiagnosticsManager(DiagnosticsManagerBase diagnosticsManager)
        {
            _diagnosticsManager = diagnosticsManager ?? throw new ArgumentNullException("diagnosticsManager");
            _diagnosticsManager.SetDependencies(_diagnosticsDestination, _loggerName);

            if (_exceptionHandlingStrategy != null)
                _exceptionHandlingStrategy.SetDiagnosticsManager(_diagnosticsManager);
        }
        public void Dispose()
        {
            try
            {
                _logIdGenerator.Dispose();
            }
            catch { }

            try
            {
                if (_diagnosticsDestination != null)
                    _diagnosticsDestination.Dispose();
            }
            catch { }

            try
            {
                _logLevel.Dispose();
            }
            catch { }

            for (int i = 0; i < _logContributors.Count; i++)
                try
                {
                    _logContributors[i].Dispose();
                }
                catch { }

            for (int i = 0; i < _logTransformers.Count; i++)
                try
                {
                    _logTransformers[i].Dispose();
                }
                catch { }

            for (int i = 0; i < _logGroups.Count; i++)
                try
                {
                    _logGroups[i].Dispose();
                }
                catch { }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder()
                .AppendLine(this.GetType().FullName)
                .AppendLine($"Logger name: {_loggerName}")
                .AppendLine($"Log level: {_logLevel.ToString()?.AddIndentationToLines()}")
                .AppendLine($"Log ID generator: {  _logIdGenerator.ToString()?.AddIndentationToLines()}")
                .AppendLine($"Log creation: {_logCreationStrategy.ToString()?.AddIndentationToLines()}")
                .Append($"Exception handling: {_exceptionHandlingStrategy.ToString()?.AddIndentationToLines()}");

            sb.AppendLine().Append($"Diagnostics destination: ");
            if (_diagnosticsDestination != null)
                sb.Append(_diagnosticsDestination.ToString()?.AddIndentationToLines());
            else
                sb.Append("[No destination]");

            sb.AppendLine().Append("Log contributors: ");
            if (_logContributors.Count != 0)
                for (int i = 0; i < _logContributors.Count; i++)
                    sb.AppendLine().Append(Defaults.ToStringIndentation).Append($"{(i + 1).ToString()}. {_logContributors[i].ToString()}".AddIndentationToLines().AddIndentationToLines());
            else
                sb.Append("[No contributors]");

            sb.AppendLine().Append("Log transformers: ");
            if (_logTransformers.Count != 0)
                for (int i = 0; i < _logTransformers.Count; i++)
                    sb.AppendLine().Append(Defaults.ToStringIndentation).Append($"{(i + 1).ToString()}. {_logTransformers[i].ToString()}".AddIndentationToLines().AddIndentationToLines());
            else
                sb.Append("[No transformers]");

            sb.AppendLine().Append("Log groups: ");
            if (_logGroups.Count != 0)
                for (int i = 0; i < _logGroups.Count; i++)
                    sb.AppendLine().Append(Defaults.ToStringIndentation).Append($"{(i + 1).ToString()}. {_logGroups[i].ToString()}".AddIndentationToLines().AddIndentationToLines());
            else
                sb.Append("[No log groups]");

            return sb.ToString();
        }
        internal static string GenerateDefaultLoggerName()
        {
            return "Logger_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        private void SendLogToLogGroups(LogModel log)
        {
            List<Exception> exceptions = null;
            for (int i = 0; i < _logGroups.Count; i++)
                try
                {
                    if (_logGroups[i].Status == LogGroupStatus.Active && _logGroups[i].IsEligible(log))
                        _logGroups[i].Send(log);
                }
                catch (Exception ex)
                {
                    if (exceptions == null)
                        exceptions = new List<Exception>(_logGroups.Count);
                    exceptions.Add(ex);
                }
            if (exceptions != null)
                throw new AggregateException("One or more errors occurred when sending log model to log groups", exceptions);
        }
        private async Task SendLogToLogGroupsAsync(LogModel log, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Exception handling missed 
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);

            // Write to log groups.
            List<Task> tasks = new List<Task>(_logGroups.Count);
            for (int i = 0; i < _logGroups.Count; i++)
                if (_logGroups[i].Status == LogGroupStatus.Active && _logGroups[i].IsEligible(log))
                    tasks.Add(_logGroups[i].SendAsync(log, cancellationToken));

            if (!cancellationToken.IsCancellationRequested)
            {
                Task mainTask = Task.WhenAll(tasks);
                try
                {
                    await mainTask;
                }
                catch (Exception)
                {
                    throw new AggregateException("One or more errors occurred when sending log model to log groups", mainTask.Exception.InnerExceptions);
                }
            }
            else
                await Task.FromCanceled(cancellationToken);
        }
        private void ApplyLogTransformers(LogModel logModel)
        {
            for (int i = 0; i < _logTransformers.Count; i++)
                if (_logTransformers[i].IsActive)
                    try
                    {
                        _logTransformers[i].Transform(logModel);
                    }
                    catch (Exception ex)
                    {
                        _exceptionHandlingStrategy.HandleException(ex, _logTransformers[i].Name);
                        if (_exceptionHandlingStrategy.ShouldRethrow)
                            throw;
                    }
        }
        private async Task ApplyLogTransformersAsync(LogModel logModel)
        {
            for (int i = 0; i < _logTransformers.Count; i++)
                if (_logTransformers[i].IsActive)
                    try
                    {
                        await _logTransformers[i].TransformAsync(logModel);
                    }
                    catch (Exception ex)
                    {
                        await _exceptionHandlingStrategy.HandleExceptionAsync(ex, _logTransformers[i].Name);
                        if (_exceptionHandlingStrategy.ShouldRethrow)
                            throw;
                    }
        }
    }
}
