
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Plant_Project.API.Services
{
    public class cacheService (IDistributedCache distributedCache): IcacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            var cachedValue=await _distributedCache.GetStringAsync(key, cancellationToken); 
            return string.IsNullOrEmpty(cachedValue) ?
                null : JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task SetAsync<T>(string Key, T? Value, CancellationToken cancellationToken = default) where T : class
        {
            await _distributedCache.SetStringAsync(Key,JsonSerializer.Serialize(Value), cancellationToken);
        }

        public async Task RemoveAsync(string Key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(Key, cancellationToken);
        }
    }
}
