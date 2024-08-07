using BBMPCITZAPI.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace BBMPCITZAPI.Services
{
   
        public class CacheService : ICacheService
        {
            private readonly IConnectionMultiplexer _connectionMultiplexer;

            public CacheService(IConnectionMultiplexer connectionMultiplexer)
            {
                _connectionMultiplexer = connectionMultiplexer;
            }

            public async Task<T> GetCachedDataAsync<T>(string key)
            {
                var db = _connectionMultiplexer.GetDatabase();
                var value = await db.StringGetAsync(key);

                if (value.IsNullOrEmpty)
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(value);
            }

            public async Task SetCacheDataAsync<T>(string key, T value)
            {
                var db = _connectionMultiplexer.GetDatabase();
                var jsonData = JsonSerializer.Serialize(value);
                await db.StringSetAsync(key, jsonData);
            }
        }
    }
