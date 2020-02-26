using Newtonsoft.Json;
using TacitusLogger.LogIdGenerators;

namespace TacitusLogger
{
    public static class Defaults
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        public static readonly LoggerSettings LoggerSettings = new LoggerSettings()
        {
            LogCreation = LogCreation.Standard,
            ExceptionHandling = ExceptionHandling.Silent
        };
        public static readonly LogGroupSettings LogGroupSettings = new LogGroupSettings()
        {
            DestinationFeeding = DestinationFeeding.Greedy,
            Status = LogGroupStatus.Active
        };
        public static readonly GuidLogIdGenerator LogIdGenerator = new GuidLogIdGenerator();
        public static readonly LogCreation LogCreationStrategy = LogCreation.Standard;
        public static readonly ExceptionHandling ExceptionHandlingStrategy = ExceptionHandling.Silent;
        public static readonly LogLevel LogLevel = LogLevel.All;
        public static readonly string ToStringIndentation = "      ";
      
    }
}
