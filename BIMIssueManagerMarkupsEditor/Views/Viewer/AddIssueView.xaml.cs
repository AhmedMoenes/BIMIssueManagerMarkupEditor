﻿using System;
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

namespace BIMIssueManagerMarkupsEditor.Views.Viewer
{
    /// <summary>
    /// Interaction logic for AddIssueView.xaml
    /// </summary>
    public partial class AddIssueView : Window
    {
        public AddIssueView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is AddIssueViewModel vm)
            {
                vm.RequestWindowHide = () =>
                {
                    this.Hide();
                };
                vm.RequestWindowShow = () =>
                {
                    this.Show();
                };
            }
        }
    }
}
