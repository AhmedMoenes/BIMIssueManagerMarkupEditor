using DTOs.ProjectTeamMember;

namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    public partial class AddTeamMemberViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;
        private readonly ProjectTeamMemberApiService _projectTeamMemberService;
        public AddTeamMemberViewModel(UserSessionService userSession, ProjectTeamMemberApiService projectTeamMemberServiceApi)
        {
            _userSession = userSession;
            _projectTeamMemberService = projectTeamMemberServiceApi;

            LoadTeamMembersByUserId();
        }

        [ObservableProperty] private ObservableCollection<ProjectTeamMemberDto> teamMembers = new();

        private async void LoadTeamMembersByUserId()
        {
            IEnumerable<ProjectTeamMemberDto> members = await _projectTeamMemberService.GetByUserAsync(_userSession.UserId);
            foreach (ProjectTeamMemberDto member in members)
            {
                teamMembers.Add(member);
            }
        }

    }
}
