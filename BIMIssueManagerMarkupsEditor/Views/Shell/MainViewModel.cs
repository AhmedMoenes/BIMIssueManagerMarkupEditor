namespace BIMIssueManagerMarkupsEditor.Views.Shell
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        public MainViewModel(IServiceProvider provider)
        {
            _provider = provider;
            NavigateMarkup();
        }

        [ObservableProperty] 
        private ObservableObject currentView;

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
            CurrentView = _provider.GetRequiredService<AddProjectViewModel>();
        }

        [RelayCommand]
        private void NavigateCompanies()
        {
            CurrentView = _provider.GetRequiredService<AddCompanyViewModel>();
        }
        
        [RelayCommand]
        private void NavigateTeams()
        {
            CurrentView = _provider.GetRequiredService<AddTeamMemberViewModel>();
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

    }
}
