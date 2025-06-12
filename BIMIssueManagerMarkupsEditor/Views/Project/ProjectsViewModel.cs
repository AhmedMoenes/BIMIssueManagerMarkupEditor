using HandyControl.Tools.Extension;

namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class ProjectsViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;
        private readonly UserSessionService _userSession;
        private readonly ProjectApiService _projectApiService;

        public ProjectsViewModel(UserSessionService userSession,
                                 ProjectApiService projectApiService,
                                 IServiceProvider serviceProvider) 
            
        {
            _userSession = userSession;
            _projectApiService = projectApiService;
            _serviceProvider = serviceProvider;

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

        [RelayCommand] private async Task OpenAddProjectViewAsync()
        {
            var addProjectWindow = _serviceProvider.GetRequiredService<AddProjectView>();
            var addProjectWindowViewModel = _serviceProvider.GetRequiredService<AddProjectViewModel>();
            addProjectWindow.DataContext = addProjectWindowViewModel;
            addProjectWindow.Show();
        }
    }
}
