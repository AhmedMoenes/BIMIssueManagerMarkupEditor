namespace BIMIssueManagerMarkupsEditor.Services
{
    public class AuthApiService : ApiService 
    {
        public AuthApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                             : base(client, userSession, settings)
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

        public async Task LogoutAsync()
        {
            await PostAsync(Auth.Logout(), string.Empty);
            _userSession.Clear();
        }

    }
}
