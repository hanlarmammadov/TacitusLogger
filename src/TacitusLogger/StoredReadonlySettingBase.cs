using System;
using TacitusLogger.Components.Time;

namespace TacitusLogger
{
    public abstract class StoredReadonlySettingBase<TValue> : Setting<TValue>
    {
        private readonly long _valueReloadIntervalTicks;
        private ITimeProvider _timeProvider;
        private long _lastUpdateTicks;
        private TValue _value;
        private Object _valueLocker = new Object();

        public StoredReadonlySettingBase(int fetchValueIntervalMilliseconds)
        {
            _valueReloadIntervalTicks = fetchValueIntervalMilliseconds * 10_000;
            _timeProvider = new SystemTimeProvider();
        }

        public override TValue Value
        {
            get
            {
                if (_timeProvider.GetLocalTime().Ticks - _lastUpdateTicks > _valueReloadIntervalTicks)
                    lock (_valueLocker)
                        if (_timeProvider.GetLocalTime().Ticks - _lastUpdateTicks > _valueReloadIntervalTicks)
                        {
                            _value = LoadValue();
                            _lastUpdateTicks = _timeProvider.GetLocalTime().Ticks;
                        }
                return _value;
            }
        }
        internal void ResetTimeProvider(ITimeProvider timeProvider)
        {
            lock (_valueLocker)
                _timeProvider = timeProvider ?? throw new ArgumentNullException("timeProvider");
        }
        protected abstract TValue LoadValue();
    }
}
