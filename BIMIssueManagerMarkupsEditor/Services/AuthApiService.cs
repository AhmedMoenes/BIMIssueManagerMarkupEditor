using DTOs.Login;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class AuthApiService : ApiService 
    {
        private readonly UserSessionService _userSession;
        public AuthApiService(HttpClient client, UserSessionService userSession):base(client)
        {
            _userSession = userSession;
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
