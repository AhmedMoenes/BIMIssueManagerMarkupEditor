﻿namespace BIMIssueManagerMarkupsEditor.Views.Project
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

        [ObservableProperty] private int totalProjects;
        [ObservableProperty] private int totalIssues;
        [ObservableProperty] private int totalMembers;

        [ObservableProperty] private IEnumerable<ISeries> chartSeries;
        public bool HasChartData => ChartSeries?.OfType<PieSeries<int>>()
            .Any(s => s.Values?.Sum() > 0) == true;
        partial void OnChartSeriesChanged(IEnumerable<ISeries> value)
        {
            OnPropertyChanged(nameof(HasChartData));
        }
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
            UpdateStatistics();
            UpdateCharts();
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
            await LoadProjectsAsync();
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
                UpdateStatistics();
                UpdateCharts();
            }
            else
            {
                IEnumerable<ProjectOverviewDto> filtered = allProjects
                    .Where(p => p.ProjectName.Contains(query, StringComparison.OrdinalIgnoreCase)
                                             || p.Description?.Contains(query, StringComparison.OrdinalIgnoreCase)
                                             == true)
                                             .ToList();
                UpdateStatistics();
                UpdateCharts();

                Projects = new ObservableCollection<ProjectOverviewDto>(filtered);
            }
        }
        partial void OnSearchQueryChanged(string value)
        {
            Search(value);
        }

        private void UpdateStatistics()
        {
            TotalProjects = Projects?.Count ?? 0;
            TotalIssues = Projects?.Sum(p => p.IssuesCount) ?? 0;
            TotalMembers = Projects?.Sum(p => p.MembersCount) ?? 0;
            UpdateCharts();
        }

        partial void OnProjectsChanged(ObservableCollection<ProjectOverviewDto> value)
        {
            UpdateStatistics();
        }

        private void UpdateCharts()
        {
            ChartSeries = Projects?
                .Select(p => new PieSeries<int>
                {
                    Values = new[] { p.IssuesCount },
                    Name = p.ProjectName
                })
                .Cast<ISeries>()
                .ToArray() ?? Array.Empty<ISeries>();
        }
    }
}
