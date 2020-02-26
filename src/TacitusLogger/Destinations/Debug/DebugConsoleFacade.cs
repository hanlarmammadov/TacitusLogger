using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Debug
{
    /// <summary>
    /// A facade for System.Diagnostics.Debug.
    /// This class is internal.
    /// </summary>
    internal class DebugConsoleFacade : IOutputDeviceFacade
    {
        /// <summary>
        /// Writes the <paramref name="text"/> to the <c>System.Diagnostics.Debug.</c>
        /// </summary>
        /// <param name="text"></param>
        public void WriteLine(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        /// <summary>
        /// Asynchronously writes the <paramref name="text"/> to the <c>System.Diagnostics.Debug.</c>
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task WriteLineAsync(string text, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            if (text == null)
                throw new ArgumentNullException("text");
            System.Diagnostics.Debug.WriteLine(text);
            return Task.CompletedTask;
        }
    }
}
