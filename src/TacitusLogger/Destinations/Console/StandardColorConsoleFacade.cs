using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Console
{
    /// <summary>
    /// A facade to the standard output device
    /// This class is internal.
    /// </summary>
    internal class StandardColorConsoleFacade : IColoredOutputDeviceFacade
    {
        /// <summary>
        /// Writes to the standard output device.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="color">Text color.</param>
        public void WriteLine(string text, ConsoleColor color)
        {
            ConsoleColor previousColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(text);
            System.Console.ForegroundColor = previousColor;
        }
        /// <summary>
        /// Asynchronously writes to the standard output device.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="color">Text color</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task WriteLineAsync(string text, ConsoleColor color, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);
            ConsoleColor previousColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            await System.Console.Out.WriteLineAsync(text);
            System.Console.ForegroundColor = previousColor;
        }
    }
}
