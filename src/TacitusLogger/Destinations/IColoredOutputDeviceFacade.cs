using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations
{
    /// <summary>
    /// An interface defining a facade to the standard output device.
    /// This interface is internal.
    /// </summary>
    internal interface IColoredOutputDeviceFacade
    {
        void WriteLine(string text, ConsoleColor color);
        Task WriteLineAsync(string text, ConsoleColor color, CancellationToken cancellationToken = default(CancellationToken));
    }
}
