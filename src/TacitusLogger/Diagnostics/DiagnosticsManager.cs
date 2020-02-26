using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Time;
using TacitusLogger.LogIdGenerators;

namespace TacitusLogger.Diagnostics
{
    public class DiagnosticsManager : DiagnosticsManagerBase
    {
        private readonly ILogIdGenerator _logIdGenerator;
        private readonly bool _useUtcTime;
        private ITimeProvider _timeProvider;

        public DiagnosticsManager(ILogIdGenerator logIdGenerator, bool useUtcTime = false)
        {
            _logIdGenerator = logIdGenerator ?? throw new ArgumentNullException("logIdGenerator");
            _useUtcTime = useUtcTime;
            _timeProvider = new SystemTimeProvider();
        }
        public DiagnosticsManager()
            : this(new GuidLogIdGenerator(), false)
        {

        }

        public ILogIdGenerator LogIdGenerator => _logIdGenerator;
        public bool UseUtcTime => _useUtcTime;
        public ITimeProvider TimeProvider => _timeProvider;

        public override void WriteToDiagnostics(Log log)
        {
            LogModel logModel = new LogModel()
            {
                Description = log.Description,
                Context = log.Context,
                LogDate = _useUtcTime ? _timeProvider.GetUtcTime() : _timeProvider.GetLocalTime(),
                LogItems = log.Items.ToArray(),
                LogType = log.Type,
                Source = _loggerName
            };
            logModel.LogId = _logIdGenerator.Generate(logModel);
            SendToDestination(logModel);
        }
        public override async Task WriteToDiagnosticsAsync(Log log, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);

            LogModel logModel = new LogModel()
            {
                Description = log.Description,
                Context = log.Context,
                LogDate = _useUtcTime ? _timeProvider.GetUtcTime() : _timeProvider.GetLocalTime(),
                LogItems = log.Items.ToArray(),
                LogType = log.Type
            };
            logModel.LogId = await _logIdGenerator.GenerateAsync(logModel);
            await SendToDestinationAsync(logModel);
        }
        public void ResetTimeProvider(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException("timeProvider");
        }
    }
}
