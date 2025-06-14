namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class CompanyProject
    {
        public static string Base => "api/companyproject";
        public static string AssignCompanies() => $"{Base}/assign-companies";
        public static string GetForCompany(int companyId) => $"{Base}/overview/company/{companyId}";

    }
}
