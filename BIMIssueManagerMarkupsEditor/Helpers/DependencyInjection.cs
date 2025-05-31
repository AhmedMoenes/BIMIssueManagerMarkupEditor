namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddTransient<IApiService,ApiService>();
            services.TryAddTransient<MainWindow>();
            services.TryAddTransient<MainViewModel>();
            services.TryAddTransient<ProfileViewModel>();
            services.TryAddTransient<AddProjectViewModel>();
            services.TryAddTransient<AddTeamMemberViewModel>();
            services.TryAddTransient<IssuesViewModel>();
            services.TryAddTransient<ModelViewerViewModel>();
            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));

            services.AddHttpClient<IApiService,ApiService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            return services;
        }


    }
}
