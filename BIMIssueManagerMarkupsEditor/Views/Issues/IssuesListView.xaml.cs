using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    /// <summary>
    /// Interaction logic for IssuesListView.xaml
    /// </summary>
    public partial class IssuesListView : UserControl
    {
        public IssuesListView(IssuesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
