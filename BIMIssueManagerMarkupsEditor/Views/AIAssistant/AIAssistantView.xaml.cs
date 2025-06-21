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

namespace BIMIssueManagerMarkupsEditor.Views.AIAssistant
{
    /// <summary>
    /// Interaction logic for AIAssistant.xaml
    /// </summary>
    public partial class AIAssistant : UserControl
    {
        private readonly AIAssistantViewModel _viewModel = new();
        public AIAssistant()
        {
            InitializeComponent();
            DataContext = _viewModel;
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            await AssistantWebView.EnsureCoreWebView2Async();


            AssistantWebView.NavigationCompleted += (s, e) =>
            {
                if (e.IsSuccess)
                {
                    MessageBox.Show("✅ WebView2 navigated successfully");
                }
                else
                {
                    MessageBox.Show($"❌ Navigation failed: {e.WebErrorStatus}");
                }
            };

            AssistantWebView.Source = new Uri("http://localhost:5500");
        }
    }
}
