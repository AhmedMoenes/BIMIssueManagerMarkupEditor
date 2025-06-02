using System.Collections.ObjectModel;
using DTOs.Issues;

namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    public class IssuesViewModel : ObservableObject
    {
        public ObservableCollection<IssueDto> Issues { get; set; } = new();

        public IssuesViewModel()
        {
            // Dummy sample data
            Issues.Add(new IssueDto
            {
                Title = "Issue 1",
                SnapshotImagePath = "https://via.placeholder.com/200x120.png?text=Snapshot+1",
                Priority = "High",
                AssignedToUser = "Ahmed M.",
                CreatedAt = DateTime.Now.AddDays(-1)
            });

            Issues.Add(new IssueDto
            {
                Title = "Issue 2",
                SnapshotImagePath = "https://via.placeholder.com/200x120.png?text=Snapshot+2",
                Priority = "Medium",
                AssignedToUser = "Mona S.",
                CreatedAt = DateTime.Now.AddDays(-2)
            });

            Issues.Add(new IssueDto
            {
                Title = "Issue 3",
                SnapshotImagePath = "https://via.placeholder.com/200x120.png?text=Snapshot+3",
                Priority = "Low",
                AssignedToUser = "Kareem A.",
                CreatedAt = DateTime.Now.AddDays(-3)
            });
        }
    }
}
