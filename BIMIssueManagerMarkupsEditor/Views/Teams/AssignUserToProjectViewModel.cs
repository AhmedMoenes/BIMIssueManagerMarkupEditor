namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class AssignUserToProjectViewModel : ObservableObject, IDialogAware
    {
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserSessionService _userSession;
        private readonly UserApiService _userApiService;
        public event Action? RequestClose;
        public AssignUserToProjectViewModel(ProjectTeamMemberApiService projectTeamMemberService,
                                            ProjectApiService projectApiService,
                                            UserSessionService userSession,
                                            UserApiService userApiService)
        {
            _projectTeamMemberService = projectTeamMemberService;
            _projectApiService = projectApiService;
            _userSession = userSession;
            _userApiService = userApiService;

            LoadProjectsAsync();
            LoadUsersAsync();
        }

        [ObservableProperty] private ObservableCollection<CompanyUserDto> members = new();
        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private CompanyUserDto selectedMember;
        [ObservableProperty] private ProjectOverviewDto selectedProject;

        [RelayCommand]
        private async Task AssignToProjectsAsync()
        {
            if (SelectedMember == null || SelectedProject == null)
            {
                return;
            }

            AssignUserToProjectDto dto = new AssignUserToProjectDto
            {
                UserId = selectedMember.Id,
                ProjectId = SelectedProject.ProjectId,
                Role = selectedMember.Role
            };

            await _projectTeamMemberService.AssignUserToProjectsAsync(dto);
            selectedMember = null;
            selectedProject = null;
            await LoadUsersAsync();
            await LoadProjectsAsync();
            RequestClose.Invoke();
        }

        private async Task LoadUsersAsync()
        {
            IEnumerable<CompanyUserDto> allMembers = Enumerable.Empty<CompanyUserDto>();
            allMembers = await _userApiService.GetCompanyUsers(_userSession.CurrentUser.CompanyId);

            Members = new ObservableCollection<CompanyUserDto>(allMembers);
        }

        private async Task LoadProjectsAsync()
        {
            IEnumerable<ProjectOverviewDto> companyProjects = Enumerable.Empty<ProjectOverviewDto>();

            companyProjects = await _projectApiService.GetForCompanyAsync(_userSession.CurrentUser.CompanyId);
            Projects = new ObservableCollection<ProjectOverviewDto>(companyProjects);
        }

    }
}
