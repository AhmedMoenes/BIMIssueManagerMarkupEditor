namespace BIMIssueManagerMarkupsEditor.Interfaces
{
    public interface IWindowService
    {
        Window? GetActiveWindow();
        Window? GetWindowByDataContext(object viewModel);
    }
}
