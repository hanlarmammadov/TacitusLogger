using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Exceptions;

namespace TacitusLogger.LogIdGenerators
{
    /// <summary>
    /// Convenient to be inherited from if log id generation operation represents a quick synchronous process
    /// which does not assume any specific overriding of the async counterpart of Generate method.
    /// </summary>
    public abstract class SynchronousLogIdGeneratorBase : ILogIdGenerator
    {
        private bool _isDisposed;

        public abstract string Generate(LogModel logModel);
        /// <summary>
        /// Asynchronous counterpart of Generate method.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>A task that represents completed asynchronous operation. The value of the TResult represents the resulting log id string.</returns>
        public Task<string> GenerateAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_isDisposed)
                throw new ObjectDisposedException("SynchronousLogIdGeneratorBase");

            // Check if operation has been canceled.
            if (cancellationToken.IsCancellationRequested)
                return Task<string>.FromCanceled<string>(cancellationToken);
            return Task.FromResult<string>(Generate(logModel));
        }
        public virtual void Dispose()
        {
            _isDisposed = true;
        }
    }
}
