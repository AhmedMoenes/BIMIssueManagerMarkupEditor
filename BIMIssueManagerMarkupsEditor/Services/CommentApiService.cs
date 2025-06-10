using DTOs.Comments;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class CommentApiService : ApiService
    {
        public CommentApiService(HttpClient client, UserSessionService userSession, IOptions<ApiSettings> settings) 
                                : base(client, userSession, settings)
        {
        }

        public Task<IEnumerable<CommentDto>> GetAllAsync()
             => GetAsync<IEnumerable<CommentDto>>(Comment.GetAll());

        public Task<CommentDto> GetByIdAsync(int commentId)
            => GetAsync<CommentDto>(Comment.GetById(commentId));

        public Task<HttpResponseMessage> DeleteAsync(int commentId)
            => base.DeleteAsync(Comment.Delete(commentId));

        public Task<IEnumerable<CommentDto>> GetByIssueIdAsync(int issueId)
            => GetAsync<IEnumerable<CommentDto>>(Comment.GetByIssueId(issueId));

        public Task<IEnumerable<CommentDto>> GetBySnapshotIdAsync(int snapshotId)
            => GetAsync<IEnumerable<CommentDto>>(Comment.GetBySnapshotId(snapshotId));

        public Task<CommentDto?> CreateAsync(CreateCommentDto dto)
            => PostAsync<CreateCommentDto, CommentDto>(Comment.Create(), dto);
        public Task<CommentDto?> CreateForIssueAsync(int issueId, CreateCommentDto dto)
            => PostAsync<CreateCommentDto, CommentDto>(Comment.CreateForIssue(issueId), dto);

        public Task<HttpResponseMessage> UpdateAsync(int id, CommentDto dto)
            => PutAsync(Comment.GetById(id), dto);
    }
}
