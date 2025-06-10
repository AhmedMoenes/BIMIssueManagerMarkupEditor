using DTOs.Comments;

namespace BIMIssueManagerMarkupsEditor.Views.Chat
{
    public partial class CommentViewModel : ObservableObject
    {
        private readonly CommentApiService _commentApiService;
        private readonly UserSessionService _userSession;
        [ObservableProperty] private string commentText;
        [ObservableProperty] private ObservableCollection<CommentDto> issueComments = new();
        public CommentViewModel(CommentApiService commentApiService, UserSessionService userSession, int issueId)
        {
            _commentApiService = commentApiService;
            IssueId = issueId;
            _userSession = userSession;
        }
        public int IssueId { get; }

        [RelayCommand] private async Task SubmitAsync()
        {
            if (!string.IsNullOrWhiteSpace(CommentText))
            {
                CreateCommentDto dto = new CreateCommentDto
                {
                    Message = CommentText,
                    IssueId = IssueId,
                    SnapshotId = null,
                    CreatedByUserId = _userSession.UserId
                };

                CommentDto result = await _commentApiService.CreateForIssueAsync(IssueId, dto);
                if (result != null)
                {
                    CommentText = string.Empty;
                    await LoadIssueCommentsAsync();
                }
                else
                {
                    MessageBox.Show("Failed to add comment");
                }
            }
        }
        public async Task LoadIssueCommentsAsync()
        {
            IEnumerable<CommentDto> comments = await _commentApiService.GetByIssueIdAsync(IssueId);
            IssueComments = new ObservableCollection<CommentDto>(comments);
        }
    }
}