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

    }
}
