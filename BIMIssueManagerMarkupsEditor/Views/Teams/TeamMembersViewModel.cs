namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class TeamMembersViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;
        private readonly UserSessionService _userSession;
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;

        public TeamMembersViewModel(ProjectTeamMemberApiService projectTeamMemberService,
                                    UserSessionService userSession,
                                    IServiceProvider serviceProvider,
                                    IDialogService dialogService)
        {
            _projectTeamMemberService = projectTeamMemberService;
            _userSession = userSession;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;

            LoadUsersAsync();
        }

        private ObservableCollection<ProjectTeamMemberDto> allMembers = new();
        [ObservableProperty] private ObservableCollection<ProjectTeamMemberDto> teamMembers = new();

        private async void LoadUsersAsync()
        {
            IEnumerable<ProjectTeamMemberDto> members = Enumerable.Empty<ProjectTeamMemberDto>();

            if (_userSession.IsInRole("SuperAdmin"))
            {
                members = await _projectTeamMemberService.GetAll();
            }
            else
            {
                members = await _projectTeamMemberService.GetByUserAsync(_userSession.UserId);
            }

            allMembers = new ObservableCollection<ProjectTeamMemberDto>(members);
            TeamMembers = new ObservableCollection<ProjectTeamMemberDto>(members);
        }

        [RelayCommand] private async Task OpenAddTeamMemberViewAsync()
        {
            AddTeamMemberViewModel addTeamMemberViewModel = _serviceProvider.GetRequiredService<AddTeamMemberViewModel>();
            await _dialogService.ShowDialogAsync<AddTeamMemberView, AddTeamMemberViewModel>(addTeamMemberViewModel);
        }

        [RelayCommand] private async Task OpenAssignToProjectView()
        {
            AssignUserToProjectViewModel assignUserToProjectViewModel = _serviceProvider.GetRequiredService<AssignUserToProjectViewModel>();
            await _dialogService.ShowDialogAsync<AssignUserToProjectView, AssignUserToProjectViewModel>(assignUserToProjectViewModel);
        }

        [RelayCommand]
       private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                TeamMembers = new ObservableCollection<ProjectTeamMemberDto>(allMembers);
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
            }
        }
    }
}
