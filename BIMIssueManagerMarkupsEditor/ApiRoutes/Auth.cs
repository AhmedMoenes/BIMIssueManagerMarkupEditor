namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class Auth
    {
        public const string Base = "api/auth";
        public static string Login() => $"{Base}/login";
    }
}
