using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IDestinationBuilder
    {
        ILogGroupDestinationsBuilder Add();
    }
}
