using DTOs.Login;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class AuthApiService : ApiService 
    {
        public AuthApiService(HttpClient client):base(client){}
        public Task<HttpResponseMessage> LoginAsync(LoginRequestDto dto) => PostAsync(Auth.Login(), dto);
        
    }
}
