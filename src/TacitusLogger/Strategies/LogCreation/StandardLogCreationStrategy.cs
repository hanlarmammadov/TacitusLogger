using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Time;
using TacitusLogger.Exceptions;
using TacitusLogger.Contributors;
using System.Text;
using TacitusLogger.Components.Strings;
using System.IO;

namespace TacitusLogger.Strategies.LogCreation
{
    /// <summary>
    /// This class manages the creation of log model using the default strategy.
    /// </summary>
    public class StandardLogCreationStrategy : LogCreationStrategyBase
    {
        private readonly bool _useUtcTime;
        private ITimeProvider _timeProvider;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.LogCreationStrategies.StandardLogCreationStrategy</c>.
        /// </summary>
        /// <param name="useUtcTime">Indicates whether local or UTC time should be assigned to log date. If omitted, local time will be used.</param>
        public StandardLogCreationStrategy(bool useUtcTime = false)
        {
            _useUtcTime = useUtcTime;
            _timeProvider = new SystemTimeProvider();
        }

        /// <summary>
        /// Gets the time provider specified during the initialization.
        /// </summary>
        public ITimeProvider TimeProvider => _timeProvider;
        /// <summary>
        /// Indicates whether local or UTC time will be assigned to log date.
        /// </summary>
        public bool UseUtcTime => _useUtcTime;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.LogModel</c> model using specified log.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="source">Log source.</param>
        /// <returns>Created log model.</returns>
        public override LogModel CreateLogModel(Log log, string source)
        {
            try
            {
                if (log == null)
                    throw new LogCreationException("Cannot create log model using null log object");

                // Create log model entity with all fields except log id and log items.
                LogModel logModel = GetLogModelWithoutItems(log, source);

                // Add log items from log class and from the contributors
                AddLogItemsToModel(logModel, log.Items);

                // Add log id.
                logModel.LogId = LogIdGenerator.Generate(logModel);

                //Return the created log model.
                return logModel;
            }
            catch (LogCreationException)
            {
                throw;
            }
            catch (LogIdGeneratorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogCreationException("Error when creating the log model. See the inner exception", ex);
            }
        }
        /// <summary>
        /// Asynchronously creates an instance of <c>TacitusLogger.LogModel</c> model using specified log.
        /// </summary>
        /// <param name="log"></param> 
        /// <param name="source">Log source.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Created log model.</returns>
        public override async Task<LogModel> CreateLogModelAsync(Log log, string source, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Check if operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    await Task<LogModel>.FromCanceled(cancellationToken);

                // 
                if (log == null)
                    throw new LogCreationException("Cannot create log model using null log object");

                // Create log model entity with all fields except log id and log items.
                LogModel logModel = GetLogModelWithoutItems(log, source);
                 
                // Add log items from log class and from the contributors.
                await AddLogItemsToModelAsync(logModel, log.Items);
                 
                // Add log id.
                logModel.LogId = await LogIdGenerator.GenerateAsync(logModel).ConfigureAwait(continueOnCapturedContext: false);

                //Return the created log model.
                return logModel;
            }
            catch (LogIdGeneratorException)
            {
                throw;
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogCreationException("Error when creating the log model. See the inner exception", ex);
            }
        }
        /// <summary>
        /// Resets the time provider that was set during the initialization.
        /// </summary>
        /// <param name="timeProvider"></param>
        public void ResetTimeProvider(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException("timeProvider");
        }
        public override string ToString()
        {
            return new StringBuilder()
                   .AppendLine(this.GetType().FullName) 
                   .AppendLine($"Time provider: {_timeProvider.ToString().AddIndentationToLines()}") 
                   .AppendLine($"Use UTC time: {_useUtcTime.ToString()}")
                   .ToString();
        }
         
        private LogModel GetLogModelWithoutItems(Log log, string source)
        {
            // Create log model entity without log id, tags and log items.
            LogModel logModel = new LogModel()
            {
                Source = source,
                Context = log.Context,
                LogType = log.Type,
                LogDate = (_useUtcTime) ? _timeProvider.GetUtcTime() : _timeProvider.GetLocalTime(),
                Description = log.Description
            };

            // Add tags
            int tagsCount = (log.Tags != null) ? log.Tags.Count : 0;
            logModel.Tags = new string[tagsCount];
            for (int i = 0; i < tagsCount; i++)
                logModel.Tags[i] = log.Tags[i];

            return logModel;
        }
        private void AddLogItemsToModel(LogModel logModel, IList<LogItem> itemsFromLog)
        {
            List<LogItem> logItemsList = new List<LogItem>(20);

            // Add log items from log object
            if (itemsFromLog != null && itemsFromLog.Count != 0)
                logItemsList.AddRange(itemsFromLog);

            // Add log items from log contributors.
            for (int i = 0; i < LogContributors.Count; i++)
            {
                if (!LogContributors[i].IsActive)
                    continue;
                try
                {
                    logItemsList.Add(LogContributors[i].ProduceLogItem());
                }
                catch (Exception ex)
                {
                    ExceptionHandlingStrategy.HandleException(ex, $"Log contributor: {LogContributors[i].Name}");
                    if (ExceptionHandlingStrategy.ShouldRethrow)
                        throw;
                }
            }
            //Set log items array.
            logModel.LogItems = logItemsList.ToArray();
        }
        private async Task AddLogItemsToModelAsync(LogModel logModel, IList<LogItem> itemsFromLog)
        {
            List<LogItem> logItemsList = new List<LogItem>(20);

            // Add log items from log object
            if (itemsFromLog != null && itemsFromLog.Count != 0)
                logItemsList.AddRange(itemsFromLog);

            // Add log items from log contributors.
            for (int i = 0; i < LogContributors.Count; i++)
            {
                if (!LogContributors[i].IsActive)
                    continue;
                try
                {
                    logItemsList.Add(await LogContributors[i].ProduceLogItemAsync());
                }
                catch (Exception ex)
                {
                    await ExceptionHandlingStrategy.HandleExceptionAsync(ex, $"Log contributor: {LogContributors[i].Name}");
                    if (ExceptionHandlingStrategy.ShouldRethrow)
                        throw;
                }
            }
            //Set log items array.
            logModel.LogItems = logItemsList.ToArray();
            await Task.CompletedTask;
        }
    }
}
