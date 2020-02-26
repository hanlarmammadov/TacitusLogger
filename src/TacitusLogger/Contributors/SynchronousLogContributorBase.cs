using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Contributors
{
    public abstract class SynchronousLogContributorBase : LogContributorBase
    {
        protected SynchronousLogContributorBase(string name)
            : base(name)
        {

        }
        protected override Task<object> GenerateLogItemDataAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<object>(cancellationToken);
            return Task.FromResult(GenerateLogItemData());
        }
    }
}
