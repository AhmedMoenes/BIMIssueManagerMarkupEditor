using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
