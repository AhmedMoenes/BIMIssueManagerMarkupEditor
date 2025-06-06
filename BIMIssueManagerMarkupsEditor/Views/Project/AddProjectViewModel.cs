namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class AddProjectViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;
        private readonly ProjectApiService _projectApiService;
        public AddProjectViewModel(UserSessionService userSession, ProjectApiService projectApiService)
        {
            _userSession = userSession;
            _projectApiService = projectApiService;

            LoadCompanyProjectsAsync();
        }

        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();

        private async void LoadUserProjectsAsync()
        { }

        private async void LoadCompanyProjectsAsync()
        {
            IEnumerable<ProjectOverviewDto> companyProjects = await _projectApiService.GetForCompanyAsync();
            foreach (ProjectOverviewDto project in companyProjects)
            {
                projects.Add(project);
            }
        }
        private async void LoadSubscriberProjectsAsync()
        { }
    }
}
