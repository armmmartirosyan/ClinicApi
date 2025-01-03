using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Services;
using Clinic.Infrastructure.Repositories;
using Clinic.Infrastructure.Validators;
using FluentValidation;

namespace Clinic.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<IAuthService, AuthService>();

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services.AddScoped<IAuthRepository, AuthRepository>();

    public static IServiceCollection AddValidators(this IServiceCollection services)
        => services.AddScoped<AbstractValidator<User>, UserValidator>();
}