using System.Collections.Generic;

namespace Blog.LRUCacheThreadSafe
{
    public class LRUCacheLock<T>
    {
        private readonly Dictionary<int, LRUCacheItem<T>> _records = new Dictionary<int, LRUCacheItem<T>>();
        private readonly LinkedList<int> _freq = new LinkedList<int>();
        private readonly int _capacity;
        private static readonly object _locker = new object();

        public LRUCacheLock(int capacity)
        {
            _capacity = capacity;
        }

        public int Capacity => _capacity;

        public object Get(int key)
        {
            lock (_locker)
            {
                if (!_records.ContainsKey(key)) return null;

                _freq.Remove(key);
                _freq.AddLast(key);

                return _records[key].CacheValue;
            }
        }

        public void Set(int key, T val)
        {
            lock (_locker)
            {
                if (_records.ContainsKey(key))
                {
                    _records[key].CacheValue = val;
                    _freq.Remove(key);
                    _freq.AddLast(key);
                    return;
                }

                if (_records.Count >= _capacity)
                {
                    _records.Remove(_freq.First.Value);
                    _freq.RemoveFirst();
                }

                _records.Add(key, new LRUCacheItem<T> { CacheKey = key, CacheValue = val });
                _freq.AddLast(key);
            }
        }
    }
}