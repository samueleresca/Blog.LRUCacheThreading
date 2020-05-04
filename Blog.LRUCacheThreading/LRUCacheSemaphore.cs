using System.Collections.Generic;
using System.Threading;

namespace Blog.LRUCacheThreadSafe
{
    public class LRUCacheSemaphore<T>
    {
        private readonly Dictionary<int, LRUCacheItem<T>> _records = new Dictionary<int, LRUCacheItem<T>>();
        private readonly LinkedList<int> _freq = new LinkedList<int>();
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(1);
        private readonly int _capacity;

        public LRUCacheSemaphore(int capacity)
        {
            _capacity = capacity;
        }

        public int Capacity => _capacity;

        public object Get(int key)
        {
            try
            {
                _sem.Wait();
                if (!_records.ContainsKey(key)) return null;

                _freq.Remove(key);
                _freq.AddLast(key);

                return _records[key].CacheValue;
            }
            finally
            {
                _sem.Release();
            }
        }

        public void Set(int key, T val)
        {
            try
            {
                _sem.Wait();

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
            finally
            {
                _sem.Release();
            }
        }
    }
}