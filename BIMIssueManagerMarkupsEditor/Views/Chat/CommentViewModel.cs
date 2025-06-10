using DTOs.Comments;

namespace BIMIssueManagerMarkupsEditor.Views.Chat
{
    public partial class CommentViewModel : ObservableObject
    {
        private readonly CommentApiService _commentApiService;
        [ObservableProperty] private string commentText;
        [ObservableProperty] private ObservableCollection<CommentDto> issueComments = new();
        public CommentViewModel(CommentApiService commentApiService, int issueId)
        {
            _commentApiService = commentApiService;
            IssueId = issueId;
        }
        public int IssueId { get; }

        [RelayCommand] private async Task SubmitAsync()
        {
            if (!string.IsNullOrWhiteSpace(CommentText))
            {
                CreateCommentDto dto = new CreateCommentDto
                {
                    Message = CommentText
                };

                await _commentApiService.CreateForSnapshotAsync(IssueId, dto);
                MessageBox.Show("Comment Added");
                CommentText = string.Empty;
                await LoadIssueCommentsAsync();
            }
        }
        public async Task LoadIssueCommentsAsync()
        {
            IEnumerable<CommentDto> comments = await _commentApiService.GetByIssueIdAsync(IssueId);
            IssueComments = new ObservableCollection<CommentDto>(comments);
        }
    }
}