using DTOs.ProjectTeamMember;
using DTOs.Users;
using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class AddTeamMemberViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        private readonly UserApiService _userApiService;
        public AddTeamMemberViewModel(UserSessionService userSession, 
                                      ProjectTeamMemberApiService projectTeamMemberServiceApi, 
                                      UserApiService userApiService)
        {
            _userSession = userSession;
            _projectTeamMemberService = projectTeamMemberServiceApi;
            _userApiService = userApiService;

            newUser = new RegisterUserDto();
            LoadUsersAsync();
        }

        [ObservableProperty] private ObservableCollection<UserDto> users = new();
        [ObservableProperty] private ObservableCollection<ProjectTeamMemberDto> teamMembers = new();
        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ObservableCollection<string> availableRoles = new(new[] { "Editor", "Viewer", "Reviewer" });
        [ObservableProperty] private RegisterUserDto newUser;
        [ObservableProperty] private UserDto selectedUser;
        [ObservableProperty] private ProjectDto selectedProject;

        [RelayCommand] private async Task CreateUserAsync(PasswordBox passwordBox)
        {
            newUser.CompanyId = _userSession.CurrentUser.CompanyId;
            newUser.Password = passwordBox.Password;
            await _userApiService.RegisterUserAsync(newUser);
            newUser = new RegisterUserDto();
            newUser.Password = null;
            LoadUsersAsync();
        }

        [RelayCommand] private async Task AssignToProjectsAsync()
        {
            if (SelectedUser == null || SelectedProject == null)
            {
                return;
            }

            var dto = new CreateProjectTeamMemberDto
            {
                ProjectId = SelectedProject.ProjectId,
                Role = selectedUser.Role
            };

            //await _projectTeamMemberService.AssignUserToProjectsAsync(dto);
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

    }
}
