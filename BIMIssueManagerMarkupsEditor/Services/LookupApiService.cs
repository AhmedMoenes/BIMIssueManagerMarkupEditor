namespace BIMIssueManagerMarkupsEditor.Services
{
    public class LookupApiService : ApiService
    {

        public LookupApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                                : base(client, userSession, settings) 
        { }

        public Task<List<AreaDto>> GetAreasByProjectIdAsync(int projectId)
            => GetAsync<List<AreaDto>>($"api/area/project/{projectId}");

        public Task<List<UserDto>> GetUsersByProjectIdAsync(int projectId)
            => GetAsync<List<UserDto>>($"api/users/project-users/{projectId}");

        public Task<List<LabelDto>> GetLabelsByProjectIdAsync(int projectId)
            => GetAsync<List<LabelDto>>($"api/labels/project/{projectId}/labels");
        public Task<List<ProjectDto>> GetProjectsByUserIdAsync(string userId)
            => GetAsync<List<ProjectDto>>($"api/projects/user/{userId}");
    }
}
