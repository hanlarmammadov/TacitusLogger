using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Transformers
{
    public abstract class LogTransformerBase : IDisposable
    {
        private readonly string _name;
        private Setting<bool> _isActive;
        private bool _isDisposed;

        protected LogTransformerBase(string name)
        {
            _name = name ?? throw new ArgumentNullException("name");
            _isActive = new MutableSetting<bool>(true);
        }

        public string Name => _name;
        public Setting<bool> IsActive => _isActive;

        public abstract void Transform(LogModel logModel);
        public abstract Task TransformAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken));
        public void SetActive(Setting<bool> isActive)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("LogTransformerBase");

            _isActive = isActive ?? throw new ArgumentNullException("isActive");
        }
        public virtual void Dispose()
        {
            if (_isDisposed)
                return;

            try
            {
                if (_isActive != null)
                    _isActive.Dispose();
            }
            catch { }

            _isDisposed = true;
        } 
        public override string ToString()
        {
            return new StringBuilder()
               .AppendLine(this.GetType().FullName)
               .AppendLine($"Name: {Name}")
               .Append($"Is active: {IsActive.ToString()}")
               .ToString();
        }
    }
}
