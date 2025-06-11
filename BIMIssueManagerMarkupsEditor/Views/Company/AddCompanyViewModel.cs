using DTOs.Companies;

namespace BIMIssueManagerMarkupsEditor.Views.Company
{
    public partial class AddCompanyViewModel : ObservableObject
    {
        private readonly CompanyApiService _companyApiService;
        private readonly UserSessionService _userSession;
        public AddCompanyViewModel(CompanyApiService companyApiService, UserSessionService userSession)
        {
            _companyApiService = companyApiService;
            _userSession = userSession;

            LoadCompaniesAsync();
        }

        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> companies = new();

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
