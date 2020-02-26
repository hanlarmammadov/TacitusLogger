
using System.Text;

namespace TacitusLogger
{
    public class MutableSetting<TValue> : Setting<TValue>
    {
        private TValue _value;

        public MutableSetting(TValue initialValue = default(TValue))
        {
            SetValue(initialValue);
        }

        public override TValue Value => _value;
        
        public static implicit operator MutableSetting<TValue>(TValue value)
        {
            var obj = new MutableSetting<TValue>();
            obj._value = value;
            return obj;
        }

        public void SetValue(TValue value)
        {
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
