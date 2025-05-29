namespace BIMIssueManagerMarkupsEditor
{
    
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _serviceProvider.Initialize();

            //ToDo
            //var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            //mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            //mainWindow.Show();
        }
    }

}
