namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class User
    {
        public static string Base => "api/users";
        public static string GetAll() => Base;
        public static string GetById(int projectId) => $"{Base}/{projectId}";
        public static string Create() => $"{Base}/register";
        public static string Update(int userId) => $"{Base}/edit/{userId}";
        public static string Delete(int userId) => $"{Base}/delete/{userId}";
        public static string GetUsersByProject(int projectId) => $"{Base}/project-users/{projectId}";

    }
}
