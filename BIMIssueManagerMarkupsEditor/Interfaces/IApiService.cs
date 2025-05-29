public interface IApiService
{
    Task<T> GetAsync<T>(string url);
    Task<HttpResponseMessage> PostAsync<T>(string url, T data);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data);
    Task<HttpResponseMessage> PutAsync<T>(string url, T data);
    Task<HttpResponseMessage> DeleteAsync(string url);
}