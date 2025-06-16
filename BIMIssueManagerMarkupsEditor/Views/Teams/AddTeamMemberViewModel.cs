using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class AddTeamMemberViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        private readonly ProjectApiService _projectApiService;
        private readonly UserApiService _userApiService;
        public AddTeamMemberViewModel(UserSessionService userSession, 
                                      ProjectTeamMemberApiService projectTeamMemberServiceApi, 
                                      UserApiService userApiService,
                                      ProjectApiService projectApiService)
        {
            _userSession = userSession;
            _projectTeamMemberService = projectTeamMemberServiceApi;
            _userApiService = userApiService;
            _projectApiService = projectApiService;

            newUser = new RegisterUserDto();
            LoadUsersAsync();
            LoadProjectsAsync();
        }

        [ObservableProperty] private ObservableCollection<UserDto> users = new();
        [ObservableProperty] private ObservableCollection<ProjectTeamMemberDto> teamMembers = new();
        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ObservableCollection<string> availableRoles = new(new[] { "Editor", "Viewer", "Reviewer" });
        [ObservableProperty] private RegisterUserDto newUser;
        [ObservableProperty] private ProjectTeamMemberDto selectedMember;
        [ObservableProperty] private ProjectOverviewDto selectedProject;

        [RelayCommand] private async Task CreateUserAsync(PasswordBox passwordBox)
        {
            newUser.CompanyId = _userSession.CurrentUser.CompanyId;
            newUser.Password = passwordBox.Password;
            await _userApiService.RegisterUserAsync(newUser);
            newUser = new RegisterUserDto();
            newUser.Password = null;
            await LoadUsersAsync();
            await LoadProjectsAsync();
        }

        private async Task LoadUsersAsync()
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

        private async Task LoadProjectsAsync()
        {
            IEnumerable<ProjectOverviewDto> companyProjects = Enumerable.Empty<ProjectOverviewDto>();

            companyProjects = await _projectApiService.GetForCompanyAsync(_userSession.CurrentUser.CompanyId);
            Projects = new ObservableCollection<ProjectOverviewDto>(companyProjects);
        }
    }
}
