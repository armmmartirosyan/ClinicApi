using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Services;
using Clinic.Infrastructure.Repositories;
using Clinic.Infrastructure.Validators;
using FluentValidation;
 
namespace Clinic.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IVisitService, VisitService>()
            .AddScoped<IWeekDayScheduleService, WeekDayScheduleService>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthRepository, AuthRepository>()
            .AddScoped<IVisitRepository, VisitRepository>()
            .AddScoped<IWeekDayScheduleRepository, WeekDayScheduleRepository>();
    } 

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<AbstractValidator<User>, UserValidator>()
            .AddScoped<AbstractValidator<AddVisitRequest>, AddVisitValidator>()
            .AddScoped<AbstractValidator<UpdateVisitRequest>, UpdateVisitValidator>()
            .AddScoped<AbstractValidator<CreateWeekDayScheduleRequest>, CreateWeekDayScheduleValidator>()
            .AddScoped<AbstractValidator<UpdateWeekDayScheduleRequest>, UpdateWeekDayScheduleValidator>();
    }
}