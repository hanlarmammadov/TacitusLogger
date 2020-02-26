using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Contributors
{
    public abstract class LogContributorBase : IDisposable
    {
        private readonly string _name;
        private Setting<bool> _isActive;

        protected LogContributorBase(string name)
        {
            _name = name ?? throw new ArgumentNullException("name");
            _isActive = new MutableSetting<bool>(true);
        }

        public string Name => _name;
        public Setting<bool> IsActive => _isActive;

        public virtual LogItem ProduceLogItem()
        {
            return new LogItem(_name, GenerateLogItemData());
        }
        public async virtual Task<LogItem> ProduceLogItemAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new LogItem(_name, await GenerateLogItemDataAsync(cancellationToken));
        }
        public void SetActive(Setting<bool> isActive)
        {
            _isActive = isActive ?? throw new ArgumentNullException("isActive");
        }
        public virtual void Dispose()
        {
            try
            {
                if (_isActive != null)
                    _isActive.Dispose();
            }
            catch { }
        }
        public override string ToString()
        {
            return new StringBuilder()
               .AppendLine(this.GetType().FullName)
               .AppendLine($"Name: {Name}")
               .Append($"Is active: {IsActive.ToString()}")
               .ToString();
        }
        protected abstract Object GenerateLogItemData();
        protected abstract Task<Object> GenerateLogItemDataAsync(CancellationToken cancellationToken);
    }
}
