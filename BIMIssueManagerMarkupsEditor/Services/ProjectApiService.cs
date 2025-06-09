using BIMIssueManagerMarkupsEditor.ApiRoutes;
using DTOs.Projects;

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

    }
}
