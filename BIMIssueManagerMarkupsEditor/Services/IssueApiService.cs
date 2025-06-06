using DTOs.Issues;
namespace BIMIssueManagerMarkupsEditor.Services
{
    public class IssueApiService : ApiService
    {
        public IssueApiService(HttpClient client, UserSessionService userSession) : base(client, userSession){}
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
        public Task<HttpResponseMessage> DeleteAsync(int id) => DeleteAsync(Issue.Delete(id));
        public async Task<IEnumerable<IssueDto>> GetIssuesByProjectIdAsync(int projectId)
        {
            return await GetAsync<List<IssueDto>>(Issue.GetByProjectId(projectId));
        }
        public async Task<IEnumerable<IssueDto>> GetIssuesByUserIdAsync(string userId)
        {
            return await GetAsync<List<IssueDto>>(Issue.GetByUserId(userId));
        }
    }
}
