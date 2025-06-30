using System.Text.Json;
using HandyControl.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    public partial class ModelViewerViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;
        private IDialogService _dialogService;
        [ObservableProperty] private string? selectedElementJson;

        [ObservableProperty] private string? selectedElementId;


        public ModelViewerViewModel(IServiceProvider serviceProvider,
            IDialogService dialogService)
        {
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
        }

        public ModelViewerViewModel()
        {
            
        }

        public void HandleWebMessage(string json)
        {
            try
            {
                var element = JsonSerializer.Deserialize<JsonElement>(json);

                SelectedElementJson = JsonSerializer.Serialize(element, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                if (element.TryGetProperty("tag", out var tagProp))
                    SelectedElementId = tagProp.GetString();
                else if (element.TryGetProperty("elementId", out var idProp))
                    SelectedElementId = idProp.ToString();
                else
                    SelectedElementId = "❌ No ID found";

                Console.WriteLine("✅ Element ID: " + SelectedElementId);
            }
            catch (Exception ex)
            {
                SelectedElementJson = "❌ Error parsing: " + ex.Message;
                SelectedElementId = "❌ Error";
            }
        }

        [RelayCommand]
        public void OpenAddIssueWindow()
        {
            var viewModel = _serviceProvider.GetRequiredService<AddIssueViewModel>();
            _dialogService.ShowDialogAsync<AddIssueView, AddIssueViewModel>(viewModel);
        }
    }
}