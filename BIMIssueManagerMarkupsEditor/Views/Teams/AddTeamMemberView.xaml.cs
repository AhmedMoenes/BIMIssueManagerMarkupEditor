using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Teams
{
    /// <summary>
    /// Interaction logic for AddTeamMemberView.xaml
    /// </summary>
    public partial class AddTeamMemberView : UserControl
    {
        public AddTeamMemberView()
        {
            InitializeComponent();
        }

        public AddTeamMemberView(AddTeamMemberViewModel viewModel) : this()
        {
            DataContext= viewModel;
        }
    }
}
