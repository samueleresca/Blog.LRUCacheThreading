using Xunit;
using Xunit.Abstractions;

namespace Blog.LRUCacheThreadSafe.Tests
{
    public class LRUCacheTests
    {
        [Fact]
        public void should_remove_least_accessed_records_when_capacity_is_reached()
        {
            LRUCache<int> cache = new LRUCache<int>(1);

            cache.Set(1, 3);
            cache.Set(2, 4);
            cache.Set(3, 5);

            Assert.Null(cache.Get(1));
            Assert.Null(cache.Get(2));
            Assert.NotNull(cache.Get(3));
        }

        [Fact]
        public void should_limit_based_on_the_capacity_and_store_the_value()
        {
            LRUCache<int> cache = new LRUCache<int>(2);

            cache.Set(1, 3);
            cache.Set(2, 4);
            cache.Set(3, 5);

            Assert.Null(cache.Get(1));
            Assert.Equal(4, cache.Get(2));
            Assert.Equal(5, cache.Get(3));
        }

        [Fact]
        public void should_prioritize_accessed_records()
        {
            LRUCache<int> cache = new LRUCache<int>(3);

            cache.Set(1, 3);
            cache.Set(2, 4);
            cache.Set(3, 5);

            var value = cache.Get(3);

            cache.Set(4, 6);

            Assert.Null(cache.Get(1));
            Assert.NotNull(cache.Get(2));
            Assert.NotNull(cache.Get(3));
            Assert.NotNull(cache.Get(4));
        }
    }
}