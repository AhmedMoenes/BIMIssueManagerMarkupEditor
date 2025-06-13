namespace BIMIssueManagerMarkupsEditor.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            loginViewModel.CloseAction = this.Close;
            PasswordBox.Password = "Sasa@123";
            DataContext = loginViewModel;
        }
    }
}
