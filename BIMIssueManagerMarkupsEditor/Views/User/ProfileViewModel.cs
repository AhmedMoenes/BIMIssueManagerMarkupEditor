using DTOs.Users;

namespace BIMIssueManagerMarkupsEditor.Views.User
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;
        public ProfileViewModel(UserSessionService userSession)
        {
            _userSession = userSession;
            currentUser = _userSession.CurrentUser;
        }

        public string UserIcon => IconPaths.GetIcon(AppIcon.User);

        [ObservableProperty] private CurrentUserDto currentUser = new();
    }
}
