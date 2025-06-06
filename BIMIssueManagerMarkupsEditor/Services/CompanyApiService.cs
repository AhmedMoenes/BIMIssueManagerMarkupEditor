using DTOs.Companies;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class CompanyApiService : ApiService
    {
        public CompanyApiService(HttpClient client, UserSessionService userSession) : base(client, userSession)
        {
        }
        public async Task<IEnumerable<CompanyOverviewDto>> GetAllCompaniesAsync()
        {
            return await GetAsync<IEnumerable<CompanyOverviewDto>>(Company.GetAll());
        }
        public async Task<IEnumerable<CompanyOverviewDto>> GetCompanyOverviewForUserAsync(string userId)
        {
            return await GetAsync<IEnumerable<CompanyOverviewDto>>(Company.GetCompanyOverviewForUser(userId));
        }
    }
}
