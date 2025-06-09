using System.Net.Http.Headers;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class ApiService : IApiService
    {
        public readonly HttpClient _client;
        protected readonly UserSessionService _userSession;
        public ApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
        {
            _client = client;
            _userSession = userSession;
            _client.BaseAddress = new Uri(settings.Value.BaseUrl);
        }
        private void ApplyAuthHeader()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _userSession.Token);
        }
        public async Task<T> GetAsync<T>(string url)
        {
            ApplyAuthHeader();
            return await _client.GetFromJsonAsync<T>(url);
        }
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            ApplyAuthHeader();
            return await _client.PostAsJsonAsync(url, data);
        }
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            ApplyAuthHeader();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, data);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<TResponse>()
                : default;
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            ApplyAuthHeader();
            return await _client.PutAsJsonAsync(url, data);
        }
        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            ApplyAuthHeader();
            return await _client.DeleteAsync(url);
        }
    }
}

