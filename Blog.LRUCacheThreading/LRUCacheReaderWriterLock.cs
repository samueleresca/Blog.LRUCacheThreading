using System.Collections.Generic;
using System.Threading;

namespace Blog.LRUCacheThreadSafe
{
    public class LRUCacheReaderWriterLock<T>
    {
        private readonly Dictionary<int, LRUCacheItem<T>> _records = new Dictionary<int, LRUCacheItem<T>>();
        private readonly LinkedList<int> _freq = new LinkedList<int>();
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();
        private readonly int _capacity;

        public LRUCacheReaderWriterLock(int capacity)
        {
            _capacity = capacity;
        }

        public int Capacity => _capacity;

        public object Get(int key)
        {
            try
            {
                _readerWriterLockSlim.EnterUpgradeableReadLock();
                var keyNotExists = !_records.ContainsKey(key);

                if (keyNotExists) return null;

                _readerWriterLockSlim.EnterWriteLock();

                _freq.Remove(key);
                _freq.AddLast(key);

                _readerWriterLockSlim.ExitWriteLock();

                return _records[key].CacheValue;
            }
            finally
            {
                _readerWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        public void Set(int key, T val)
        {
            try
            {
                _readerWriterLockSlim.EnterWriteLock();

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
                _readerWriterLockSlim.ExitWriteLock();
            }
        }
    }
}