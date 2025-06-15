public partial class IssueDetailsViewModel : ObservableObject
{
    private readonly IssueApiService _issueApiService;

    public IssueDetailsViewModel(IssueApiService issueApiService)
    {
        _issueApiService = issueApiService;
    }

    [ObservableProperty]
    private IssueDto? issue;

    [RelayCommand]
    private async Task MarkAsResolved()
    {
        if (Issue == null || Issue.IsResolved) return;

        Issue.IsResolved = true;
        await _issueApiService.UpdateResolvedStatusAsync(Issue.IssueId, true);

        OnPropertyChanged(nameof(Issue));
    }


    [RelayCommand]
    private async Task DeleteIssue()
    {
        if (Issue == null) return;
        await _issueApiService.DeleteAsync(Issue.IssueId);
        MessageBox.Show("Issue deleted.");
    }

    [RelayCommand]
    private void EditIssue()
    {
        MessageBox.Show("Edit Issue flow to be implemented...");
        // Possibly launch another dialog with pre-filled form
    }

    public async Task LoadIssueAsync(int issueId)
    {
        Issue = await _issueApiService.GetByIdAsync(issueId);
    }
}
