using SimpleNetCoreSecurityWeb.Services.Impl;

namespace SimpleNetCoreSecurityWeb.Services.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
