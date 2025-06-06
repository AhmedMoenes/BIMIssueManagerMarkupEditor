using BIMIssueManagerMarkupsEditor.ApiRoutes;
using DTOs.Projects;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class ProjectApiService : ApiService
    {
        public ProjectApiService(HttpClient client) : base(client)
        {
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByUserIdAsync(string userId)
        {
            return await GetAsync<IEnumerable<ProjectDto>>(Project.GetByUser(userId));
        }

        public async Task<IEnumerable<ProjectOverviewDto>> GetForCompanyAsync()
        {
            return await GetAsync<IEnumerable<ProjectOverviewDto>>(Project.GetForCompany());
        }

    }
}
