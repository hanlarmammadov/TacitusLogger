using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IDebugDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<IDebugDestinationBuilder>
    {

    }
}
