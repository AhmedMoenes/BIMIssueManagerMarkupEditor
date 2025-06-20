namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class AssignCompaniesToProjectViewModel : ObservableObject, IDialogAware
    {
        public event Action? RequestClose;
        private readonly ProjectApiService _projectApiService;
        private readonly CompanyApiService _companyApiService;
        private readonly UserSessionService _userSession;

        public AssignCompaniesToProjectViewModel(ProjectApiService projectApiService,
                                                 UserSessionService userSession,
                                                 CompanyApiService companyApiService)
        {
            _projectApiService = projectApiService;
            _userSession = userSession;
            _companyApiService = companyApiService;

            LoadProjectsAsync();
            LoadCompaniesAsync();
        }

        [ObservableProperty] private ObservableCollection<ProjectOverviewDto> projects = new();
        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> companies = new();
        [ObservableProperty] private ProjectOverviewDto selectedProject;
        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> selectedCompanies = new();
        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);

        [RelayCommand] async Task AssignCompaniesAsync()
        {
            if (selectedProject == null || selectedCompanies.Count == 0)
                return;

            AssignCompaniesToProjectDto dto = new AssignCompaniesToProjectDto()
            {
                ProjectId = selectedProject.ProjectId,
                CompanyIds = selectedCompanies.Select(c => c.CompanyId).ToList()
            };

            await _projectApiService.AssignCompanyToProjectAsync(dto);
            selectedProject = null;
            selectedCompanies = null;

            LoadProjectsAsync();
            RequestClose.Invoke();
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
