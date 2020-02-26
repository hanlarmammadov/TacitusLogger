using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using System.Threading;
using TacitusLogger.Exceptions;
using TacitusLogger.Strategies.DestinationFeeding;
using TacitusLogger.Caching;
using System.Text;
using TacitusLogger.Components.Strings;

namespace TacitusLogger
{
    /// <summary>
    /// Represents a named group of log destinations provided with the log rule predicate.
    /// </summary>
    public class LogGroup : LogGroupBase
    {
        private readonly string _name;
        private readonly LogModelFunc<bool> _rule;
        private readonly List<ILogDestination> _logDestinations;
        private Setting<LogGroupStatus> _status;
        private bool _cachingIsActive;
        private ILogCache _logCache;
        private DestinationFeedingStrategyBase _destinationFeedingStrategy;
        private LogGroupSettings _logGroupSettings;

        public LogGroup(string name, LogModelFunc<bool> rule, IEnumerable<ILogDestination> logDestinations, LogGroupSettings logGroupSettings)
        {
            _logGroupSettings = logGroupSettings ?? throw new ArgumentNullException("logGroupSettings");
            _name = name ?? throw new ArgumentNullException("name");
            _status = logGroupSettings.Status ?? new MutableSetting<LogGroupStatus>(LogGroupStatus.Active);
            _cachingIsActive = false;
            _rule = rule ?? throw new ArgumentNullException("rule");
            _logDestinations = (logDestinations != null) ? logDestinations.ToList() : throw new ArgumentNullException("logDestinations");
            _destinationFeedingStrategy = new DestinationFeedingStrategyFactory().GetStrategy(logGroupSettings.DestinationFeeding);
        }
        public LogGroup(string name, LogModelFunc<bool> rule, IEnumerable<ILogDestination> logDestinations)
            : this(name, rule, logDestinations, Defaults.LogGroupSettings)
        {

        }
        public LogGroup(LogModelFunc<bool> rule, IEnumerable<ILogDestination> logDestinations, LogGroupSettings logGroupSettings)
            : this(GenerateGroupName(), rule, logDestinations, logGroupSettings)
        {

        }
        public LogGroup(LogModelFunc<bool> rule, IEnumerable<ILogDestination> logDestinations)
             : this(GenerateGroupName(), rule, logDestinations, Defaults.LogGroupSettings)
        {

        }
        public LogGroup(LogModelFunc<bool> rule, LogGroupSettings logGroupSettings)
            : this(GenerateGroupName(), rule, new List<ILogDestination>(), logGroupSettings)
        {

        }
        public LogGroup(LogModelFunc<bool> rule)
            : this(GenerateGroupName(), rule, new List<ILogDestination>(), Defaults.LogGroupSettings)
        {

        }
        public LogGroup(LogGroupSettings logGroupSettings)
            : this(GenerateGroupName(), l => true, new List<ILogDestination>(), logGroupSettings)
        {

        }
        public LogGroup()
            : this(GenerateGroupName(), l => true, new List<ILogDestination>(), Defaults.LogGroupSettings)
        {

        }

        /// <summary>
        /// Gets the LogModelFunc<bool> rule of the log group.
        /// </summary> 
        public LogModelFunc<bool> Rule => _rule;
        /// <summary>
        /// Gets the List<ILogDestination> of all log destinations of the log group.
        /// </summary> 
        public List<ILogDestination> LogDestinations => _logDestinations;
        /// <summary>
        /// Gets the name of the log group.
        /// </summary> 
        public override string Name => _name;
        /// <summary>
        /// Gets if log caching is enabled or not for this log group.
        /// </summary> 
        public bool CachingIsActive => _cachingIsActive;
        /// <summary>
        /// Gets the log cache that was specified during the initialization.
        /// </summary> 
        public ILogCache LogCache => _logCache;
        /// <summary>
        /// Gets the destination feeding strategy that was specified during the initialization.
        /// </summary> 
        public DestinationFeedingStrategyBase DestinationFeedingStrategy => _destinationFeedingStrategy;
        /// <summary>
        /// 
        /// </summary>
        public override Setting<LogGroupStatus> Status => _status;
        /// <summary>
        /// 
        /// </summary>
        internal LogGroupSettings LogGroupSettings => _logGroupSettings;


