using System.ComponentModel;
using TacitusLogger.Caching;
using TacitusLogger.Strategies.DestinationFeeding;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ILogGroupBuilder
    {
        ILogGroupDestinationsBuilder ForRule(LogModelFunc<bool> rule);
        ILogGroupBuilder SetStatus(Setting<LogGroupStatus> status);
        ILogGroupBuilder WithCaching(ILogCache logCache, bool isActive = true);
        ILogGroupBuilder WithDestinationFeeding(DestinationFeedingStrategyBase destinationFeedingStrategy);
    }
}
