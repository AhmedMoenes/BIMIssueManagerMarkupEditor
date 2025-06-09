namespace BIMIssueManagerMarkupsEditor.Interfaces
{
    public interface IDialogService
    {
        Task ShowDialogAsync<TView, TViewModel>(TViewModel viewModel)
            where TView : Window, new()
            where TViewModel : class;
    }
}
