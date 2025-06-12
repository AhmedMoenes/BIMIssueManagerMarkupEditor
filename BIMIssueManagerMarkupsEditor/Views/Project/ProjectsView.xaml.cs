using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.Project
{
    /// <summary>
    /// Interaction logic for ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : UserControl
    {
        public ProjectsView()
        {
            InitializeComponent();
        }

        public ProjectsView(ProjectsViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}

