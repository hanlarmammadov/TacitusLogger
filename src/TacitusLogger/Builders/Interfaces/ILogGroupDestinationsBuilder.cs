using System.ComponentModel;
using TacitusLogger.Destinations;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)] 
    public interface ILogGroupDestinationsBuilder
    {
        ILogGroupDestinationsBuilder CustomDestination(ILogDestination logDestination);
        ILoggerBuilder BuildLogGroup();
    }
}
