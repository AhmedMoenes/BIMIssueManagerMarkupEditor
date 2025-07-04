﻿namespace BIMIssueManagerMarkupsEditor.Services
{
    public class IssueApiService : ApiService
    {
        public IssueApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings)
                              : base(client, userSession, settings)
        {}
        public Task<IEnumerable<IssueDto>> GetAllAsync() => GetAsync<IEnumerable<IssueDto>>(Issue.GetAll());
        public Task<IssueDto?> GetByIdAsync(int id) => GetAsync<IssueDto>(Issue.GetById(id));
        public async Task<IssueDto?> CreateAsync(CreateIssueDto dto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(Issue.Create(), dto);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<IssueDto>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error => {ex.Message}");
                throw;
            }
        }
        public Task<HttpResponseMessage> UpdateAsync(int id, UpdateIssueDto dto) => PutAsync(Issue.Update(id), dto);
        public Task<HttpResponseMessage> UpdateResolvedStatusAsync(int id, bool status) => PutAsync(Issue.MarkAsResolved(id), status);
        public Task<HttpResponseMessage> DeleteAsync(int id) => base.DeleteAsync(Issue.Delete(id));
        public async Task<IEnumerable<IssueDto>> GetIssuesByProjectIdAsync(int projectId)
        {
            return await GetAsync<List<IssueDto>>(Issue.GetByProjectId(projectId));
        }
        public async Task<IEnumerable<IssueDto>> GetIssuesByUserIdAsync(string userId)
        {
            return await GetAsync<List<IssueDto>>(Issue.GetByUserId(userId));
        }

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
