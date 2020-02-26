using System.ComponentModel;

namespace TacitusLogger
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SettingBuilder<TValue>
    {
        internal SettingBuilder() { }

        public MutableSetting<TValue> Variable(TValue initialValue = default(TValue))
        {
            return new MutableSetting<TValue>(initialValue);
        }
    }
}
