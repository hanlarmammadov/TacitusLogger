using System;
using System.Text;

namespace TacitusLogger
{
    public class MutableSetting<TValue> : Setting<TValue>
    {
        private TValue _value;
        private Object _valueLocker = new Object();

        public MutableSetting(TValue initialValue = default(TValue))
        {
            _value = initialValue;
        }

        public override TValue Value
        {
            get
            {
                lock (_valueLocker)
                    return _value;
            }
        }

        public static implicit operator MutableSetting<TValue>(TValue value)
        {
            var obj = new MutableSetting<TValue>();
            obj._value = value;
            return obj;
        }

        public void SetValue(TValue value)
        {
            lock (_valueLocker)
                _value = value;
        }
        public override string ToString()
        {
            return new StringBuilder()
               .Append($"MutableSetting [Value = {_value}]")
               .ToString();
        }
    }
}
