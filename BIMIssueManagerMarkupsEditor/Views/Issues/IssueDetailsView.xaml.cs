namespace BIMIssueManagerMarkupsEditor.Views.Issues
{
    /// <summary>
    /// Interaction logic for IssueDetailsView.xaml
    /// </summary>
    public partial class IssueDetailsView : HandyControl.Controls.Window
    {
        public IssueDetailsView()
        {
            InitializeComponent();
        }

        public IssueDetailsView(IssueDetailsViewModel vm) : this()
        {
            DataContext = vm;
        }
    }
}
