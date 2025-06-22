using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class AddTeamMemberViewModel : ObservableObject, IDialogAware
    {
        private readonly UserSessionService _userSession;
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        private readonly UserApiService _userApiService;
        public event Action? RequestClose;

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
        [ObservableProperty] private ProjectTeamMemberDto selectedMember;
        [ObservableProperty] private ProjectOverviewDto selectedProject;

        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);

        [RelayCommand] private async Task CreateUserAsync(PasswordBox passwordBox)
        {
            newUser.CompanyId = _userSession.CurrentUser.CompanyId;
            newUser.Password = passwordBox.Password;
            await _userApiService.RegisterUserAsync(newUser);
            newUser = new RegisterUserDto();
            newUser.Password = null;

            await LoadUsersAsync();
            RequestClose.Invoke();
            
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
 
    }
}
