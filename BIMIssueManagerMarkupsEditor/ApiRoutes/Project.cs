namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class Project
    {
        public static string Base => "api/projects";
        public static string GetAll() => Base;
        public static string GetById(int projectId) => $"{Base}/{projectId}";
        public static string Create() => $"{Base}/create";
        public static string Update(int projectId) => $"{Base}/edit/{projectId}";
        public static string Delete(int projectId) => $"{Base}/delete/{projectId}";
        public static string GetForSubscriber() => $"{Base}/overview/subscriber";
        public static string GetForUser(string userId) => $"{Base}/overview/user/{userId}";
        public static string GetByUser(string userId) => $"{Base}/user/{userId}";

    }
}
