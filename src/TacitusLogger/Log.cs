using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger
{
    public class Log : LogBuilderBase<Log>
    {
        private String _context; 
        private LogType _type;
        private String _description;
        private IList<String> _tags;
        private IList<LogItem> _items;

        public Log(string context, LogType type, string description, IList<String> tags, IList<LogItem> logItems)
        {
            _context = context;
            _type = type; 
            _description = description;
            _tags = tags ?? new List<String>(20);
            _items = logItems ?? new List<LogItem>(20);
        }
        public Log(LogType type, string description)
            : this(context: null, type: type, description: description, tags: null, logItems: null)
        {

        }
        public Log()
            : this(context: null, type: default(LogType), description: null, tags: null, logItems: null)
        {

        }

        public String Context => _context; 
        public LogType Type => _type; 
        public String Description => _description;
        public IList<String> Tags => _tags;
        public IList<LogItem> Items => _items;

        #region Entry points 

        public static Log Success(string description)
        {
            return new Log(LogType.Success, description);
        }
        public static Log Info(string description)
        {
            return new Log(LogType.Info, description);
        }
        public static Log Event(string description)
        {
            return new Log(LogType.Event, description);
        }
        public static Log Warning(string description)
        {
            return new Log(LogType.Warning, description);
        }
        public static Log Failure(string description)
        {
            return new Log(LogType.Failure, description);
        }
        public static Log Error(string description)
        {
            return new Log(LogType.Error, description);
        }
        public static Log Critical(string description)
        {
            return new Log(LogType.Critical, description);
        }

        #endregion

        public override Log Tagged(params string[] tags)
        {
            if (tags != null)
                for (int i = 0; i < tags.Length; i++)
                    if (tags[i] != null)
                        _tags.Add(tags[i]);
            return this;
        }
        public override Log From(string context)
        {
            _context = context;
            return this;
        }
        public override Log With(LogItem item)
        {
            if (item != null)
                _items.Add(item);
            return this;
        }
        public override Log WithMany(params LogItem[] items)
        {
            if (items != null)
                for (int i = 0; i < items.Length; i++)
                    if (items[i] != null)
                        _items.Add(items[i]);
            return this;
        }
        public void To(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");
            logger.Log(this);
        }
        public Task ToAsync(ILogger logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (logger == null)
                return Task.FromException<ArgumentNullException>(new ArgumentNullException("logger"));
            return logger.LogAsync(this, cancellationToken);
        }
    }
}
