namespace Plant_Project.API.Services
{
    public interface IcacheService
    {
        Task<T?>GetAsync<T>(string key,CancellationToken cancellationToken=default) where T:class;
        Task SetAsync<T>(string Key,T? Value,CancellationToken cancellationToken=default) where T :class;
        Task RemoveAsync(string Key, CancellationToken cancellationToken = default);
    }
}
