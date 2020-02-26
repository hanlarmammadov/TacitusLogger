using System.ComponentModel; 
using TacitusLogger.Destinations.TextWriter;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ITextWriterDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<ITextWriterDestinationBuilder>
    {
        ITextWriterDestinationBuilder WithWriter(ITextWriterProvider textWriterProvider); 
    }
}
