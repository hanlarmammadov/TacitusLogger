using System.ComponentModel; 
using TacitusLogger.Transformers;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ILogTransformersBuilder
    {
        ILogTransformersBuilder Custom(LogTransformerBase logTransformer, Setting<bool> isActive);
        ILoggerBuilder BuildTransformers();
    }
}
