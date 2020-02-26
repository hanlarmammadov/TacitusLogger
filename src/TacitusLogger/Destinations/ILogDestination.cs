using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations
{
    /// <summary>
    /// Represents an interface which all log destinations should implement.
    /// </summary>
    public interface ILogDestination : IDisposable
    {
        void Send(LogModel[] logs);
        Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken));
    }
}
