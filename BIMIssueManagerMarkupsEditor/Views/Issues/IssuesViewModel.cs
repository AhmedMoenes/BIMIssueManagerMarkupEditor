namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    public partial class IssuesViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IssueApiService _issueApiService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserSessionService _userSession;
        private readonly Func<int,CommentViewModel> _CommentVmFactory;
        public IssuesViewModel(IssueApiService issueApiService, 
                               UserSessionService userSession, 
                               ProjectApiService projectApiService,
                               Func<int,CommentViewModel> CommentVmFactory,
                               IDialogService dialogService)
        {
            _issueApiService = issueApiService;
            _userSession = userSession;
            _projectApiService = projectApiService;
            _CommentVmFactory = CommentVmFactory;
            _dialogService = dialogService;

            LoadIssuesAsync();
            LoadProjectsAsync();
            LoadPriorities();
            LoadRevitVersions();

            ApplyFilterCommand = new RelayCommand(async () => await FilterIssuesAsync());
            ResetFilterCommand = new RelayCommand(async () => await LoadIssuesAsync());
            AddCommentCommand = new RelayCommand<IssueDto>(async issue => await CreateCommentAsync(issue.IssueId));
        }

        [ObservableProperty] private ObservableCollection<IssueDto> issues = new();

        [ObservableProperty] private ObservableCollection<string> projects = new(); 

        [ObservableProperty] private string selectedProject;

        [ObservableProperty] private ObservableCollection<string> assignedToUser = new();

        [ObservableProperty] private string selectedAssignee;

        [ObservableProperty] private ObservableCollection<string> priorities = new();

        [ObservableProperty] private Priority selectedPriority;

        [ObservableProperty] private DateTime? selectedDate;

        [ObservableProperty] private ObservableCollection<string> revitVersionOptions = new();

        [ObservableProperty] private string selectedRevitVersion;

        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand AddCommentCommand { get; }

        private void LoadPriorities()
        {
            Priorities = new ObservableCollection<string>(Enum.GetNames(typeof(Priority)));
        }
        private void LoadRevitVersions()
        {
            RevitVersionOptions = new ObservableCollection<string>
            {
                "2021", "2022", "2023", "2024","2025","2026"
            };
        }
        private async void LoadProjectsAsync()
        {
            if (string.IsNullOrEmpty(_userSession.UserId))
            {
                MessageBox.Show("User ID is null. Session not initialized.");
                return;
            }

            IEnumerable<ProjectDto> projects = await _projectApiService.GetProjectsByUserIdAsync(_userSession.UserId);
            Projects = new ObservableCollection<string>();

            foreach (ProjectDto project in projects)
            {
                Projects.Add(project.ProjectName);
            }
        }
        private async Task LoadIssuesAsync()
        {
            IEnumerable<IssueDto> allIssues = await _issueApiService.GetIssuesByUserIdAsync(_userSession.UserId);
            Issues = new ObservableCollection<IssueDto>(allIssues);
        }
        private async Task FilterIssuesAsync()
        {
            IEnumerable<IssueDto> allIssues = await _issueApiService.GetIssuesByUserIdAsync(_userSession.UserId);
            IEnumerable<IssueDto> filtered = allIssues;

            if (!string.IsNullOrEmpty(SelectedProject))
                filtered = filtered.Where(i => i.ProjectName == SelectedProject);

            if (SelectedPriority != null)
                filtered = filtered.Where(i => i.Priority == SelectedPriority);

            if (SelectedDate != null)
                filtered = filtered.Where(i => i.CreatedAt.Date == SelectedDate.Value.Date);

            Issues = new ObservableCollection<IssueDto>(filtered);
        }

        private async Task CreateCommentAsync(int IssueId)
        {
            CommentViewModel vm = _CommentVmFactory(IssueId);
            await vm.LoadIssueCommentsAsync();
            await _dialogService.ShowDialogAsync<CommentView, CommentViewModel>(vm);
        }
    }
}
