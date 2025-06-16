public partial class IssueDetailsViewModel : ObservableObject
{
    private readonly IssueApiService _issueApiService;
    private readonly IDialogService _dialogService;
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<int, CommentViewModel> _commentVmFactory;

    public IssueDetailsViewModel(
        IssueApiService issueApiService,
        IDialogService dialogService,
        IServiceProvider serviceProvider,
        Func<int, CommentViewModel> commentVmFactory)
    {
        _issueApiService = issueApiService;
        _dialogService = dialogService;
        _serviceProvider = serviceProvider;
        _commentVmFactory = commentVmFactory;
    }

    [ObservableProperty]
    private IssueDto? issue;

    [RelayCommand]
    private async Task MarkAsResolved()
    {
        if (Issue == null || Issue.IsResolved)
            return;

        Issue.IsResolved = true;
        await _issueApiService.UpdateResolvedStatusAsync(Issue.IssueId, true);
        OnPropertyChanged(nameof(Issue));
    }

    [RelayCommand]
    private async Task DeleteIssue()
    {
        if (Issue == null)
            return;

        await _issueApiService.DeleteAsync(Issue.IssueId);
        MessageBox.Show("Issue deleted.");
    }

    [RelayCommand]
    private void EditIssue()
    {
        MessageBox.Show("Edit Issue flow to be implemented...");
        // TODO: Launch dialog with pre-filled form for editing
    }

    [RelayCommand]
    private async Task AddCommentAsync()
    {
        var vm = _commentVmFactory(Issue.IssueId);
        await vm.LoadIssueCommentsAsync();
        await _dialogService.ShowDialogAsync<CommentView, CommentViewModel>(vm);
    }

    public async Task LoadIssueAsync(int issueId)
    {
        Issue = await _issueApiService.GetByIdAsync(issueId);
    }
}
