namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public enum AppIcon
    {
        Logo,
        User,
        Markup,
        Profile,
        Projects,
        Company,
        Teams,
        Issues,
        Chat,
        Viewer
    }

    public static class IconPaths
    {
        private const string BaseUrl = "https://localhost:44374/icons";
        public static string Logo => $"{BaseUrl}/logo.png";
        public static string User => $"{BaseUrl}/user.png";
        public static string Markup => $"{BaseUrl}/markup.png";
        public static string Profile => $"{BaseUrl}/profile.png";
        public static string Projects => $"{BaseUrl}/projects.png";
        public static string Company => $"{BaseUrl}/company.png";
        public static string Teams => $"{BaseUrl}/teams.png";
        public static string Issues => $"{BaseUrl}/issues.png";
        public static string Chat => $"{BaseUrl}/chat.png";
        public static string Viewer => $"{BaseUrl}/viewer.png";

        public static string GetIcon(AppIcon icon) => icon switch
        {
            AppIcon.Logo => Logo,
            AppIcon.User => User,
            AppIcon.Markup => Markup,
            AppIcon.Profile => Profile,
            AppIcon.Projects => Projects,
            AppIcon.Company => Company,
            AppIcon.Teams => Teams,
            AppIcon.Issues => Issues,
            AppIcon.Chat => Chat,
            AppIcon.Viewer => Viewer
        };
    }
}