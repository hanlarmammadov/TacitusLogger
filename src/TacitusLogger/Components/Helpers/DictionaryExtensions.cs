using System.Collections.Generic; 

namespace TacitusLogger.Components.Helpers
{
    public static class DictionaryExtensions
    {
        public static void TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (!self.ContainsKey(key))
                self.Add(key, value);
        }
    }
}
