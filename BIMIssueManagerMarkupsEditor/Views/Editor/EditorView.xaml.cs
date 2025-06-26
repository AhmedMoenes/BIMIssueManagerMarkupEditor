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

            EditorWebView.Source = new Uri("http://localhost:5501");
        }


    }
}
