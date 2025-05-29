namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class Startup
    {
        public static IServiceProvider Initialize(this IServiceProvider serviceProvider)
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
