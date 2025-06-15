namespace BIMIssueManagerMarkupsEditor.Services
{
    public class CompanyApiService : ApiService
    {
        public CompanyApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                                : base(client, userSession,settings)
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
        public async Task<CompanyDto> CreateCompanyWithAdminAsync(CreateCompanyWithAdminDto dto)
        {
            return await PostAsync<CreateCompanyWithAdminDto, CompanyDto>(Company.CreateWithAdmin(), dto);
        }
        public Task<HttpResponseMessage> DeleteAsync(int companyId)
            => base.DeleteAsync(Company.Delete(companyId));
    }
}
