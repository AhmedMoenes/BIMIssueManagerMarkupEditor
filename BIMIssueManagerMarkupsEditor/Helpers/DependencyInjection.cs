
using BIMIssueManagerMarkupsEditor.Interfaces;

namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddSingleton<HttpClient>();
            services.TryAddSingleton<IDialogService, DialogService>();
            services.TryAddSingleton<IApiService,ApiService>();
            services.TryAddSingleton<AuthApiService>();
            services.TryAddSingleton<UserSessionService>();
            services.TryAddScoped<IssueApiService>();
            services.TryAddScoped<ProjectTeamMemberApiService>();
            services.TryAddScoped<ProjectApiService>();
            services.TryAddScoped<UserApiService>();
            services.TryAddScoped<CompanyApiService>();
            return services;
        }
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.TryAddSingleton<LoginWindow>();
            services.TryAddTransient<MainWindow>();
            services.TryAddTransient<AddCommentView>();
            services.TryAddTransient<MainViewModel>();
            services.TryAddTransient<MarkupEditorViewModel>();
            services.TryAddTransient<ProfileViewModel>();
            services.TryAddTransient<AddProjectViewModel>();
            services.TryAddTransient<AddCompanyViewModel>();
            services.TryAddTransient<AddTeamMemberViewModel>();
            services.TryAddTransient<IssuesViewModel>();
            services.TryAddTransient<ChatViewModel>();
            services.TryAddTransient<ModelViewerViewModel>();
            services.TryAddTransient<LoginViewModel>();
            services.TryAddTransient<Func<int, AddCommentViewModel>>(sp => issueId =>
                new AddCommentViewModel(sp.GetRequiredService<CommentApiService>(), issueId));
            return services;
        }
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));

            services.AddHttpClient<IApiService,ApiService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
            });
            return services;
        }


    }
}
