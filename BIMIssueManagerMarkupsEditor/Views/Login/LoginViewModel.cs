using DTOs.Login;
using HandyControl.Controls;
using MessageBox = System.Windows.MessageBox;

namespace BIMIssueManagerMarkupsEditor.Views.Login
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthApiService _authApiService;
        private IServiceProvider _serviceProvider;
        public Action? CloseAction { get; set; }

        public LoginViewModel(AuthApiService authApiService, IServiceProvider serviceProvider)
        {
            _authApiService = authApiService;
            _serviceProvider = serviceProvider;
        }

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? errorMessage;

        [RelayCommand]
        private async Task LoginAsync(PasswordBox passwordbox)
        {

            IsLoading = true;
            ErrorMessage = null;

            LoginRequestDto dto = new LoginRequestDto
            {
                Email = Email,
                Password = passwordbox.Password
            };

            try
            {
                var response = await _authApiService.LoginAsync(dto);
                var status = response.StatusCode.ToString();
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string token = await response.Content.ReadAsStringAsync();
                    UserSession.Token = token;
                    UserSession.IsUserLoggedIn = true;

                    //ToReview
                    //await UserSession.LoadUserIdFromTokenAsync(new HttpClient
                    //{
                    //    BaseAddress = new Uri("https://localhost:44374/")
                    //});

                    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    mainWindow.DataContext = new MainViewModel(_serviceProvider);
                    mainWindow.Show();

                    CloseAction?.Invoke();
                }
                else
                {
                    ErrorMessage = "Invalid email or password.";
                    UserSession.IsUserLoggedIn = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                UserSession.IsUserLoggedIn = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseAction?.Invoke();
        }
    }
}
