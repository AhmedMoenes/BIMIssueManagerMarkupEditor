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
        [ObservableProperty] private string searchQuery;

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

        [RelayCommand]
        private async Task OpenAddCompanyViewAsync()
        {
            AddCompanyViewModel addCompanyViewModel = _serviceProvider.GetRequiredService<AddCompanyViewModel>();
            await _dialogService.ShowDialogAsync<AddCompanyView, AddCompanyViewModel>(addCompanyViewModel);

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
    }
}
