using System;
using TacitusLogger.Components.Time;

namespace TacitusLogger
{
    public abstract class StoredReadonlySettingBase<TValue> : Setting<TValue>
    {
        private readonly int _valueReloadInterval;
        private ITimeProvider _timeProvider;
        private long _lastUpdateTicks;
        private TValue _value;

        public StoredReadonlySettingBase(int valueReloadInterval)
        {
            _valueReloadInterval = valueReloadInterval;
            _timeProvider = new SystemTimeProvider();
        }

        public override TValue Value
        {
            get
            {
                if (_timeProvider.GetLocalTime().Ticks - _lastUpdateTicks > _valueReloadInterval * 10_000)
                {
                    _value = LoadValue();
                    _lastUpdateTicks = _timeProvider.GetLocalTime().Ticks;
                }
                return _value;
            }
        }
        internal void ResetTimeProvider(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException("timeProvider");
        }
        protected abstract TValue LoadValue();
    }
}
