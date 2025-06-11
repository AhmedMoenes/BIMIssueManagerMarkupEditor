using DTOs.Login;
using HandyControl.Controls;

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

        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string? errorMessage;

        [RelayCommand] private async Task LoginAsync(PasswordBox passwordbox)
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
                LoginResponseDto response = await _authApiService.LoginAsync(dto);

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                var mainWindowVM = _serviceProvider.GetRequiredService<MainViewModel>();
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

        [RelayCommand] private void Cancel()
        {
            CloseAction?.Invoke();
        }
    }
}
