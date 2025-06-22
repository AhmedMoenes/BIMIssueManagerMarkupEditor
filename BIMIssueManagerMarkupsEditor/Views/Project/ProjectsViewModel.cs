namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class ProjectsViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private IServiceProvider _serviceProvider;
        private readonly UserSessionService _userSession;
        private readonly ProjectApiService _projectApiService;

        public ProjectsViewModel(UserSessionService userSession,
                                 ProjectApiService projectApiService,
                                 IServiceProvider serviceProvider, 
                                 IDialogService dialogService) 
            
        {
            _userSession = userSession;
            _projectApiService = projectApiService;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;

            LoadProjectsAsync();
        }

        private ObservableCollection<ProjectOverviewDto> allProjects = new();
        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ProjectOverviewDto selectedProject;
        [ObservableProperty] private string searchQuery;
        public bool IsSuperAdmin => _userSession.IsInRole("SuperAdmin");

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

            allProjects = new ObservableCollection<ProjectOverviewDto>(userProjects);
            Projects = new ObservableCollection<ProjectOverviewDto>(userProjects);
        }
        [RelayCommand] private async Task OpenAddProjectViewAsync()
        {
            AddProjectViewModel addProjectViewModel = _serviceProvider.GetRequiredService<AddProjectViewModel>();
            await _dialogService.ShowDialogAsync<AddProjectView, AddProjectViewModel>(addProjectViewModel);
            await LoadProjectsAsync();
        }
        [RelayCommand] private async Task OpenAssignCompaniesView()
        {
            AssignCompaniesToProjectViewModel assignCompaniesToProjectViewModel = _serviceProvider.GetRequiredService<AssignCompaniesToProjectViewModel>();
            await _dialogService.ShowDialogAsync<AssignCompaniesToProjectView, AssignCompaniesToProjectViewModel>(assignCompaniesToProjectViewModel);
        }
        [RelayCommand] private async Task DeleteSelectedProjectAsync()
        {
            if (selectedProject == null)
                return;

            await _projectApiService.DeleteAsync(selectedProject.ProjectId);
            selectedProject = null;
            await LoadProjectsAsync();
        }
        [RelayCommand] private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))   
            {
                Projects = new ObservableCollection<ProjectOverviewDto>(allProjects);
            }
            else
            {
                IEnumerable<ProjectOverviewDto> filtered = allProjects
                    .Where(p => p.ProjectName.Contains(query, StringComparison.OrdinalIgnoreCase)
                                             || p.Description?.Contains(query, StringComparison.OrdinalIgnoreCase)
                                             == true)
                                             .ToList();

                Projects = new ObservableCollection<ProjectOverviewDto>(filtered);
            }
        }
        partial void OnSearchQueryChanged(string value)
        {
            Search(value);
        }
    }
}
