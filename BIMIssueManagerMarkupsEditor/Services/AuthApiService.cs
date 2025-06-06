using DTOs.Login;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class AuthApiService : ApiService 
    {
        public AuthApiService(HttpClient client, UserSessionService userSession):base(client, userSession)
        {
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var response = await PostAsync<LoginRequestDto,LoginResponseDto>(Auth.Login(), dto);

            if (response is not null)
            {
                _userSession.SetSession(response);
            }

            return response;
        }
        
    }
}
