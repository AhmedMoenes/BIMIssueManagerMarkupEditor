using BIMIssueManagerMarkupsEditor.ViewModels.Issues;

namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    public partial class IssuesViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IssueApiService _issueApiService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserSessionService _userSession;
        private readonly Func<int,CommentViewModel> _CommentVmFactory;
        public IssuesViewModel(IssueApiService issueApiService, 
                               UserSessionService userSession,
                               ProjectApiService projectApiService,
                               Func<int, CommentViewModel> CommentVmFactory,
                               IDialogService dialogService,
                               IServiceProvider serviceProvider)
        {
            _issueApiService = issueApiService;
            _userSession = userSession;
            _projectApiService = projectApiService;
            _CommentVmFactory = CommentVmFactory;
            _dialogService = dialogService;
            _serviceProvider = serviceProvider;

            OpenTabs.Add(new IssuesListTabViewModel(this, issueApiService, userSession, serviceProvider, projectApiService));
            LoadIssuesAsync();
            LoadProjectsAsync();
            LoadPriorities();

            ApplyFilterCommand = new RelayCommand(async () => await FilterIssuesAsync());
            ResetFilterCommand = new RelayCommand(async () => await LoadIssuesAsync());
            AddCommentCommand = new RelayCommand<IssueDto>(async issue => await CreateCommentAsync(issue.IssueId));
            OpenIssueDetailsViewCommand = new RelayCommand<IssueDto>(async issue => await OpenIssueDetailsViewAsync(issue.IssueId));
            CloseIssueTabCommand = new RelayCommand<IssueDetailsViewModel>(vm => { if (vm != null) OpenIssueTabs.Remove(vm); });
        }

        [ObservableProperty] private ObservableCollection<IssueDto> issues = new();
        [ObservableProperty] private ObservableCollection<IssueDetailsViewModel> openIssueTabs = new();
        [ObservableProperty] private ObservableCollection<object> openTabs = new();
        [ObservableProperty] private ObservableCollection<string> projects = new(); 
        [ObservableProperty] private string selectedProject;
        [ObservableProperty] private ObservableCollection<string> assignedToUser = new();
        [ObservableProperty] private string selectedAssignee;
        [ObservableProperty] private ObservableCollection<string> priorities = new();
        [ObservableProperty] private Priority selectedPriority;
        [ObservableProperty] private DateTime? selectedDate;

        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand AddCommentCommand { get; }
        public ICommand OpenIssueDetailsViewCommand { get; }
        public ICommand CloseIssueTabCommand { get; }

        private void LoadPriorities()
        {
            Priorities = new ObservableCollection<string>(Enum.GetNames(typeof(Priority)));
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
        private async Task OpenIssueDetailsViewAsync (int Id)
        {
            var existingTab = OpenTabs.OfType<IssueDetailsViewModel>().FirstOrDefault(t => t.Issue?.IssueId == Id);
            if (existingTab != null)
            {
                // Tab already exists, just select it
                return;
            }

            IssueDetailsViewModel vm = _serviceProvider.GetRequiredService<IssueDetailsViewModel>();
            await vm.LoadIssueAsync(Id);
            OpenTabs.Add(vm);
        }
        [RelayCommand] private void CloseTab(object tab)
        {
            // Don't allow closing the first tab (issues list)
            if (tab is IssuesListTabViewModel)
                return;

            if (tab is IssueDetailsViewModel issueDetailsVm)
            {
                OpenTabs.Remove(issueDetailsVm);
            }
        }

    }
}
