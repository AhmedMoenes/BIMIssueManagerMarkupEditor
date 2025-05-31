using BIMIssueManagerMarkupsEditor.Views.Shell;

namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddTransient<IApiService,ApiService>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<AddProjectViewModel>();
            services.AddTransient<AddTeamMemberViewModel>();
            services.AddTransient<IssuesViewModel>();
            services.AddTransient<ModelViewerViewModel>();
            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));

            services.AddHttpClient<ApiService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            return services;
        }


    }
}
