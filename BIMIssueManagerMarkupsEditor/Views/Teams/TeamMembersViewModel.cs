namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class TeamMembersViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;
        private readonly UserSessionService _userSession;
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserApiService _userApiService;
        public TeamMembersViewModel(ProjectTeamMemberApiService projectTeamMemberService,
                                    UserSessionService userSession,
                                    IServiceProvider serviceProvider,
                                    IDialogService dialogService,
                                    UserApiService userApiService,
                                    ProjectApiService projectApiService)
        {
            _projectTeamMemberService = projectTeamMemberService;
            _userSession = userSession;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
            _userApiService = userApiService;
            _projectApiService = projectApiService;

            LoadUsersAsync();
        }

        private ObservableCollection<ProjectTeamMemberDto> allMembers = new();
        [ObservableProperty] private ObservableCollection<ProjectTeamMemberDto> teamMembers = new();
        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ProjectTeamMemberDto selectedMember;
        [ObservableProperty] private string searchQuery;
        [ObservableProperty] private int totalMembers;
        [ObservableProperty] private int totalProjects;

        [ObservableProperty] private IEnumerable<ISeries> chartSeries;
        public bool IsSuperAdmin => _userSession.IsInRole("SuperAdmin");
        public bool IsCompanyAdmin => _userSession.IsInRole("CompanyAdmin");
      
        [RelayCommand] private async Task OpenAddTeamMemberViewAsync()
        {
            AddTeamMemberViewModel addTeamMemberViewModel = _serviceProvider.GetRequiredService<AddTeamMemberViewModel>();
            await _dialogService.ShowDialogAsync<AddTeamMemberView, AddTeamMemberViewModel>(addTeamMemberViewModel);
        }

        [RelayCommand] private async Task OpenAssignToProjectView()
        {
            AssignUserToProjectViewModel assignUserToProjectViewModel = _serviceProvider.GetRequiredService<AssignUserToProjectViewModel>();
            await _dialogService.ShowDialogAsync<AssignUserToProjectView, AssignUserToProjectViewModel>(assignUserToProjectViewModel);
            LoadUsersAsync();
            LoadProjectsAsync();
        }

        [RelayCommand]
        private async Task DeleteSelectedMemberAsync()
        {
            if (selectedMember == null)
                return;

            await _userApiService.DeleteAsync(selectedMember.UserId);
            selectedMember = null;
            LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            IEnumerable<ProjectTeamMemberDto> members = Enumerable.Empty<ProjectTeamMemberDto>();

            if (_userSession.IsInRole("SuperAdmin"))
            {
                members = await _projectTeamMemberService.GetAll();
            }
            else if (_userSession.IsInRole("CompanyAdmin"))
            {
                var projects = await _projectApiService.GetForCompanyAsync(_userSession.CurrentUser.CompanyId);
                List<ProjectTeamMemberDto> companyMembers = new();

                foreach (var project in projects)
                {
                    var projectMembers = await _projectTeamMemberService.GetByProjectAsync(project.ProjectId);
                    companyMembers.AddRange(projectMembers);
                }

                members = companyMembers;
            }
            else
            {
                members = await _projectTeamMemberService.GetByUserAsync(_userSession.UserId);
            }

            TeamMembers = new ObservableCollection<ProjectTeamMemberDto>(members);
            UpdateStatistics();
        }

        private async Task LoadProjectsAsync()
        {
            IEnumerable<ProjectOverviewDto> companyProjects = Enumerable.Empty<ProjectOverviewDto>();
            companyProjects = await _projectApiService.GetForCompanyAsync(_userSession.CurrentUser.CompanyId);

            Projects = new ObservableCollection<ProjectOverviewDto>(companyProjects);
        }

        [RelayCommand]
       private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                TeamMembers = new ObservableCollection<ProjectTeamMemberDto>(allMembers);
                UpdateStatistics();
                UpdateCharts();
            }
            else
            {
                IEnumerable<ProjectTeamMemberDto> filtered = allMembers
                    .Where(p =>
                        (!string.IsNullOrEmpty(p.FullName) && p.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(p.Role) && p.Role.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(p.ProjectName) && p.ProjectName.Contains(query, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                TeamMembers = new ObservableCollection<ProjectTeamMemberDto>(filtered);
                UpdateStatistics();
                UpdateCharts();
            }
        }
        partial void OnSearchQueryChanged(string value)
        {
            Search(value);
        }
        private void UpdateStatistics()
        {
            TotalMembers = TeamMembers?.Count ?? 0;
            TotalProjects = TeamMembers?.Select(t => t.ProjectName).Distinct().Count() ?? 0;
            UpdateCharts();
        }

        partial void OnTeamMembersChanged(ObservableCollection<ProjectTeamMemberDto> value)
        {
            UpdateStatistics();
        }

        private void UpdateCharts()
        {
            var grouped = TeamMembers?
                              .GroupBy(t => t.ProjectName)
                              .ToDictionary(g => g.Key, g => g.Count())
                          ?? new Dictionary<string, int>();

            ChartSeries = grouped
                .Select(kvp => new PieSeries<int>
                {
                    Values = new[] { kvp.Value },
                    Name = kvp.Key
                })
                .Cast<ISeries>()
                .ToArray();
        }
    }
}
