using System.Threading.Tasks;
using Xunit;

namespace Blog.LRUCacheThreadSafe.Tests
{
    public class LRUCacheLockThreadsTests
    {

        [Fact]
        public async Task multiple_threads_should_get_data_correctly_lock()
        {
            LRUCacheLock<int> cache = new LRUCacheLock<int>(3);
            
            Task[] tasks = new[] {
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Set(1, 3))
            };

            await Task.WhenAll(tasks);
            Assert.NotNull(cache.Get(1));
        }
    
    }
}