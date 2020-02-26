using System.ComponentModel;
using TacitusLogger.Destinations;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)] 
    public interface ILogGroupDestinationsBuilder
    {
        ILogGroupDestinationsBuilder CustomDestination(ILogDestination logDestination);
        ILoggerBuilder BuildLogGroup();
    }
}
