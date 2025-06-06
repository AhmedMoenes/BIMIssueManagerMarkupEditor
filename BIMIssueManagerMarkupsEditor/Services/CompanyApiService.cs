namespace BIMIssueManagerMarkupsEditor.Services
{
    public class CompanyApiService : ApiService
    {
        public CompanyApiService(HttpClient client, UserSessionService userSession) : base(client, userSession)
        {
        }
    }
}
