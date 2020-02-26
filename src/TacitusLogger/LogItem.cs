using System;

namespace TacitusLogger
{
    public class LogItem
    {
        private String _name;
        private Object _value;

        public LogItem(String name, Object value)
        {
            _name = name;
            _value = value;
        }

        public String Name { get { return _name; } set { _name = value; } }
        public Object Value { get { return _value; } set { _value = value; } }

        public static LogItem FromObj(Object value)
        {
            return new LogItem("Log item", value);
        }
        public static LogItem FromObj(String name, Object value)
        {
            return new LogItem(name, value);
        }
        public static LogItem FromEx(Exception ex)
        {
            return new LogItem("Exception", ex);
        }
        public static LogItem[] FromSeveral(params Object[] values)
        {
            LogItem[] logItems = null;
            if (values != null)
            {
                logItems = new LogItem[values.Length];
                for (int i = 0; i < values.Length; i++)
                    logItems[i] = new LogItem("Log item", values[i]);
            }
            else
                logItems = new LogItem[0];
            return logItems;
        }
    }
}
