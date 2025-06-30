namespace BIMIssueManagerMarkupsEditor.Services
{
    public class WindowService : IWindowService
    {
        public Window? GetActiveWindow()
        {
            return Application.Current?.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive);
        }

        public Window? GetWindowByDataContext(object viewModel)
        {
            return Application.Current?.Windows
                .OfType<Window>()
                .FirstOrDefault(w => ReferenceEquals(w.DataContext, viewModel));
        }
    }
}
