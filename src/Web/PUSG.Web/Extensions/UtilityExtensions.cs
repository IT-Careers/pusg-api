using System.Collections.Concurrent;

namespace PUSG.Web.Extensions
{
    public static class UtilityExtensions
    {
        public static bool Delete<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue result = default(TValue);
            return dictionary.TryRemove(key, out result);
        }
    }
}
