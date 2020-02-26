using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.LogIdGenerators
{
    /// <summary>
    /// Represents an interface which all log id generators should implement.
    /// </summary>
    public interface ILogIdGenerator : IDisposable
    {
        string Generate(LogModel logModel);
        Task<string> GenerateAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken));
    }
}
