using Microsoft.Web.WebView2.Core;
using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    public partial class ModelViewerView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        public ModelViewerView(ModelViewerViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        public ModelViewerView()
        {
            InitializeComponent();
            InitializeViewer();
        }

        private async void InitializeViewer()
        {
            await WebView.EnsureCoreWebView2Async();

            if (WebView.CoreWebView2 == null)
            {
                Console.WriteLine("❌ CoreWebView2 not initialized!");
                return;
            }

            string distPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Viewer", "dist");
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "app", distPath, CoreWebView2HostResourceAccessKind.Allow);

            WebView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;

            WebView.Source = new Uri("https://app/index.html");
        }

        //private async void InitViewer()
        //{
        //    await WebView.EnsureCoreWebView2Async();

            //    if (WebView.CoreWebView2 == null)
            //    {
            //        Console.WriteLine("❌ CoreWebView2 not initialized!");
            //        return;
            //    }

            //    Console.WriteLine("✅ CoreWebView2 ready");

            //   // WebView.CoreWebView2.OpenDevToolsWindow();
            //    WebView.WebMessageReceived += WebView_WebMessageReceived;

            //    Console.WriteLine("✅ WebMessageReceived event attached");

            //    WebView.Source = new Uri("http://localhost:5174/");
            //}
       

        private void WebView_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string? json = e.TryGetWebMessageAsString();
            Console.WriteLine("📥 Web message received:\n" + json);

            if (!string.IsNullOrWhiteSpace(json) && DataContext is ModelViewerViewModel vm)
            {
                vm.HandleWebMessage(json);
            }
        }

    }
}