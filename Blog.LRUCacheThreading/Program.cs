using System;
using System.Threading.Tasks;

namespace Blog.LRUCacheThreadSafe
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LRUCache<int> cache = new LRUCache<int>(3);

            Task[] tasks = new[] {
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Set(1, 3)),
                Task.Run(() => cache.Set(1, 3))
            };

            await Task.WhenAll(tasks);
        }
    }
}