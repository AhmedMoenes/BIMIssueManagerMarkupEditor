namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class Issue
    {
        public const string Base = "api/issues";
        public static string GetAll() => Base;
        public static string GetById(int id) => $"{Base}/{id}";
        public static string Create() => $"{Base}/create";
        public static string Update(int id) => $"{Base}/edit/{id}";
        public static string Delete(int id) => $"{Base}/delete/{id}";
        public static string GetByProjectId(int projectId) => $"{Base}/project/{projectId}";
        public static string GetByUserId(string userId) => $"{Base}/user/{userId}";
        public static string MarkAsResolved(int id) => $"{Base}/resolve/{id}";
    }
}
