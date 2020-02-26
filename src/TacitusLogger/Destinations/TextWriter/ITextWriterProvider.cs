using System; 

namespace TacitusLogger.Destinations.TextWriter
{
    /// <summary>
    /// An interface all TextWriter providers should implement.
    /// </summary>
    public interface ITextWriterProvider : IDisposable
    {
        System.IO.TextWriter GetTextWriter(LogModel logModel);
    }
}
