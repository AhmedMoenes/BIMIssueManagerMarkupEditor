namespace BIMIssueManagerMarkupsEditor.Services
{
    public class ProjectTeamMemberApiService : ApiService
    {
        public ProjectTeamMemberApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                                          : base(client, userSession, settings)
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

        public async Task<ProjectTeamMemberDto> AssignUserToProjectsAsync(AssignUserToProjectDto dto)
        {
            return await PostAsync<AssignUserToProjectDto, ProjectTeamMemberDto>(ProjectTeamMember.AssignUserToProject(), dto);
        }
    }
}
