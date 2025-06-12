namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class ProjectsViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;
        private readonly ProjectApiService _projectApiService;
        public ProjectsViewModel(UserSessionService userSession,
                                 ProjectApiService projectApiService)
            
        {
            _userSession = userSession;
            _projectApiService = projectApiService;

            LoadProjectsAsync();
        }

        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private string searchQuery;

        private async Task LoadProjectsAsync()
        {
            IEnumerable<ProjectOverviewDto> userProjects = Enumerable.Empty<ProjectOverviewDto>();

            if (_userSession.Role == "SuperAdmin")
            {
                userProjects = await _projectApiService.GetForSubscriberAsync();
            }
            else if (_userSession.Role == "CompanyAdmin")
            {
                userProjects = await _projectApiService.GetForCompanyAsync(_userSession.CurrentUser.CompanyId);
            }
            else
            {
                userProjects = await _projectApiService.GetForUserAsync(_userSession.UserId);
            }

            Projects = new ObservableCollection<ProjectOverviewDto>(userProjects);
        }

        [RelayCommand] void OpenAddProjectView()
        {
            // Navigate to or open project creation view/dialog
        }
    }
}
