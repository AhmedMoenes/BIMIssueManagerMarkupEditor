public partial class IssueDetailsViewModel : ObservableObject , IDialogAware
{
    private readonly IssueApiService _issueApiService;
    private readonly IDialogService _dialogService;
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<int, CommentViewModel> _commentVmFactory;
    public event Action? RequestClose;

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

    [ObservableProperty] private IssueDto? issue;
    public string Title => Issue?.Title ?? "Issue Details";
    partial void OnIssueChanged(IssueDto? value)
    {
        OnPropertyChanged(nameof(Title));
    }

    [RelayCommand] private async Task MarkAsResolved()
    {
        if (Issue == null || Issue.IsResolved)
            return;

        Issue.IsResolved = true;
        await _issueApiService.UpdateResolvedStatusAsync(Issue.IssueId, true);
        OnPropertyChanged(nameof(Issue));
    }
    [RelayCommand] private async Task DeleteIssue()
    {
        if (Issue == null)
            return;

        await _issueApiService.DeleteAsync(Issue.IssueId);
        RequestClose.Invoke();        
    }
    [RelayCommand] private async Task EditIssue()
    {
        var vmFactory = _serviceProvider.GetRequiredService<Func<int, EditIssueViewModel>>();
        var vm = vmFactory(Issue.IssueId);
        await vm.LoadAsync();
        await _dialogService.ShowDialogAsync<EditIssueView, EditIssueViewModel>(vm);
        Issue = await _issueApiService.GetByIdAsync(Issue.IssueId);
    }
    [RelayCommand] private async Task AddCommentAsync()
    {
        var vm = _commentVmFactory(Issue.IssueId);
        await vm.LoadIssueCommentsAsync();
        await _dialogService.ShowDialogAsync<CommentView, CommentViewModel>(vm);
        Issue = await _issueApiService.GetByIdAsync(Issue.IssueId);
    }
    public async Task LoadIssueAsync(int issueId)
    {
        Issue = await _issueApiService.GetByIdAsync(issueId);
    }

}
