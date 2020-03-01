using System.ComponentModel;
using TacitusLogger.Destinations;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ILoggerBuilder
    {
        ILoggerBuilder WithLogLevel(Setting<LogLevel> logLevel);
        ILoggerBuilder WithLogIdGenerator(ILogIdGenerator logIdGenerator);
        ILogContributorsBuilder Contributors();
        ILogTransformersBuilder Transformers();
        ILoggerBuilder WithLogCreation(LogCreationStrategyBase logCreationStrategy);
        ILoggerBuilder WithExceptionHandling(ExceptionHandlingStrategyBase exceptionHandlingStrategy);
        ILoggerBuilder WithDiagnostics(ILogDestination diagnosticsDestination);
        ILoggerBuilder WriteLoggerConfigurationToDiagnostics(bool should);
        ILoggerBuilder NewLogGroup(LogGroupBase logGroup);
        ILogGroupBuilder NewLogGroup(string logGroupName);
        ILogGroupBuilder NewLogGroup();
        Logger BuildLogger();
    }
}
