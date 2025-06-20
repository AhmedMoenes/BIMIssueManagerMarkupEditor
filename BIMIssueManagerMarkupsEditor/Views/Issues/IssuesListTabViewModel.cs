using System.ComponentModel;

namespace BIMIssueManagerMarkupsEditor.ViewModels.Issues
{
    public partial class IssuesListTabViewModel : ObservableObject
    {
        private readonly IssuesViewModel _parentViewModel;
     

        public IssuesListTabViewModel(IssuesViewModel parentViewModel) 
        {
            _parentViewModel = parentViewModel;
            _parentViewModel.PropertyChanged += ParentViewModel_PropertyChanged;
            ApplyFilterCommand = _parentViewModel.ApplyFilterCommand;
            ResetFilterCommand = _parentViewModel.ResetFilterCommand;
        }

        public string Title => "All Issues";
        public ObservableCollection<IssueDto> Issues => _parentViewModel.Issues;
        public ObservableCollection<string> Projects => _parentViewModel.Projects;
        public ObservableCollection<string> Priorities => _parentViewModel.Priorities;

        // Expose the parent's commands
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand OpenIssueDetailsViewCommand => _parentViewModel.OpenIssueDetailsViewCommand;

        private void ParentViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IssuesViewModel.Issues))
            {
                OnPropertyChanged(nameof(Issues));
            }
        }
    }
}