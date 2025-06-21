using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Company
{
    public partial class AddCompanyViewModel : ObservableObject, IDialogAware
    {
        private readonly CompanyApiService _companyApiService;
        private readonly UserSessionService _userSession;
        public event Action? RequestClose;
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

        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);

        [RelayCommand] private async Task CreateCompanyAsync(PasswordBox passwordBox)
        {
            Company.Password = passwordBox.Password;
            await _companyApiService.CreateCompanyWithAdminAsync(Company);
            Company.Password = null;
            Company = new CreateCompanyWithAdminDto();

            await LoadCompaniesAsync();
            RequestClose?.Invoke();

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
