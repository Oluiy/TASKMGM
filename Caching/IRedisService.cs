namespace TaskManager.Caching;

public interface IRedisService
{
    T? GetData<T>(string key);
    void SetData<T>(string key, T data);
}