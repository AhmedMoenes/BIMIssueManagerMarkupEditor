using DTOs.Login;
using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Login
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthApiService _authApiService;
        private IServiceProvider _serviceProvider;

        public LoginViewModel(AuthApiService authApiService, IServiceProvider serviceProvider, UserSessionService userSession)
        {
            _authApiService = authApiService;
            _serviceProvider = serviceProvider;
        }
        public Action? CloseAction { get; set; }
        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);

        [ObservableProperty] private string email = "amunes.f98@gmail.com";
        [ObservableProperty] private string password = "Issue@33";
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
    }
}
