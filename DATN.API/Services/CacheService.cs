using DATN.DAL.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Services
{
    public class CacheService
    {
        private readonly IMemoryCache memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public List<JobSearchResult> GetJobResultCache(string key)
        {
            List<JobSearchResult> value = new List<JobSearchResult>();
            memoryCache.TryGetValue(key, out value);
            return value != null ? value : new List<JobSearchResult>();
        }

        public void SetJobResultCache(string key, List<JobSearchResult> value)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1024,
            };

            memoryCache.Set(key, value, cacheExpiryOptions);
        }
    }
}
