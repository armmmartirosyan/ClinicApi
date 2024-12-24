using Clinic.Core.Interfaces;
using Clinic.Core.Services;

namespace Clinic.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services.AddScoped<IAuthService, AuthService>();
    }
}
