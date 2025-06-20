using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    /// <summary>
    /// Interaction logic for IssueDetailsTabView.xaml
    /// </summary>
    public partial class IssueDetailsTabView : UserControl
    {
        public IssueDetailsTabView()
        {
            InitializeComponent();
        }
        public IssueDetailsTabView(IssueDetailsViewModel vm) : this()
        {
            DataContext = vm;
        }
    }
}
