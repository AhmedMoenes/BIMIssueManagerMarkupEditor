namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class StartApp
    {
        public static IServiceProvider Initialize()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",false).
                Build();
            
            var services = new ServiceCollection();
            services.AddConfiguration(configuration);
            services.AddServices();

            return services.BuildServiceProvider();
        }
    }
}
