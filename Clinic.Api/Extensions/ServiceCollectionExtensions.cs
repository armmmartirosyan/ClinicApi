using Clinic.Core.Interfaces;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Services;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<IAuthService, AuthService>();

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services.AddScoped<IAuthRepository, AuthRepository>();
}