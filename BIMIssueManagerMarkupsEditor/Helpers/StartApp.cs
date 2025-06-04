namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class StartApp
    {
        public static IServiceProvider Initialize()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",false, reloadOnChange:true).
                Build();
            
            var services = new ServiceCollection();
            services.AddConfiguration(configuration);
            services.AddServices();
            services.AddViewModels();

            return services.BuildServiceProvider();
        }
    }
}
