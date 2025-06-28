using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.AIAssistant
{
    /// <summary>
    /// Interaction logic for AIAssistant.xaml
    /// </summary>
    public partial class AIAssistantView : UserControl
    {
        private readonly AIAssistantViewModel _viewModel = new();
        public AIAssistantView()
        {
            InitializeComponent();
            DataContext = _viewModel;
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            await AssistantWebView.EnsureCoreWebView2Async();


            //AssistantWebView.NavigationCompleted += (s, e) =>
            //{
            //    if (e.IsSuccess)
            //    {
            //        MessageBox.Show("✅ WebView2 navigated successfully");
            //    }
            //    else
            //    {
            //        MessageBox.Show($"❌ Navigation failed: {e.WebErrorStatus}");
            //    }
            //};

            AssistantWebView.Source = new Uri("http://localhost:5500");
        }
    }
}
