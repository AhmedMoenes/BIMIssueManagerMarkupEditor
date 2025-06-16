using BIMIssueManagerMarkupsEditor.ApiRoutes;
using BIMIssueManagerMarkupsEditor.Services;
using DTOs.IssueLabel;

public partial class EditIssueViewModel : ObservableObject, IDialogAware
{
    private readonly ProjectApiService _projectApiService;
    private readonly IssueApiService _issueApiService;
    private readonly LookupApiService _lookupService;
    private readonly int _issueId;

    public EditIssueViewModel(IssueApiService issueApiService, LookupApiService lookupService, ProjectApiService projectApiService, int issueId)
    {
        _projectApiService = projectApiService;
        _issueApiService = issueApiService;
        _lookupService = lookupService;
        _issueId = issueId;

        PriorityOptions = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();
    }

    [ObservableProperty] private UpdateIssueDto issueForm = new();
    [ObservableProperty] private ObservableCollection<AreaDto> areas = new();
    [ObservableProperty] private ObservableCollection<UserDto> users = new();
    [ObservableProperty] private ObservableCollection<LabelDto> labels = new();
    [ObservableProperty] private int? selectedLabelId;

    public List<Priority> PriorityOptions { get; }

    public async Task LoadAsync()
    {
        var issue = await _issueApiService.GetByIdAsync(_issueId);
        if (issue is null) return;

        ProjectDto project = await _projectApiService.GetProjectByIssueIdAsync(issue.IssueId);
        await LoadLookupsByProjectIdAsync(project.ProjectId);

        IssueForm = new UpdateIssueDto
        {
            Title = issue.Title,
            Description = issue.Description,
            AreaId = issue.Area.AreaId,
            Priority = issue.Priority,
            AssignedToUserId = issue.AssignedToUserId,
            IsResolved = issue.IsResolved,
            IsDeleted = issue.IsDeleted,
            Labels = issue.Labels.Select(l => new AssignLabelToIssueDto { LabelId = l.LabelId }).ToList(),
            Snapshot = issue.Snapshot
        };

        SelectedLabelId = issue.Labels.FirstOrDefault()?.LabelId;
    }

    private async Task LoadLookupsByProjectIdAsync(int projectId)
    {
        var areas = await _lookupService.GetAreasByProjectIdAsync(projectId);
        var users = await _lookupService.GetUsersByProjectIdAsync(projectId);
        var labels = await _lookupService.GetLabelsByProjectIdAsync(projectId);

        Areas = new ObservableCollection<AreaDto>(areas);
        Users = new ObservableCollection<UserDto>(users);
        Labels = new ObservableCollection<LabelDto>(labels);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        IssueForm.Labels = SelectedLabelId.HasValue
            ? new List<AssignLabelToIssueDto> { new AssignLabelToIssueDto { LabelId = SelectedLabelId.Value } }
            : new List<AssignLabelToIssueDto>();

        var response = await _issueApiService.UpdateAsync(_issueId, IssueForm);
        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Issue updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            RequestClose?.Invoke();
        }
        else
        {
            MessageBox.Show("Update failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Cancel() => RequestClose?.Invoke();

    public event Action? RequestClose;
}
