using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Blog.LRUCacheThreadSafe.Tests
{
    public class LRUCacheSignalingTests
    {
        private const int threadNumbers = 20;

        private readonly LRUCacheReaderWriterLock<int> _cache
            = new LRUCacheReaderWriterLock<int>(threadNumbers);

        private readonly ITestOutputHelper _outputHelper;

        public LRUCacheSignalingTests(ITestOutputHelper outputHelper) =>
            _outputHelper = outputHelper;

        [Fact]
        public void should_supports_operations_from_multiple_threads()
        {
            _outputHelper.WriteLine($"1: Spawn {threadNumbers} threads");

            ManualResetEventSlim setPhase = new ManualResetEventSlim(false);
            ManualResetEventSlim getPhase = new ManualResetEventSlim(false);

            Thread[] threads = new Thread[threadNumbers];

            int progressCounter = 0;

            for (int threadid = 0; threadid < threadNumbers; threadid++)
            {
                threads[threadid] = new Thread(id =>
                {
                    setPhase.Wait();
                    
                    int currentId = (int) id;
                    
                    _cache.Set(currentId, currentId);
                    _outputHelper.WriteLine($"	Setting the following id: {currentId} | value : {currentId}");

                    Interlocked.Increment(ref progressCounter);
                    getPhase.Wait();

                    object value = _cache.Get(currentId);
                    _outputHelper.WriteLine($"	Getting the following id: {currentId} | value: {value}");

                    Interlocked.Increment(ref progressCounter);
                });

                threads[threadid].Start(threadid);
            }

            _outputHelper.WriteLine($"2: Performing n.{threadNumbers} Set operations from multiple threads");
            setPhase.Set();
            while (progressCounter < threadNumbers) continue;

            progressCounter = 0;

            _outputHelper.WriteLine($"3: Performing n.{threadNumbers} Get operations from multiple threads");
            getPhase.Set();
            while (progressCounter < threadNumbers) continue;

            _outputHelper.WriteLine("--- End of test ---");
        }
    }
}