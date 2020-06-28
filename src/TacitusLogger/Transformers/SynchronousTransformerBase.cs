using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Transformers
{
    public abstract class SynchronousTransformerBase : LogTransformerBase
    {
        private bool _isDisposed;

        public SynchronousTransformerBase(string name)
            : base(name)
        {

        }

        public override Task TransformAsync(LogModel logModel, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("SynchronousTransformerBase");

            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            Transform(logModel);
            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            if (_isDisposed)
                return;

            base.Dispose();

            _isDisposed = true;
        }
    }
}