        /// <summary>
        /// Adds one or more log destinations to the list of log group log destinations
        /// </summary>
        /// <exception cref="ArgumentNullException">If no ILogDestination was specified or null array specified or one of specified ILogDestination is null.</exception>
        /// <param name="logDestinations">One or more ILogDestination</param>
        /// <returns>The LogGroup itself</returns>
        public LogGroup AddDestinations(params ILogDestination[] logDestinations)
        {
            if ((logDestinations == null) || (logDestinations.Length == 0))
                throw new ArgumentNullException("logDestinations", "Log destinations list should not be empty");

            if (logDestinations.Where(x => x == null).Count() != 0)
                throw new ArgumentNullException("logDestinations", "Log destination cannot be null");

            _logDestinations.AddRange(logDestinations);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override bool IsEligible(LogModel log)
        {
            return _rule(log);
        }
        /// <summary>
        /// Sends provided <paramref name="log"/> to all log destinations of this log group.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="log"/> is null.</exception>
        /// <param name="log">Log model of type log group.</param>
        public override void Send(LogModel log)
        {
            try
            {
                if (log == null)
                    throw new LogGroupException("Log model cannot be null");

                // Creates the logs array that will be sent to each log destination.
                // If the caching is not enabled, an array with the singe (current) log is created and sent to
                // destinations. Else if caching is enabled, then there are two possibilities: Either the cache is full or not.
                // If the cache is full after calling the AddToCache, it is flushed to logs array and that array is sent
                // to log destinations. Else if the cache is not yet full, AddToCache returns null and nothing happens with the log destinations 
                // and method returns. 
                LogModel[] logsArray;
                if (_cachingIsActive)
                    logsArray = _logCache.AddToCache(log);
                else
                    logsArray = new LogModel[1] { log };

                // Send logs to the strategy.
                if (logsArray != null)
                    _destinationFeedingStrategy.Feed(logsArray, _logDestinations);
            }
            catch (LogGroupException)
            {
                throw;
            }
            catch (LogCacheException)
            {
                throw;
            }
            catch (DestinationFeedingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogGroupException("Error when writing log to destinations. See the inner exception", ex);
            }
        }
        /// <summary>
        /// Asynchronously sends provided <paramref name="log"/> to all log destinations of this log group.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="log"/> is null.</exception>
        /// <param name="log">Log model of type LogModel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task SendAsync(LogModel log, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if the log is null. After this, no checks of this type will be performed for the log.
                if (log == null)
                    throw new LogGroupException("Log model cannot be null");

                // Check is operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    await Task.FromCanceled(cancellationToken);

                // Creates the logs array that will be sent to each log destination.
                // If the caching is not enabled, an array with the singe (current) log is created and sent to
                // destinations. Else if caching is enabled, then there are two possibilities: Either the cache is full or not.
                // If the cache is full after calling the AddToCache, it is flushed to logs array and that array is sent
                // to log destinations. Else if the cache is not yet full, AddToCache returns null and nothing happens with the log destinations 
                // and method returns.  
                LogModel[] logsArray;
                if (_cachingIsActive)
                    logsArray = _logCache.AddToCache(log);
                else
                    logsArray = new LogModel[1] { log };

                // Send logs to the strategy.
                if (logsArray != null)
                    await _destinationFeedingStrategy.FeedAsync(logsArray, _logDestinations, cancellationToken);
            }
            catch (LogGroupException)
            {
                throw;
            }
            catch (LogCacheException)
            {
                throw;
            }
            catch (DestinationFeedingException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogGroupException("Error when writing log to destinations. See the inner exception", ex);
            }
        }
        public LogGroup SetLogCache(ILogCache logCache, bool isActive = true)
        {
            _cachingIsActive = isActive;
            _logCache = logCache ?? throw new ArgumentNullException("logCache");
            return this;
        }
        public LogGroup SetStatus(Setting<LogGroupStatus> status)
        {
            _status = status ?? throw new ArgumentNullException("status");
            return this;
        }
        public void ResetDestinationFeedingStrategy(DestinationFeedingStrategyBase destinationFeedingStrategy)
        {
            _destinationFeedingStrategy = destinationFeedingStrategy ?? throw new ArgumentNullException("destinationFeedingStrategy");
        }
        public override void Dispose()
        {
            try
            {
                if (_cachingIsActive)
                {
                    // logModelTemp is used instead of passing null to log cache. It would be dangerous with custom implementations to send null. 
                    LogModel logModelTemp = new LogModel();
                    LogModel[] logsArray = _logCache.AddToCache(logModelTemp, true);
                    if (logsArray.Length > 1)
                    {
                        var logsList = logsArray.ToList();
                        logsList.Remove(logModelTemp);
                        _destinationFeedingStrategy.Feed(logsList.ToArray(), _logDestinations);
                    }
                }
            }
            catch { }

            try
            {
                if (_logCache != null)
                    _logCache.Dispose();
            }
            catch { }
            try
            {
                _status.Dispose();
            }
            catch { }

            for (int i = 0; i < _logDestinations.Count; i++)
                try
                {
                    _logDestinations[i].Dispose();
                }
                catch { }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder()
                                    .AppendLine(this.GetType().FullName)
                                    .AppendLine($"Name: {_name}")
                                    .AppendLine($"Status: {_status?.ToString()?.AddIndentationToLines()}")
                                    .AppendLine($"Destination feeding: {_destinationFeedingStrategy.ToString()?.AddIndentationToLines()}")
                                    .Append($"Caching is active: {_cachingIsActive.ToString().AddIndentationToLines()}");

            sb.AppendLine().Append($"Log Cache: ");
            if (_logCache != null)
                sb.Append(_logCache.ToString()?.AddIndentationToLines());
            else
                sb.Append("[No cache]");
             
            sb.AppendLine().Append("Log destinations: ");
            if (_logDestinations.Count != 0)
                for (int i = 0; i < _logDestinations.Count; i++)
                    sb.AppendLine().Append(Defaults.ToStringIndentation).Append($"{(i + 1).ToString()}. {_logDestinations[i].ToString()}".AddIndentationToLines().AddIndentationToLines());
            else
                sb.Append("[No destinations]");

            return sb.ToString();
        }
        /// <summary>
        /// Generates a GUID based random unique name for log group. 
        /// </summary>
        /// <returns>Random group name.</returns>
        internal static string GenerateGroupName()
        {
            return "Group_" + Guid.NewGuid().ToString("N").Substring(0, 8); 
        }

    }
}
