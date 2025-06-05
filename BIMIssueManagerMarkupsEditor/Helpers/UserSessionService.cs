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
                Role = loginResponse.Role
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

        //public static string? GetEmailFromToken()
        //{
        //    if (string.IsNullOrWhiteSpace(Token)) return null;

        //    var parts = Token.Split('.');
        //    if (parts.Length < 2) return null;

        //    var payload = parts[1];
        //    payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

        //    var bytes = Convert.FromBase64String(payload);
        //    var json = Encoding.UTF8.GetString(bytes);

        //    var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        //    return claims != null && claims.TryGetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out var email)
        //        ? email?.ToString()
        //        : null;
        //}

        //public static async Task<bool> LoadUserIdFromTokenAsync(HttpClient client)
        //{
        //    string? email = GetEmailFromToken();
        //    if (string.IsNullOrWhiteSpace(email)) return false;

        //    try
        //    {
        //        var response = await client.GetAsync($"api/Users/get-id-by-email/{email}");
        //        if (!response.IsSuccessStatusCode) return false;

        //        UserId = await response.Content.ReadAsStringAsync();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }
}
