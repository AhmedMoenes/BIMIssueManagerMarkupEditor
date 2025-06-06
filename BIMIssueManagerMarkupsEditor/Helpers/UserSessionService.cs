using DTOs.Login;
using DTOs.Users;

namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public class UserSessionService
    {
        public CurrentUserDto? CurrentUser { get; private set; }
        public string? Token { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
        public bool IsInRole(string role) =>
            string.Equals(CurrentUser?.Role, role, StringComparison.OrdinalIgnoreCase);

        public void SetSession(LoginResponseDto loginResponse)
        {
            if (loginResponse == null)
                throw new ArgumentNullException(nameof(loginResponse));

            Token = loginResponse.Token;
            CurrentUser = new CurrentUserDto
            {
                UserId = loginResponse.UserId,
                FullName = loginResponse.FullName,
                Email = loginResponse.Email,
                Role = loginResponse.Role,
                CompanyId = loginResponse.CompanyId,
                CompanyName = loginResponse.CompanyName,
                AssignedIssues = loginResponse.AssignedIssues,
                CreatedIssues = loginResponse.CreatedIssues,
                CreatedOn = loginResponse.CreatedOn,
                ProjectMemberships = loginResponse.Projects,
                ProjectsIncludedCount = loginResponse.ProjectsIncludedCount,
                IssuesCreatedCount = loginResponse.IssuesCreatedCount,
                IssuesAssignedCount = loginResponse.IssuesAssignedCount
            };
        }
        public void Clear()
        {
            Token = null;
            CurrentUser = null;
        }

        public string? UserId => CurrentUser?.UserId;
        public string? Email => CurrentUser?.Email;
        public string? Role => CurrentUser?.Role;
    }
}
