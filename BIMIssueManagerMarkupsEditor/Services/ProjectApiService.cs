using DTOs.CompanyProject;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class ProjectApiService : ApiService
    {
        public ProjectApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                                : base(client, userSession, settings)
        {
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByUserIdAsync(string userId)
        {
            return await GetAsync<IEnumerable<ProjectDto>>(Project.GetByUser(userId));
        }

        public async Task<IEnumerable<ProjectOverviewDto>> GetForCompanyAsync(int companyId)
        {
            return await GetAsync<IEnumerable<ProjectOverviewDto>>(Project.GetForCompany(companyId));
        }
        public async Task<IEnumerable<ProjectOverviewDto>> GetForUserAsync(string userId)
        {
            return await GetAsync<IEnumerable<ProjectOverviewDto>>(Project.GetForUser(userId));
        }
        public async Task<IEnumerable<ProjectOverviewDto>> GetForSubscriberAsync()
        {
            return await GetAsync<IEnumerable<ProjectOverviewDto>>(Project.GetForSubscriber());
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            return await PostAsync<CreateProjectDto, ProjectDto>(Project.Create(),dto);
        }

        public async Task<HttpResponseMessage> AssignCompanyToProjectAsync(AssignCompaniesToProjectDto dto)
        {
            return await PostAsync (CompanyProject.AssignCompanies(), dto);
        }
    }
}
