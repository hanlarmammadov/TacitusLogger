using System; 

namespace TacitusLogger
{
    public abstract class LogBuilderBase<TReturn>
    {
        public abstract TReturn Tagged(params string[] tags);
        public abstract TReturn From(string context);
        public abstract TReturn With(LogItem item);
        public abstract TReturn WithMany(params LogItem[] items);
         
        public TReturn From(Object context)
        {
            return From(context?.GetType().FullName);
        }
        public TReturn From<TContext>()
        {
            return From(typeof(TContext).FullName);
        }
        public TReturn With(string itemName, object itemValue)
        {
            return With(new LogItem(itemName, itemValue));
        }
        public TReturn With(object itemValue)
        {
            return With(new LogItem(itemValue?.GetType().FullName, itemValue));
        }
        public TReturn WithMany(params Object[] items)
        {
            if (items == null)
                return WithMany(new LogItem[0]);
            LogItem[] logItems = new LogItem[items.Length];
            for (int i = 0; i < items.Length; i++)
                logItems[i] = new LogItem(items[i]?.GetType().FullName, items[i]);
            return WithMany(logItems);
        }
    }
}
