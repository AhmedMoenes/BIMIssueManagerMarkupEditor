﻿using System.Windows.Controls;

namespace BIMIssueManagerMarkupsEditor.Views.User
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        public ProfileView(ProfileViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
