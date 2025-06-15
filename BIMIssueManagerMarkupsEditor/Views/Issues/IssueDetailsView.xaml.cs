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
using System.Windows.Shapes;

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
