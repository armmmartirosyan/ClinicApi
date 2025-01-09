using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.DTO;
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
            .AddScoped<IWeekDayScheduleService, WeekDayScheduleService>()
            .AddScoped<INotWorkingDaysService, NotWorkingDaysService>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthRepository, AuthRepository>()
            .AddScoped<IVisitRepository, VisitRepository>()
            .AddScoped<IWeekDayScheduleRepository, WeekDayScheduleRepository>()
            .AddScoped<INotWorkingDaysRepository, NotWorkingDaysRepository>();
    } 

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<AbstractValidator<User>, UserValidator>()
            .AddScoped<AbstractValidator<AddVisitRequest>, AddVisitValidator>()
            .AddScoped<AbstractValidator<UpdateVisitRequest>, UpdateVisitValidator>()
            .AddScoped<AbstractValidator<CreateWeekDayScheduleRequest>, CreateWeekDayScheduleValidator>()
            .AddScoped<AbstractValidator<UpdateWeekDayScheduleRequest>, UpdateWeekDayScheduleValidator>()
            .AddScoped<AbstractValidator<CreateNotWorkingDayRequest>, CreateNotWorkingDayValidator>()
            .AddScoped<AbstractValidator<UpdateNotWorkingDateValidateDTO>, UpdateNotWorkingDayValidator>();
    }
}