using DTOs.Comments;

namespace BIMIssueManagerMarkupsEditor.Views.Chat
{
    public partial class AddCommentViewModel : ObservableObject
    {
        private readonly CommentApiService _commentApiService;
        [ObservableProperty] private string commentText;
     
        public AddCommentViewModel(CommentApiService commentApiService, int issueId)
        {
            _commentApiService = commentApiService;
            IssueId = issueId;
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
                // Optional: notify parent view or close dialog
            }
        }
    }
}