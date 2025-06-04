namespace BIMIssueManagerMarkupsEditor.Services
{
    public class ApiService : IApiService
    {
        public readonly HttpClient _client;
        public ApiService(HttpClient client) => _client = client;
        public async Task<T> GetAsync<T>(string url) => await _client.GetFromJsonAsync<T>(url);
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data) => await _client.PostAsJsonAsync(url, data);
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, data);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<TResponse>()
                : default;
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
            => await _client.PutAsJsonAsync(url, data);
        public async Task<HttpResponseMessage> DeleteAsync(string url)
            => await _client.DeleteAsync(url);
    }
}

