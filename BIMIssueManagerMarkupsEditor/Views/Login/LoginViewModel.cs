using HandyControl.Controls;
using MessageBox = HandyControl.Controls.MessageBox;

namespace BIMIssueManagerMarkupsEditor.Views.Login
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthApiService _authApiService;
        private IServiceProvider _serviceProvider;
        private readonly UserSessionService _userSession;

        public LoginViewModel(AuthApiService authApiService, IServiceProvider serviceProvider, UserSessionService userSession)
        {
            _authApiService = authApiService;
            _serviceProvider = serviceProvider;
            _userSession = userSession;
        }
        public Action? CloseAction { get; set; }
        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);

        [ObservableProperty] private string email = "amunes.f98@gmail.com";
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string? errorMessage;

        [RelayCommand] private async Task LoginAsync(PasswordBox passwordBox)
        {
            IsLoading = true;
            ErrorMessage = null;

            LoginRequestDto dto = new LoginRequestDto
            {
                Email = Email,
                Password = passwordBox.Password
            };

            try
            {
                LoginResponseDto response = await _authApiService.LoginAsync(dto);

                if (response is null || !_userSession.IsAuthenticated)
                {
                    MessageBox.Show("Invalid email or password");
                    return;
                }

                MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                MainViewModel mainWindowVM = _serviceProvider.GetRequiredService<MainViewModel>();
                mainWindow.DataContext = mainWindowVM;
                mainWindow.Show();

                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
