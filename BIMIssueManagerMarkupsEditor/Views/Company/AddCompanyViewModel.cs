using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Company
{
    public partial class AddCompanyViewModel : ObservableObject, IDialogAware
    {
        private readonly CompanyApiService _companyApiService;
        private readonly UserSessionService _userSession;
        public AddCompanyViewModel(CompanyApiService companyApiService,
                                   UserSessionService userSession)
        {
            _companyApiService = companyApiService;
            _userSession = userSession;
            Company = new CreateCompanyWithAdminDto();

            LoadCompaniesAsync();
        }

        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> companies = new();
        [ObservableProperty] private CreateCompanyWithAdminDto company;

        [RelayCommand] private async Task CreateCompanyAsync(PasswordBox passwordBox)
        {
            Company.Password = passwordBox.Password;
            await _companyApiService.CreateCompanyWithAdminAsync(Company);
            Company.Password = null;
            Company = new CreateCompanyWithAdminDto();

            RequestClose?.Invoke();
            await LoadCompaniesAsync();

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

        public event Action? RequestClose;
    }
}
