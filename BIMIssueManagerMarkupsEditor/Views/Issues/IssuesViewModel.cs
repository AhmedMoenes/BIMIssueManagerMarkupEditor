using System.Collections.ObjectModel;
using System.Windows.Input;
using DTOs.Issues;

namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    public partial class IssuesViewModel : ObservableObject
    {
        private readonly IssueApiService _issueApiService;
        public IssuesViewModel(IssueApiService issueApiService)
        {
            _issueApiService = issueApiService;

            LoadIssuesAsync();
            LoadPriorities();
            LoadRevitVersions();
            LoadProjectsAsync();
            LoadUsersAsync();

            ApplyFilterCommand = new RelayCommand(async () => await LoadIssuesAsync());
        }

        [ObservableProperty]
        private ObservableCollection<IssueDto> issues = new();

        [ObservableProperty]
        private ObservableCollection<string> projects = new(); 
        [ObservableProperty]
        private string selectedProject;

        [ObservableProperty]
        private ObservableCollection<string> assignedToUser = new();
        [ObservableProperty]
        private string selectedAssignee;

        [ObservableProperty]
        private ObservableCollection<string> priorities = new();
        [ObservableProperty]
        private string selectedPriority;

        [ObservableProperty]
        private DateTime? selectedDate;

        [ObservableProperty]
        private ObservableCollection<string> revitVersionOptions = new();
        [ObservableProperty]
        private string selectedRevitVersion;
        public ICommand ApplyFilterCommand { get; }
        private void LoadPriorities()
        {
            Priorities = new ObservableCollection<string>(Enum.GetNames(typeof(Priority)));
        }
        private void LoadRevitVersions()
        {
            RevitVersionOptions = new ObservableCollection<string>
            {
                "2020", "2021", "2022", "2023", "2024","2025","2026"
            };
        }
        private async void LoadProjectsAsync()
        {
            // TODO: Replace with actual project service
            Projects = new ObservableCollection<string>
            {
                "Project Alpha", "Project Beta", "Project Gamma"
            };
        }

        private async void LoadUsersAsync()
        {
            // TODO: Replace with actual user service
            AssignedToUser = new ObservableCollection<string>
            {
                "user1@bim.com", "user2@bim.com", "user3@bim.com"
            };
        }
        private async Task LoadIssuesAsync()
        {
            var allIssues = await _issueApiService.GetAllAsync();
            Issues = new ObservableCollection<IssueDto>(allIssues);
        }

        private async Task FilterIssuesAsync()
        {
            var allIssues = await _issueApiService.GetAllAsync();
            var filtered = allIssues;

            if (!string.IsNullOrEmpty(SelectedProject))
                filtered = filtered.Where(i => i.ProjectName == SelectedProject);

            if (!string.IsNullOrEmpty(SelectedAssignee))
                filtered = filtered.Where(i => i.AssignedToUser == SelectedAssignee);

            if (!string.IsNullOrEmpty(SelectedPriority))
                filtered = filtered.Where(i => i.Priority == SelectedPriority);

            if (SelectedDate != null)
                filtered = filtered.Where(i => i.CreatedAt.Date == SelectedDate.Value.Date);

            Issues = new ObservableCollection<IssueDto>(filtered);
        }


    }
}
