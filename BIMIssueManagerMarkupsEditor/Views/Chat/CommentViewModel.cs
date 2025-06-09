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

            LoadIssueCommentsAsync();
        }

        public int IssueId { get; }

        [RelayCommand]
        private async Task SubmitAsync()
        {
            if (!string.IsNullOrWhiteSpace(CommentText))
            {
                var dto = new CreateCommentDto
                {
                    IssueId = IssueId,
                    Message = CommentText
                };

                await _commentApiService.CreateForIssueAsync(IssueId, dto);
                CommentText = string.Empty;
                await LoadIssueCommentsAsync();
            }
        }
        private async Task LoadIssueCommentsAsync()
        {
            var comments = await _commentApiService.GetByIssueIdAsync(IssueId);
            IssueComments = new ObservableCollection<CommentDto>(comments);
        }
    }
}