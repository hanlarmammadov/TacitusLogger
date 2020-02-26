using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;

namespace TacitusLogger.Strategies.DestinationFeeding
{
    /// <summary>
    /// Defines the logic of how log models will be consumed by log destinations within the log groups.
    /// </summary>
    public abstract class DestinationFeedingStrategyBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="logDestinations"></param>
        public abstract void Feed(LogModel[] logs, IList<ILogDestination> logDestinations);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="logDestinations"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task FeedAsync(LogModel[] logs, IList<ILogDestination> logDestinations, CancellationToken cancellationToken = default(CancellationToken));
    }
}
