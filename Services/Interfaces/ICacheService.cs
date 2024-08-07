namespace BBMPCITZAPI.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetCachedDataAsync<T>(string key);
        Task SetCacheDataAsync<T>(string key, T value);
    }
}
