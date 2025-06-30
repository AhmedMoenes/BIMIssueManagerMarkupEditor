using BIMIssueManagerMarkupsEditor.ApiRoutes;
using DTOs.IssueLabel;
using DTOs.Snapshots;

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    public partial class AddIssueViewModel : ObservableObject, IDialogAware
    {
        public event Action? RequestClose;
        private readonly UserSessionService _userSession;
        private readonly IssueApiService _issueApiService;
        private readonly ProjectApiService _projectApiService;

        public AddIssueViewModel(UserSessionService userSession, 
                                 IssueApiService issueApiService, 
                                 ProjectApiService projectApiService)
        {
            _userSession = userSession;
            _issueApiService = issueApiService;
            _projectApiService = projectApiService;

            Projects = new();
            Areas = new();
            Users = new();
            Labels = new();

            LoadUserProjectsAsync();
        }

        [ObservableProperty] private string title;
        [ObservableProperty] private string description;
        [ObservableProperty] private int areaId;
        [ObservableProperty] private int projectId;
        [ObservableProperty] private string createdByUserId;
        [ObservableProperty] private string assignedToUserId;
        [ObservableProperty] private Priority priorityChoice;
        [ObservableProperty] private string? snapshotImagePath;
        [ObservableProperty] private LabelDto? selectedLabel;
        [ObservableProperty] private ProjectDto? selectedProject;
        [ObservableProperty] private ObservableCollection<ProjectDto> projects;
        [ObservableProperty] private ObservableCollection<AreaDto> areas;
        [ObservableProperty] private ObservableCollection<UserDto> users;
        [ObservableProperty] private ObservableCollection<LabelDto> labels;
        [ObservableProperty] private ObservableCollection<Priority> priorities = new();


        private async void LoadUserProjectsAsync()
        {
            if (string.IsNullOrWhiteSpace(_userSession.UserId)) return;

            var projects = await _issueApiService.GetProjectsByUserIdAsync(_userSession.UserId);
            Projects.Clear();
            foreach (var p in projects) Projects.Add(p);
        }

        partial void OnSelectedProjectChanged(ProjectDto? value)
        {
            if (value is null) return;
            projectId = value.ProjectId;
            LoadLookupsByProjectIdAsync(projectId);
        }

        private async void LoadLookupsByProjectIdAsync(int projectId)
        {
            var areas = await _issueApiService.GetAreasByProjectIdAsync(projectId);
            var users = await _issueApiService.GetUsersByProjectIdAsync(projectId);
            var labels = await _issueApiService.GetLabelsByProjectIdAsync(projectId);

            Areas.Clear();
            foreach (var a in areas) Areas.Add(a);

            Users.Clear();
            foreach (var u in users) Users.Add(u);

            Labels.Clear();
            foreach (var l in labels) Labels.Add(l);
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            var dto = new CreateIssueDto
            {
                Title = Title,
                Description = Description,
                AreaId = AreaId,
                ProjectId = SelectedProject?.ProjectId ?? 0,
                CreatedByUserId = _userSession.UserId,
                AssignedToUserId = AssignedToUserId,
                CreatedAt = DateTime.UtcNow,
                Priority = PriorityChoice,
                Labels = SelectedLabel is not null
                    ? new List<AssignLabelToIssueDto> { new() { LabelId = SelectedLabel.LabelId } }
                    : new List<AssignLabelToIssueDto>(),
                Snapshot = !string.IsNullOrWhiteSpace(SnapshotImagePath)
                    ? new SnapshotDto { Path = SnapshotImagePath, CreatedAt = DateTime.UtcNow }
                    : null
            };

            var created = await _issueApiService.CreateAsync(dto);
            if (created is not null)
            {
                MessageBox.Show("Issue Created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Failed to create issue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ResetForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            AreaId = 0;
            AssignedToUserId = string.Empty;
            PriorityChoice = default;
            SnapshotImagePath = null;
            SelectedLabel = null;
            SelectedProject = null;
        }

        [RelayCommand]
        private void SaveIssue()
        {

        }

        [RelayCommand]
        private void TakeSnapShot()
        {

        }

        public static readonly List<Priority> All = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();

    }
}
