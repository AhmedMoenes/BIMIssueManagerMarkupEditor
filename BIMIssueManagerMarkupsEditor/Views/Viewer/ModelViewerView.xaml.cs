using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    public partial class ModelViewerView : UserControl
    {
        private readonly ModelViewerViewModel _viewModel = new();

        public ModelViewerView()
        {
            InitializeComponent();
            DataContext = _viewModel;
            InitViewer();
        }

        private async void InitViewer()
        {
            await WebView.EnsureCoreWebView2Async();

            if (WebView.CoreWebView2 == null)
            {
                Console.WriteLine("❌ CoreWebView2 not initialized!");
                return;
            }

            Console.WriteLine("✅ CoreWebView2 ready");

           // WebView.CoreWebView2.OpenDevToolsWindow();
            WebView.WebMessageReceived += WebView_WebMessageReceived;

            Console.WriteLine("✅ WebMessageReceived event attached");

            WebView.Source = new Uri("http://localhost:5174/");
        }

        private void WebView_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string? json = e.TryGetWebMessageAsString();
            Console.WriteLine("📥 Web message received:\n" + json);

            if (!string.IsNullOrWhiteSpace(json))
            {
                _viewModel.HandleWebMessage(json);
            }
        }


    }
}