using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger
{
    public class LogBuilder : LogBuilderBase<LogBuilder>
    {
        private readonly ILogger _logger;
        private readonly Log _log;

        internal LogBuilder(ILogger logger, LogType logType, string description)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
            _log = new Log(logType, description);
        }

        internal ILogger Logger => _logger;
        internal Log BuiltLog => _log;

        public override LogBuilder Tagged(params string[] tags)
        {
            _log.Tagged(tags);
            return this;
        }
        public override LogBuilder From(string context)
        {
            _log.From(context);
            return this;
        }
        public override LogBuilder With(LogItem item)
        {
            _log.With(item);
            return this;
        }
        public override LogBuilder WithMany(params LogItem[] items)
        {
            _log.WithMany(items);
            return this;
        }
        public string Log()
        {
            return _logger.Log(_log);
        }
        public Task<string> LogAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _logger.LogAsync(_log, cancellationToken);
        }
    }
}
