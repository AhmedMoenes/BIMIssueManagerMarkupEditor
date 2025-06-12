using BIMIssueManagerMarkupsEditor.Interfaces;

namespace BIMIssueManagerMarkupsEditor.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowDialogAsync<TView, TViewModel>(TViewModel viewModel)
            where TView : Window, new()
            where TViewModel : class
        {
            var window = new TView
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow
            };

            var tcs = new TaskCompletionSource<bool>();
            if (viewModel is IDialogAware dialogAware)
            {
                dialogAware.RequestClose += () =>
                {
                    window.Close();
                };
            }

            window.Closed += (_, _) => tcs.SetResult(true);
            window.ShowDialog();

            return tcs.Task;
        }
    }
}
