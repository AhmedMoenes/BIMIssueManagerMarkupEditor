namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class Company
    {
        public static string Base => "api/companies";
        public static string GetAll() => Base;
        public static string GetById(int companyId) => $"{Base}/{companyId}";
        public static string GetCompanyUsers (int companyId) => $"{Base}/company-users/{companyId}";
        public static string Delete(int id) => $"{Base}/delete/{id}";
        public static string Create() => $"{Base}/create";
        public static string CreateWithAdmin() => $"{Base}/create-with-admin";
        public static string GetCompanyOverviewForUser(string userId) => $"{Base}/overview/{userId}";

    }
}
