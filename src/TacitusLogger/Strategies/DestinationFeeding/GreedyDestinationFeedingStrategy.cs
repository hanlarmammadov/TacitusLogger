using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Exceptions;

namespace TacitusLogger.Strategies.DestinationFeeding
{
    /// <summary>
    /// This destination feeding strategy sends all logs successively to all destinations, 
    /// irrelevant to exceptions thrown by one or more of them.
    /// </summary>
    public class GreedyDestinationFeedingStrategy : DestinationFeedingStrategyBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="logDestinations"></param>
        public override void Feed(LogModel[] logs, IList<ILogDestination> logDestinations)
        {
            try
            {
                List<Exception> exceptions = null;
                for (int i = 0; i < logDestinations.Count; i++)
                    try
                    {
                        logDestinations[i].Send(logs);
                    }
                    catch (Exception ex)
                    {
                        if (exceptions == null)
                            exceptions = new List<Exception>(logDestinations.Count);
                        exceptions.Add(ex);
                    }
                if (exceptions != null)
                    throw new AggregateException(exceptions);
            }
            catch (AggregateException ex)
            {
                throw new DestinationFeedingException("One or more destinations threw exceptions. See inner exceptions", ex);
            }
            catch (Exception ex)
            {
                throw new DestinationFeedingException("Error in destination feeding strategy. See inner exception", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="logDestinations"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task FeedAsync(LogModel[] logs, IList<ILogDestination> logDestinations, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Check if operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    await Task.FromCanceled(cancellationToken);

                Task[] tasks = new Task[logDestinations.Count];
                for (int i = 0; i < logDestinations.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    tasks[i] = logDestinations[i].SendAsync(logs, cancellationToken);
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    Task mainTask = Task.WhenAll(tasks);
                    try
                    {
                        await mainTask;
                    }
                    catch (Exception)
                    {
                        throw mainTask.Exception;
                    }
                }
                else
                    await Task.FromCanceled(cancellationToken);
            }
            catch (AggregateException ex)
            {
                throw new DestinationFeedingException("One or more destinations threw exceptions. See inner exceptions", ex);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DestinationFeedingException("Error in destination feeding strategy. See inner exception", ex);
            }
        }
    }
}
