using BIMIssueManagerMarkupsEditor.Views.Editor;

namespace BIMIssueManagerMarkupsEditor.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IDialogService, DialogService>();
            services.TryAddSingleton<IApiService,ApiService>();
            services.TryAddSingleton<AuthApiService>();
            services.TryAddSingleton<UserSessionService>();
            services.TryAddScoped<IssueApiService>();
            services.TryAddScoped<ProjectTeamMemberApiService>();
            services.TryAddSingleton<IWindowService, WindowService>();
            services.TryAddScoped<ProjectApiService>();
            services.TryAddScoped<UserApiService>();
            services.TryAddScoped<CommentApiService>();
            services.TryAddScoped<CompanyApiService>();
            return services;
        }
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.TryAddTransient<LoginWindow>();
            services.TryAddTransient<MainWindow>();
            services.TryAddTransient<CommentView>();
            services.TryAddTransient<AddProjectView>();
            services.TryAddTransient<AddTeamMemberView>();
            services.TryAddTransient<AssignUserToProjectView>();
            services.TryAddTransient<AssignCompaniesToProjectView>();
            services.TryAddTransient<AddCompanyView>();
            services.TryAddTransient<IssueDetailsView>();
            services.TryAddSingleton<MainViewModel>();
            services.TryAddTransient<MarkupEditorViewModel>();
            services.TryAddTransient<ProfileViewModel>();
            services.TryAddTransient<ProjectsViewModel>();
            services.TryAddTransient<CompaniesViewModel>();
            services.TryAddTransient<AddProjectViewModel>();
            services.TryAddTransient<AssignCompaniesToProjectViewModel>();
            services.TryAddTransient<AddCompanyViewModel>();
            services.TryAddTransient<AddTeamMemberViewModel>();
            services.TryAddTransient<AssignUserToProjectViewModel>();
            services.TryAddTransient<TeamMembersViewModel>();
            services.TryAddTransient<IssuesViewModel>();
            services.TryAddTransient<IssueDetailsViewModel>();
            services.TryAddTransient<ChatViewModel>();
            services.TryAddTransient<ModelViewerViewModel>();
            services.TryAddTransient<AddIssueViewModel>();
            services.TryAddTransient<AddIssueView>();
            services.TryAddTransient<ModelViewerView>();
            services.TryAddTransient<LoginViewModel>();
            services.TryAddTransient<CommentViewModel>();
            services.TryAddTransient<EditIssueViewModel>();
            services.TryAddTransient<EditIssueView>();
            services.TryAddTransient<AIAssistantView>();
            services.TryAddTransient<AIAssistantViewModel>();
            services.TryAddTransient<EditorViewModel>();
            services.TryAddTransient<EditorView>();

            services.AddTransient<Func<int, EditIssueViewModel>>(provider => issueId =>
                new EditIssueViewModel(
                    provider.GetRequiredService<IssueApiService>(),
                    provider.GetRequiredService<ProjectApiService>(),
                    issueId));

            services.TryAddTransient<Func<int, CommentViewModel>>(sp => issueId =>
                new CommentViewModel(
                    sp.GetRequiredService<CommentApiService>(),
                    sp.GetRequiredService<UserSessionService>(),
                    issueId
                ));

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
