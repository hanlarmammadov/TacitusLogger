using System;
using System.Text;

namespace TacitusLogger
{
    public class Setting<TValue> : IDisposable
    {
        private TValue _value;

        public static SettingBuilder<TValue> From => new SettingBuilder<TValue>();

        public virtual TValue Value => _value;

        protected Setting()
        {

        }

        public static implicit operator Setting<TValue>(TValue value)
        {
            var obj = new Setting<TValue>();
            obj._value = value;
            return obj;
        }
        public static implicit operator TValue(Setting<TValue> setting)
        {
            return setting.Value;
        }
        public virtual void Dispose()
        {

        }
        public override string ToString()
        {
            return new StringBuilder()
               .Append($"Setting [Value = {_value}]")
               .ToString();
        }
    }
}
