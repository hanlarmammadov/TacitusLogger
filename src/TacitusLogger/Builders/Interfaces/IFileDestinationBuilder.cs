using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFileDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<IFileDestinationBuilder>, IBuilderWithFilePath<IFileDestinationBuilder>
    {

    }
}
