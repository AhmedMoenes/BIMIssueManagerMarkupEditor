namespace BIMIssueManagerMarkupsEditor
{

    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _serviceProvider = StartApp.Initialize();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = new MainViewModel(_serviceProvider);
            mainWindow.Show();
        }

    }

}
