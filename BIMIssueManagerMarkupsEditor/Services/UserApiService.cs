namespace BIMIssueManagerMarkupsEditor.Services
{
    public class UserApiService : ApiService
    {
        public UserApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                             : base(client, userSession, settings)
        {
        }
        public async Task<UserOverviewDto> RegisterUserAsync(RegisterUserDto dto)
        {
            return await PostAsync<RegisterUserDto, UserOverviewDto>(User.Create(), dto);
        }

        public async Task<IEnumerable<UserOverviewDto>> GetAllUsersAsync()
        {
            return await GetAsync<IEnumerable<UserOverviewDto>>(User.GetAll());
        }

        public async Task<IEnumerable<CompanyUserDto>> GetCompanyUsers(int companyId)
        {
            return await GetAsync<IEnumerable<CompanyUserDto>>(User.GetUsersByCompany(companyId));
        }
    }
}
