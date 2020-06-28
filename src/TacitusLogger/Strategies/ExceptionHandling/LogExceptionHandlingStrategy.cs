using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Strategies.ExceptionHandling
{
    public class LogExceptionHandlingStrategy : ExceptionHandlingStrategyBase
    {
        public override bool ShouldRethrow => false;

        public override void HandleException(Exception exception, string context)
        {
            try
            {
                var log = Log.Error("Logger threw an exception. See the log item.").From(context).WithEx(exception);
                DiagnosticsManager.WriteToDiagnostics(log);
            }
            catch
            {

            }
        }
        public override async Task HandleExceptionAsync(Exception exception, string context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    await Task.FromCanceled(cancellationToken);

                var log = Log.Error("Logger threw an exception. See the log item.").From(context).WithEx(exception);
                await DiagnosticsManager.WriteToDiagnosticsAsync(log, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {

            }
        }
    }
}
