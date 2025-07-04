﻿using BIMIssueManagerMarkupsEditor.Views.Editor;

namespace BIMIssueManagerMarkupsEditor.Views.Shell
{
    public partial class MainViewModel : ObservableObject, IDialogAware
    {
        private readonly IServiceProvider _provider;
        private readonly UserSessionService _userSession;
        private readonly AuthApiService _authApiService;
        public event Action? RequestClose;

        public MainViewModel(IServiceProvider provider, UserSessionService userSession, AuthApiService authApiService, IDialogService dialogService)
        {
            _provider = provider;
            _userSession = userSession;
            _authApiService = authApiService;
            NavigateProfile();
        }
        public void RefreshUserInfo()
        {
            OnPropertyChanged(nameof(IsSuperAdmin));
            OnPropertyChanged(nameof(IsCompanyAdmin));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsUser));

            OnPropertyChanged(nameof(UserFullName));
            OnPropertyChanged(nameof(UserEmail));
            OnPropertyChanged(nameof(UserRole));
            OnPropertyChanged(nameof(UserCompany));
        }


        #region Roles
        public bool IsSuperAdmin => _userSession.IsInRole("SuperAdmin");
        public bool IsCompanyAdmin => _userSession.IsInRole("CompanyAdmin");
        public bool IsAdmin => _userSession.IsInRole("CompanyAdmin") || _userSession.IsInRole("SuperAdmin");
        public bool IsUser => !_userSession.IsInRole("SuperAdmin");
        #endregion

        #region LoggedInUserSummary
        public string UserFullName => _userSession.CurrentUser?.FullName ?? "";
        public string UserEmail => _userSession.CurrentUser?.Email ?? "";
        public string UserRole => _userSession.CurrentUser?.Role ?? "";
        public string UserCompany => _userSession.CurrentUser?.CompanyName ?? "";

        #endregion

        #region Icons
        public string LogoIcon => IconPaths.GetIcon(AppIcon.Logo);
        public string LogoInIcon => IconPaths.GetIcon(AppIcon.LogoIn);
        public string UserIcon => IconPaths.GetIcon(AppIcon.User);
        public string ProfileIcon => IconPaths.GetIcon(AppIcon.Profile);
        public string MarkupIcon => IconPaths.GetIcon(AppIcon.Markup);
        public string ProjectsIcon => IconPaths.GetIcon(AppIcon.Projects);
        public string CompanyIcon => IconPaths.GetIcon(AppIcon.Company);
        public string TeamsIcon => IconPaths.GetIcon(AppIcon.Teams);
        public string IssuesIcon => IconPaths.GetIcon(AppIcon.Issues);
        public string ChatIcon => IconPaths.GetIcon(AppIcon.Chat);
        public string ViewerIcon => IconPaths.GetIcon(AppIcon.Viewer);
        public string AIAssistantIcon => IconPaths.GetIcon(AppIcon.AIAssistant);
        public string EditorIcon => IconPaths.GetIcon(AppIcon.Editor);
        #endregion

        #region Navigation
        [ObservableProperty] private  ObservableObject currentView;

        [RelayCommand]
        private void NavigateMarkup()
        {
            CurrentView = _provider.GetRequiredService<MarkupEditorViewModel>();
        }

        [RelayCommand]
        private void NavigateProfile()
        {
            CurrentView = _provider.GetRequiredService<ProfileViewModel>();
        }

        [RelayCommand]
        private void NavigateProjects()
        {
            CurrentView = _provider.GetRequiredService<ProjectsViewModel>();
        }

        [RelayCommand]
        private void NavigateCompanies()
        {
            CurrentView = _provider.GetRequiredService<CompaniesViewModel>();
        }

        [RelayCommand]
        private void NavigateTeams()
        {
            CurrentView = _provider.GetRequiredService<TeamMembersViewModel>();
        }

        [RelayCommand]
        private void NavigateIssues()
        {
            CurrentView = _provider.GetRequiredService<IssuesViewModel>();
        }

        [RelayCommand]
        private void NavigateChat()
        {
            CurrentView = _provider.GetRequiredService<ChatViewModel>();
        }

        [RelayCommand]
        private void NavigateViewer()
        {
            CurrentView = _provider.GetRequiredService<ModelViewerViewModel>();
        }
        [RelayCommand]
        private void NavigateAIAssistant()
        {
            CurrentView = _provider.GetRequiredService<AIAssistantViewModel>();
        }
        [RelayCommand]
        private void NavigateEditor()
        {
            CurrentView = _provider.GetRequiredService<EditorViewModel>();
        }
        #endregion

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authApiService.LogoutAsync();

            LoginWindow loginWindow = _provider.GetRequiredService<LoginWindow>();
            LoginViewModel loginViewModel = _provider.GetRequiredService<LoginViewModel>();
            loginViewModel.CloseAction = loginWindow.Close;
            loginWindow.DataContext = loginViewModel;
            loginWindow.Show();

            Application.Current.Windows.OfType<MainWindow>()
                .FirstOrDefault(w => Equals(w.DataContext, this))?.Close();
        }

    }
}
