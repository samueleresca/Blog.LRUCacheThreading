using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.LRUCacheThreadSafe.Tests
{
    public class LRUCacheReaderWriterLockSlimThreadsTests
    {
        [Fact]
        public async Task  multiple_threads_should_get_data_correctly_semaphore()
        {
            LRUCacheReaderWriterLock<int> cache = new LRUCacheReaderWriterLock<int>(3);
            
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