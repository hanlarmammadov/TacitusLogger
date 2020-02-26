using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations
{
    /// <summary>
    /// An interface defining a facade to the standard output device. 
    /// </summary>
    public interface IOutputDeviceFacade
    {
        void WriteLine(string text);
        Task WriteLineAsync(string text, CancellationToken cancellationToken = default(CancellationToken));
    }
}
