using DTOs.ProjectTeamMember;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class ProjectTeamMemberApiService : ApiService
    {
        public ProjectTeamMemberApiService(HttpClient client, UserSessionService userSession) : base(client, userSession)
        {
        }

        public async Task<IEnumerable<ProjectTeamMemberDto>> GetAll()
        {
            return await GetAsync<IEnumerable<ProjectTeamMemberDto>>(ProjectTeamMember.GetAll());
        }
        public async Task<IEnumerable<ProjectTeamMemberDto>> GetByProjectAsync(int projectId)
        {
            return await GetAsync<IEnumerable<ProjectTeamMemberDto>>(ProjectTeamMember.GetByProjectId(projectId));
        }

        public async Task<IEnumerable<ProjectTeamMemberDto>> GetByUserAsync(string userId)
        {
            return await GetAsync<IEnumerable<ProjectTeamMemberDto>>(ProjectTeamMember.GetTeamByUserId(userId));
        }
    }
}
