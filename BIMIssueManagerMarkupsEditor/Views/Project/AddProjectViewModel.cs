namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class AddProjectViewModel : ObservableObject, IDialogAware
    {
        public event Action RequestClose;
        private readonly UserSessionService _userSession;
        private readonly ProjectApiService _projectApiService;
        private readonly CompanyApiService _companyApiService;
        public AddProjectViewModel(UserSessionService userSession,
                                   ProjectApiService projectApiService,
                                   CompanyApiService companyApiService)
        {
            _userSession = userSession;
            _projectApiService = projectApiService;
            _companyApiService = companyApiService;

            Project = new CreateProjectDto();
            LoadProjectsAsync();
            LoadCompaniesAsync();
        }

        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> companies = new();
        [ObservableProperty] private CreateProjectDto project;
        [ObservableProperty] private ProjectOverviewDto selectedProject;
        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> selectedCompanies = new();
        [ObservableProperty] private ObservableCollection<CreateLabelDto> projectLabels = new();
        [ObservableProperty] private ObservableCollection<CreateAreaDto> projectAreas = new();
        [ObservableProperty] private string newLabelName;
        [ObservableProperty] private string newAreaName;

        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);

        [RelayCommand] private void AddLabel()
        {
            if (!string.IsNullOrWhiteSpace(NewLabelName))
            {
                ProjectLabels.Add(new CreateLabelDto { LabelName = NewLabelName });
                NewLabelName = string.Empty;
            }
        }
        [RelayCommand] private void AddArea()
        {
            if (!string.IsNullOrWhiteSpace(NewAreaName))
            {
                ProjectAreas.Add(new CreateAreaDto { AreaName = NewAreaName });
                NewAreaName = string.Empty;
            }
        }
        [RelayCommand] async Task CreateProjectAsync()
        {
            Project.Labels = ProjectLabels.ToList();
            Project.Areas = ProjectAreas.ToList();
            await _projectApiService.CreateProjectAsync(Project);

            Project = new CreateProjectDto
            {
                StartDate = null,
                EndDate = null
            };

            ProjectLabels.Clear();
            ProjectAreas.Clear();
            await LoadProjectsAsync();
            RequestClose?.Invoke();
        }
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
        private async Task LoadCompaniesAsync()
        {
            IEnumerable<CompanyOverviewDto> allCompanies = Enumerable.Empty<CompanyOverviewDto>();

            if (_userSession.Role == "SuperAdmin")
            {
                allCompanies = await _companyApiService.GetAllCompaniesAsync();
            }
            else
            {
                allCompanies = await _companyApiService.GetCompanyOverviewForUserAsync(_userSession.UserId);
            }

            Companies = new ObservableCollection<CompanyOverviewDto>(allCompanies);
        }
    }
}
