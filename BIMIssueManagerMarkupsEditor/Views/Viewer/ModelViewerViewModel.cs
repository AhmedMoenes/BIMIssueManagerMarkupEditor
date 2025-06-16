using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    public partial class ModelViewerViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? selectedElementJson;

        [ObservableProperty]
        private string? selectedElementId;

        
    }
}