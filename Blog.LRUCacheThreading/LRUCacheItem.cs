namespace Blog.LRUCacheThreadSafe
{
    public class LRUCacheItem<T>
    {
        public int CacheKey { get; set; }
        public T CacheValue { get; set; }
    }
}