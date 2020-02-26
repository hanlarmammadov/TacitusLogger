using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Exceptions;

namespace TacitusLogger.Strategies.DestinationFeeding
{
    /// <summary>
    /// 
    /// </summary>
    public class FirstSuccessDestinationFeedingStrategy : DestinationFeedingStrategyBase
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
                List<Exception> exceptions = new List<Exception>(logDestinations.Count);
                for (int i = 0; i < logDestinations.Count; i++)
                    try
                    {
                        logDestinations[i].Send(logs);
                        // If no exception was thrown, then consider the feed successful and do not proceed next destinations.
                        break;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                if (exceptions.Count != 0)
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

                List<Exception> exceptions = new List<Exception>(logDestinations.Count);

                for (int i = 0; i < logDestinations.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    try
                    {
                        await logDestinations[i].SendAsync(logs, cancellationToken);
                        // If no exception was thrown, then consider the feed successful and do not proceed next destinations.
                        break;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (exceptions.Count != 0)
                        throw new AggregateException(exceptions);
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
