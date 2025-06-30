using DTOs.IssueLabel;
using DTOs.Snapshots;
using HandyControl.Controls;
using HandyControl.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    public partial class AddIssueViewModel : ObservableObject, IDialogAware
    {
        public event Action? RequestClose;
        private readonly UserSessionService _userSession;
        private readonly IssueApiService _issueApiService;
        private readonly ProjectApiService _projectApiService;
        private readonly IWindowService _windowService;
        public Action? RequestWindowHide { get; set; }
        public Action? RequestWindowShow { get; set; }
        public AddIssueViewModel(UserSessionService userSession, 
                                 IssueApiService issueApiService, 
                                 ProjectApiService projectApiService,
                                 IWindowService windowService)
        {
            _userSession = userSession;
            _issueApiService = issueApiService;
            _projectApiService = projectApiService;
            _windowService = windowService;
            Screenshot.Snapped += Screenshot_Snapped;

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
        private async Task SaveIssueAsync()
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
                HandyControl.Controls.MessageBox.Show("Issue Created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetForm();
            }
            else
            {
                HandyControl.Controls.MessageBox.Show("Failed to create issue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private System.Windows.Window? _activeWindow;
        [RelayCommand]
        private async Task TakeSnapShotAsync()
        {
            RequestWindowHide?.Invoke();
            await Task.Delay(200); // optional to ensure it's hidden first
            new Screenshot().Start();
        }

        private void Screenshot_Snapped(object? sender, FunctionEventArgs<ImageSource> e)
        {
            _ = HandleScreenshotAsync(e.Info);
        }
        private async Task HandleScreenshotAsync(ImageSource imageSource)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), $"Snapshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            try
            {
                // Save image to disk
                var encoder = new PngBitmapEncoder();
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
                
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }
                });

                // Upload to API
                using var multipart = new MultipartFormDataContent();
                multipart.Add(new StreamContent(File.OpenRead(tempPath)), "file", Path.GetFileName(tempPath));

                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44374/") };
                var response = await client.PostAsync("/api/Snapshot/upload", multipart);

                if (!response.IsSuccessStatusCode)
                {
                    ShowMessage("Upload failed!", "Error");
                    return;
                }

                var rawPath = await response.Content.ReadAsStringAsync();
                if (!rawPath.StartsWith("http"))
                {
                    rawPath = "https://localhost:44374/" + rawPath.TrimStart('/');
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    SnapshotImagePath = rawPath;
                    ShowMessage("Snapshot Added!", "Success");
                    RequestWindowHide?.Invoke();

                });
            }
            catch (Exception ex)
            {
                ShowMessage($"Error uploading image: {ex.Message}", "Error");
            }
            finally
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    RequestWindowShow?.Invoke();
                });
            }
        }
        private void ShowMessage(string msg, string caption)
        {
            HandyControl.Controls.MessageBox.Show(msg, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Dispose()
        {
            Screenshot.Snapped -= Screenshot_Snapped;
        }
        
        public static readonly List<Priority> All = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();

    }
}
