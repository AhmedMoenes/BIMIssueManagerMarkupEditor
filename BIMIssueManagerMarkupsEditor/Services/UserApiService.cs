namespace BIMIssueManagerMarkupsEditor.Services
{
    public class UserApiService : ApiService
    {
        public UserApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                             : base(client, userSession, settings)
        {
        }
    }
}
