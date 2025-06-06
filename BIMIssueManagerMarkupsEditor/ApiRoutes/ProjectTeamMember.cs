namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public class ProjectTeamMember
    {
        public static string Base => "api/ProjectTeamMembers";
        public static string GetByProjectId(int projectId) => $"{Base}/team-project/{projectId}";
        public static string GetTeamByUserId(string userId) => $"{Base}/team-user/{userId}";
        public static string AssignUserToProject() => $"{Base}/user-project";
        public static string RemoveUserFromProject(int projectId, string userId) => $"{Base}/project/{projectId}/user/{userId}";

    }
}
