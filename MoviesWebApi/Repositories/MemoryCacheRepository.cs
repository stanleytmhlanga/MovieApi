using MoviesWebApi.Interfaces;
using System;
using System.Runtime.Caching;

namespace MoviesWebApi.Repositories
{
    public class MemoryCacheRepository : ICacheService
    {
        public T Get<T>(string cache) where T : class
        {
            var caching = MemoryCache.Default.Get(cache) as T;
            return caching;
        }

        public void Set(string cache, object item, int minutes)
        {
            if(item != null)
            {
                MemoryCache.Default.Add(cache, item, DateTime.Now.AddMinutes(20));
            }
        }
    }
}
