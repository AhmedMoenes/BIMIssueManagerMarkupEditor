namespace BIMIssueManagerMarkupsEditor.ApiRoutes
{
    public static class Comment
    {
        private const string Base = "api/comments";

        public static string GetAll() => $"{Base}";
        public static string GetById(int commentId) => $"{Base}/{commentId}";
        public static string Delete(int commentId) => $"{Base}/delete/{commentId}";

        public static string Create() => $"{Base}/create";

        public static string CreateForIssue(int issueId) => $"{Base}/issue/{issueId}";
        public static string CreateForSnapshot(int snapshotId) => $"{Base}/snapshot/{snapshotId}";

        public static string GetByIssueId(int issueId) => $"{Base}/issue/{issueId}";
        public static string GetBySnapshotId(int snapshotId) => $"{Base}/snapshot/{snapshotId}";

        public static string Update(int commentId) => $"{Base}/{commentId}";
    }
}