using System;
using System.ComponentModel;
using TacitusLogger.Components.Time;
using TacitusLogger.Caching;
using TacitusLogger.Strategies.DestinationFeeding;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.ILogGroupBuilder</c> interface and its implementations.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ILogGroupBuilderExtensions
    {
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider all logs as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForAllLogs(this ILogGroupBuilder self)
        {
            return self.ForRule(x => true);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of this type as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <param name="logType">Specified log type.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForLogType(this ILogGroupBuilder self, LogType logType)
        {
            return self.ForRule(x => x.LogType == logType);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Success as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForSuccessLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Success);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Info as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForInfoLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Info);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Event as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForEventLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Event);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Warning as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForWarningLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Warning);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Failure as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForFailureLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Failure);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Error as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForErrorLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Error);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs of type Critical as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForCriticalLogs(this ILogGroupBuilder self)
        {
            return self.ForLogType(LogType.Critical);
        }
        /// <summary>
        /// Sets the log group rule predicate to the log group 
        /// that will consider only logs with this context as eligible to be sent to this log group.
        /// Context matching is case-sensitive.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <param name="context">Specified log context.</param> 
        /// <returns>Log group destinations builder for this log group builder.</returns> 
        public static ILogGroupDestinationsBuilder ForLogContext(this ILogGroupBuilder self, string context)
        {
            return self.ForRule(x => x.Context == context);
        }
        /// <summary>
        /// Sets the log group rule predicate that will consider only logs 
        /// with log type higher than or equal to the specified log level as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Log group builder.</param>
        /// <param name="logLevel">Specified log level.</param>
        /// <returns></returns>
        public static ILogGroupDestinationsBuilder ForLogLevel(this ILogGroupBuilder self, LogLevel logLevel)
        {
            return self.ForRule(x => x.IsWithinLogLevel(logLevel));
        }
        /// <summary>
        /// Adds logs caching. When enabled, caching suspends sending logs to destinations until specified number of 
        /// them has been collected in cache or specified time in milliseconds has been passed from the previous flush depending which occurs first. After this
        /// all cached logs are flushed to destinations at the same time.
        /// </summary>
        /// <param name="cacheSize">Max size of caching collection after reaching which the cache is pending flushing.</param>
        /// <param name="cacheTime">Time in milliseconds from the previous flush after which the cache is pending flushing. By default -1 which means cache time check turned off.</param>
        /// <param name="isActive"></param>
        /// <returns>Self.</returns> 
        public static ILogGroupBuilder WithCaching(this ILogGroupBuilder self, int cacheSize, int cacheTime = -1, bool isActive = true)
        {
            return self.WithCaching(new InMemoryLogCache(cacheSize, cacheTime), isActive);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="logCache"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static ILogGroupBuilder WithCaching(this ILogGroupBuilder self, ILogCache logCache, bool isActive = true)
        {
            return self.WithCaching(logCache, isActive);
        }
        /// <summary>
        /// 
        /// </summary> 
        /// <param name="destinationFeedingStrategyType"></param>
        /// <returns>Self.</returns>
        public static ILogGroupBuilder WithDestinationFeeding(this ILogGroupBuilder self, DestinationFeeding destinationFeeding)
        {
            return self.WithDestinationFeeding(new DestinationFeedingStrategyFactory().GetStrategy(destinationFeeding));
        }
        /// <summary>
        /// 
        /// </summary> 
        /// <returns></returns>
        public static ILogGroupBuilder WithGreedyDestinationFeeding(this ILogGroupBuilder self)
        {
            return self.WithDestinationFeeding(new DestinationFeedingStrategyFactory().GetStrategy(DestinationFeeding.Greedy));
        }
        /// <summary>
        /// 
        /// </summary> 
        /// <returns></returns>
        public static ILogGroupBuilder WithFirstSuccessDestinationFeeding(this ILogGroupBuilder self)
        {
            return self.WithDestinationFeeding(new DestinationFeedingStrategyFactory().GetStrategy(DestinationFeeding.FirstSuccess));
        }
    }
}
