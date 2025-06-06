using DTOs.Companies;

namespace BIMIssueManagerMarkupsEditor.Views.Company
{
    public partial class AddCompanyViewModel:ObservableObject
    {
        private readonly CompanyApiService _companyApiService;
        private readonly UserSessionService _userSession;
        public AddCompanyViewModel(CompanyApiService companyApiService, UserSessionService userSession)
        {
            _companyApiService = companyApiService;
            _userSession = userSession;

            if (_userSession.IsInRole("SuperAdmin"))
                LoadCompanies();

            if (_userSession.IsInRole("CompanyAdmin"))
                LoadUserCompanies();

        }

        [ObservableProperty] private ObservableCollection<CompanyOverviewDto> companies = new();

        private async void LoadCompanies()
        {
            IEnumerable<CompanyOverviewDto> allCompanies = await _companyApiService.GetAllCompaniesAsync();
            foreach (CompanyOverviewDto company in allCompanies)
            {
                companies.Add(company);
            }
        }

        private async void LoadUserCompanies()
        {
            IEnumerable<CompanyOverviewDto> userCompanies = await _companyApiService.GetCompanyOverviewForUserAsync(_userSession.UserId);
            foreach (CompanyOverviewDto company in userCompanies)
            {
                companies.Add(company);
            }
        }
    }
}
