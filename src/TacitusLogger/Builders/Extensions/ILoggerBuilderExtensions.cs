using System.ComponentModel;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.ILoggerBuilder</c> interface and its implementations.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ILoggerBuilderExtensions
    {
        /// <summary>
        /// Creates a new log group and sets the rule predicate to it
        /// that will consider all logs as eligible to be sent to this log group.
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <returns>Log group destinations builder for this log group builder.</returns>
        public static ILogGroupDestinationsBuilder ForAllLogs(this ILoggerBuilder self)
        {
            return self.NewLogGroup().ForAllLogs();
        }
        /// <summary>
        /// Adds a custom log id generator of type <c>TacitusLogger.LogIdGenerators.GuidLogIdGenerator</c> to the 
        /// logger builder.
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithGuidLogId(this ILoggerBuilder self)
        {
            return self.WithLogIdGenerator(new GuidLogIdGenerator());
        }
        /// <summary>
        /// Adds a custom log id generator of type <c>TacitusLogger.LogIdGenerators.GuidLogIdGenerator</c> with the
        /// specified GUID string format to the logger builder.
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <param name="guidFormat">The guid format that will be used to create the string representation of the GUID.</param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithGuidLogId(this ILoggerBuilder self, string guidFormat)
        {
            return self.WithLogIdGenerator(new GuidLogIdGenerator(guidFormat));
        }
        /// <summary>
        /// Adds to the logger builder a custom log id generator of type <c>TacitusLogger.LogIdGenerators.GuidLogIdGenerator</c>
        /// that will use only the substring of the resulting GUID string as a log id. Substring is considered from the beginning of the initial GUID string.
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <param name="guidSubstringLength">The length of substring of the initial GUID string from the beginning.</param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithGuidLogId(this ILoggerBuilder self, int guidSubstringLength)
        {
            return self.WithLogIdGenerator(new GuidLogIdGenerator("N", guidSubstringLength));
        }
        /// <summary>
        /// Adds to the logger builder a custom log id generator of type <c>TacitusLogger.LogIdGenerators.GuidLogIdGenerator</c> with the
        /// specified GUID string format that will use only the substring of the resulting GUID string as a log id. 
        /// Substring is considered from the beginning of the initial GUID string.
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <param name="guidFormat">The guid format that will be used to create the string representation of the GUID.</param>
        /// <param name="guidSubstringLength">The length of substring of the initial GUID string from the beginning.</param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithGuidLogId(this ILoggerBuilder self, string guidFormat, int guidSubstringLength)
        {
            return self.WithLogIdGenerator(new GuidLogIdGenerator(guidFormat, guidSubstringLength));
        } 
        /// <summary>
        /// Adds to the logger builder a log id generator of type <c>TacitusLogger.LogIdGenerators.NullLogIdGenerator</c> which simply generates
        /// NULL string as log ID for every log. 
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithNullLogId(this ILoggerBuilder self)
        {
            return self.WithLogIdGenerator(new NullLogIdGenerator());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <param name="logCreationStrategy"></param>
        /// <param name="useUtcTime"></param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithLogCreation(this ILoggerBuilder self, LogCreation logCreationStrategy, bool useUtcTime = false)
        {
            return self.WithLogCreation(new LogCreationStrategyFactory().GetStrategy(logCreationStrategy, useUtcTime));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <param name="exceptionHandlingStrategy"></param>
        /// <param name="useUtcTime"></param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithExceptionHandling(this ILoggerBuilder self, ExceptionHandling exceptionHandlingStrategy)
        {
            return self.WithExceptionHandling(new ExceptionHandlingStrategyFactory().GetStrategy(exceptionHandlingStrategy));
        }
        /// <summary>
        /// Sets the minimal log level. All logs that have log type
        /// less than this level will be ignored by the logger.
        /// </summary>
        /// <param name="self">Logger builder.</param>
        /// <param name="logLevel">Log level.</param>
        /// <returns>Self.</returns>
        public static ILoggerBuilder WithLogLevel(this ILoggerBuilder self, LogLevel logLevel)
        {
            return self.WithLogLevel(new MutableSetting<LogLevel>(logLevel));
        }
    }
}
