namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class AssignUserToProjectViewModel : ObservableObject, IDialogAware
    {
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserSessionService _userSession;
        public event Action? RequestClose;
        public AssignUserToProjectViewModel(ProjectTeamMemberApiService projectTeamMemberService,
                                            ProjectApiService projectApiService,
                                            UserSessionService userSession)
        {
            _projectTeamMemberService = projectTeamMemberService;
            _projectApiService = projectApiService;
            _userSession = userSession;

            LoadProjectsAsync();
            LoadUsersAsync();
        }

        [ObservableProperty] private ObservableCollection<ProjectTeamMemberDto> teamMembers = new();
        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ProjectTeamMemberDto selectedMember;
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
                UserId = selectedMember.UserId,
                ProjectId = SelectedProject.ProjectId,
                Role = selectedMember.Role
            };

            await _projectTeamMemberService.AssignUserToProjectsAsync(dto);
            LoadUsersAsync();
            LoadProjectsAsync();
            selectedMember = null;
            selectedProject = null;

            RequestClose.Invoke();
        }

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

            TeamMembers = new ObservableCollection<ProjectTeamMemberDto>(members);
        }

        private async void LoadProjectsAsync()
        {
            IEnumerable<ProjectOverviewDto> companyProjects = Enumerable.Empty<ProjectOverviewDto>();

            companyProjects = await _projectApiService.GetForCompanyAsync(_userSession.CurrentUser.CompanyId);
            Projects = new ObservableCollection<ProjectOverviewDto>(companyProjects);
        }

    }
}
