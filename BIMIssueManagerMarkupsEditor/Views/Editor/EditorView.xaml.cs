using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Editor
{
    /// <summary>
    /// Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl
    {
        private readonly EditorViewModel _viewModel = new();
        public EditorView()
        {
            InitializeComponent();
            DataContext = _viewModel;
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            await EditorWebView.EnsureCoreWebView2Async();

            // Get the absolute path to the local HTML file
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string localPath = System.IO.Path.Combine(exeDir, "Resources", "Editor", "index.html");

            // Convert to file:// URI format and load it
            var uri = new Uri($"file:///{localPath.Replace("\\", "/")}");
            EditorWebView.Source = uri;
        }


    }
}
