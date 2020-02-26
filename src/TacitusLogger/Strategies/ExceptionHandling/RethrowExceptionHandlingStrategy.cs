using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;

namespace TacitusLogger.Strategies.ExceptionHandling
{
    public class RethrowExceptionHandlingStrategy : ExceptionHandlingStrategyBase
    {
        public override bool ShouldRethrow => true;

        public override void HandleException(Exception exception, string context)
        {

        }
        public override Task HandleExceptionAsync(Exception exception, string context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            else
                return Task.CompletedTask;
        }
    }
}
