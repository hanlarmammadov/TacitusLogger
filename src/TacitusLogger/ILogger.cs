using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger
{
    /// <summary>
    /// ILogger interface that contains only the basic logging methods.
    /// </summary>
    public interface ILogger : IDisposable
    {
        string Log(Log log);
        Task<string> LogAsync(Log log, CancellationToken cancellationToken = default(CancellationToken));
    }
}
