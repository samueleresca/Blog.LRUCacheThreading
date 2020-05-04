using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.LRUCacheThreadSafe.Tests
{
    public class LRUCacheSemaphoreThreadsTests
    {
        [Fact]
        public async Task  multiple_threads_should_get_data_correctly_semaphore()
        {
            LRUCacheSemaphore<int> cache = new LRUCacheSemaphore<int>(3);
            
            Task[] tasks = new[] {
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Get(1)),
                Task.Run(() => cache.Get(1)),
                Task.Run(() => cache.Get(1))

            };

            await Task.WhenAll(tasks);
            Assert.NotNull(cache.Get(1));
        }
    
    }
}