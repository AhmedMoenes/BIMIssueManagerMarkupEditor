namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public enum AppIcon
    {
        Logo,
        LogoIn,
        User,
        Markup,
        Profile,
        Projects,
        Company,
        Teams,
        Issues,
        Chat,
        Viewer,
        AIAssistant,
        Editor
    }

    public static class IconPaths
    {
        private const string BasePath = "pack://application:,,,/Resources/Icons";
        public static string Logo => $"{BasePath}/logo.png";
        public static string LogoIn => $"{BasePath}/logo-in.png";
        public static string User => $"{BasePath}/user.png";
        public static string Markup => $"{BasePath}/markup.png";
        public static string Profile => $"{BasePath}/profile.png";
        public static string Projects => $"{BasePath}/projects.png";
        public static string Company => $"{BasePath}/company.png";
        public static string Teams => $"{BasePath}/teams.png";
        public static string Issues => $"{BasePath}/issues.png";
        public static string Chat => $"{BasePath}/chat.png";
        public static string Viewer => $"{BasePath}/viewer.png";
        public static string AIAssistant =>$"{BasePath}/AIAssistant.png";
        public static string IssueViewer => $"{BasePath}/IssueViewer.png";

        public static string GetIcon(AppIcon icon) => icon switch
        {
            AppIcon.Logo => Logo,
            AppIcon.LogoIn => LogoIn,
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