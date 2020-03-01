using System.ComponentModel;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuilderWithFilePath<TBuilder>
    {
        TBuilder WithPath(ILogSerializer filePathGenerator);
    }
}
