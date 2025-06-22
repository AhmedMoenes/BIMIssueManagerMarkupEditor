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
            }
            else
            {
                IEnumerable<CompanyOverviewDto> filtered = allCompanies
                    .Where(c => c.CompanyName.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                Companies = new ObservableCollection<CompanyOverviewDto>(filtered);
            }
        }
        partial void OnSearchQueryChanged(string value)
        {
            Search(value);
        }
    }
}
