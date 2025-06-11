using DTOs.Areas;
using DTOs.Companies;
using DTOs.CompanyProject;
using DTOs.Labels;

namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    public partial class AddProjectViewModel : ObservableObject
    {
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
            if (Project == null || string.IsNullOrWhiteSpace(Project.ProjectName))
                return;
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
        }

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
            await LoadCompaniesAsync();

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
