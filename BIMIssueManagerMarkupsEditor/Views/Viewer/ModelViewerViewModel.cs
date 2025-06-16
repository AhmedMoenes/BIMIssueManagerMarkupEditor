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
    }
}