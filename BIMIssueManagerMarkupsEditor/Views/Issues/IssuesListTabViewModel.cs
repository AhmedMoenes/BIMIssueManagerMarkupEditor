namespace BIMIssueManagerMarkupsEditor.ViewModels.Issues
{
    public class IssuesListTabViewModel : ObservableObject
    {
        private readonly IssuesViewModel _parentViewModel;
        private readonly IssueApiService _issueApiService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserSessionService _userSession;
        private readonly Func<int, CommentViewModel> _CommentVmFactory;

        public IssuesListTabViewModel(IssuesViewModel parentViewModel, 
                                      IssueApiService issueApiService, 
                                      UserSessionService userSession, 
                                      IServiceProvider serviceProvider, 
                                      ProjectApiService projectApiService)
        {
            _parentViewModel = parentViewModel;
            _issueApiService = issueApiService;
            _userSession = userSession;
            _projectApiService = projectApiService;

            // Initialize commands
            LoadIssuesAsync();
            ApplyFilterCommand = _parentViewModel.ApplyFilterCommand;
            ResetFilterCommand = _parentViewModel.ResetFilterCommand;
        }

        public string Title => "All Issues";

        // Expose the parent's collections
        public ObservableCollection<IssueDto> Issues;
        public ObservableCollection<string> Projects => _parentViewModel.Projects;
        public ObservableCollection<string> Priorities => _parentViewModel.Priorities;

        // Expose the parent's commands
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand OpenIssueDetailsViewCommand => _parentViewModel.OpenIssueDetailsViewCommand;

        private async Task LoadIssuesAsync()
        {
            IEnumerable<IssueDto> allIssues = await _issueApiService.GetIssuesByUserIdAsync(_userSession.UserId);
            Issues = new ObservableCollection<IssueDto>(allIssues);
        }

    }
}