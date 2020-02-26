using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Transformers
{
    public abstract class SynchronousTransformerBase : LogTransformerBase
    {
        public SynchronousTransformerBase(string name)
            : base(name)
        {

        }

        public override Task TransformAsync(LogModel logModel, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            Transform(logModel);
            return Task.CompletedTask;
        }
    }
}
