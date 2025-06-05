using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    /// <summary>
    /// Interaction logic for IssuesListView.xaml
    /// </summary>
    public partial class IssuesListView : UserControl
    {
        public IssuesListView()
        {
            InitializeComponent();
        }
        public IssuesListView(IssuesViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
