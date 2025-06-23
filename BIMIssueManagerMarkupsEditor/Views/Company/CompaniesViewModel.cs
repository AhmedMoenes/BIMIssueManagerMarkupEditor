namespace BIMIssueManagerMarkupsEditor.Views.Company
{
    public partial class CompaniesViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private IServiceProvider _serviceProvider;
        private readonly UserSessionService _userSession;
        private readonly CompanyApiService _companyApiService;
        public CompaniesViewModel(IDialogService dialogService,
                                  IServiceProvider serviceProvider,
                                  UserSessionService userSession,
                                  CompanyApiService companyApiService)
        {
            _dialogService = dialogService;
            _serviceProvider = serviceProvider;
            _userSession = userSession;
            _companyApiService = companyApiService;

            LoadCompaniesAsync();
        }

        private ObservableCollection<CompanyOverviewDto> allCompanies = new();
        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> companies = new();
        [ObservableProperty] private CompanyOverviewDto selectedCompany;
        [ObservableProperty] private string searchQuery;
        [ObservableProperty] private int totalCompanies;
        [ObservableProperty] private int totalProjects;
        [ObservableProperty] private int totalUsers;

        [ObservableProperty] private IEnumerable<ISeries> chartSeries;
        public bool IsSuperAdmin => _userSession.IsInRole("SuperAdmin");

        private async Task LoadCompaniesAsync()
        {
            IEnumerable<CompanyOverviewDto> companies = Enumerable.Empty<CompanyOverviewDto>();

            if (_userSession.Role == "SuperAdmin")
            {
                companies = await _companyApiService.GetAllCompaniesAsync();
            }
            else
            {
                companies = await _companyApiService.GetCompanyOverviewForUserAsync(_userSession.UserId);
            }

            allCompanies = new ObservableCollection<CompanyOverviewDto>(companies);
            Companies = new ObservableCollection<CompanyOverviewDto>(companies);
            UpdateStatistics();
        }

        [RelayCommand]
        private async Task OpenAddCompanyViewAsync()
        {
            AddCompanyViewModel addCompanyViewModel = _serviceProvider.GetRequiredService<AddCompanyViewModel>();
            await _dialogService.ShowDialogAsync<AddCompanyView, AddCompanyViewModel>(addCompanyViewModel);
        }

        [RelayCommand]
        private async Task DeleteSelectedCompanyAsync()
        {
            if (selectedCompany == null)
                return;

            await _companyApiService.DeleteAsync(selectedCompany.CompanyId);
            selectedCompany = null;
            await LoadCompaniesAsync();
        }

        [RelayCommand]
        private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Companies = new ObservableCollection<CompanyOverviewDto>(allCompanies);
                UpdateStatistics();
                UpdateCharts();
            }
            else
            {
                IEnumerable<CompanyOverviewDto> filtered = allCompanies
                    .Where(c => c.CompanyName.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                Companies = new ObservableCollection<CompanyOverviewDto>(filtered);
                UpdateStatistics();
                UpdateCharts();
            }
        }
        partial void OnSearchQueryChanged(string value)
        {
            Search(value);
        }

        private void UpdateStatistics()
        {
            TotalCompanies = Companies?.Count ?? 0;
            TotalProjects = Companies?.Sum(c => c.ProjectsCount) ?? 0;
            TotalUsers = Companies?.Sum(c => c.UsersCount) ?? 0;
            UpdateCharts();
        }

        partial void OnCompaniesChanged(ObservableCollection<CompanyOverviewDto> value)
        {
            UpdateStatistics();
        }

        private void UpdateCharts()
        {
            ChartSeries = Companies?
                .Select(c => new PieSeries<int>
                {
                    Values = new[] { c.ProjectsCount },
                    Name = c.CompanyName
                })
                .Cast<ISeries>()
                .ToArray() ?? Array.Empty<ISeries>();
        }
    }
}
