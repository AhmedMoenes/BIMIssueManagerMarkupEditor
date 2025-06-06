namespace BIMIssueManagerMarkupsEditor.Services
{
    public class UserApiService : ApiService
    {
        public UserApiService(HttpClient client, UserSessionService userSession) : base(client, userSession)
        {
        }
    }
}
