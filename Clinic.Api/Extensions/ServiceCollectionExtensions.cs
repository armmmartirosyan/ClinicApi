using Clinic.Core.Interfaces.Helpers;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.DTO;
using Clinic.Core.Models.Request;
using Clinic.Core.Services;
using Clinic.Infrastructure.Helpers;
using Clinic.Infrastructure.Repositories;
using Clinic.Infrastructure.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Clinic.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        return services
            .AddScoped<IFileHelper, FileHelper>()
            .AddScoped<IAuthHelper, AuthHelper>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IVisitService, VisitService>()
            .AddScoped<IWeekDayScheduleService, WeekDayScheduleService>()
            .AddScoped<INotWorkingDaysService, NotWorkingDaysService>()
            .AddScoped<IProceduresService, ProceduresService>()
            .AddScoped<ISpecializationsService, SpecializationsService>()
            .AddScoped<IVisitProcedureService, VisitProcedureService>()
            .AddScoped<IMedicinesAssignedService, MedicinesAssignedService>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthRepository, AuthRepository>()
            .AddScoped<IVisitRepository, VisitRepository>()
            .AddScoped<IWeekDayScheduleRepository, WeekDayScheduleRepository>()
            .AddScoped<INotWorkingDaysRepository, NotWorkingDaysRepository>()
            .AddScoped<IProceduresRepository, ProceduresRepository>()
            .AddScoped<ISpecializationsRepository, SpecializationsRepository>()
            .AddScoped<IVisitProcedureRepository, VisitProcedureRepository>()
            .AddScoped<IMedicinesAssignedRepository, MedicinesAssignedRepository>();
    } 

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<AbstractValidator<RegisterRequest>, UserValidator>()

            .AddScoped<AbstractValidator<IFormFile>, ImageValidator>()

            .AddScoped<AbstractValidator<AddUpdateSpecializationRequest>, AddUpdateSpecializationValidator>()

            .AddScoped<AbstractValidator<AddVisitRequest>, AddVisitValidator>()
            .AddScoped<AbstractValidator<UpdateVisitRequest>, UpdateVisitValidator>()

            .AddScoped<AbstractValidator<CreateWeekDayScheduleRequest>, CreateWeekDayScheduleValidator>()
            .AddScoped<AbstractValidator<UpdateWeekDayScheduleRequest>, UpdateWeekDayScheduleValidator>()

            .AddScoped<AbstractValidator<CreateNotWorkingDayRequestValidator>, CreateNotWorkingDayValidator>()
            .AddScoped<AbstractValidator<UpdateNotWorkingDateValidateDTO>, UpdateNotWorkingDayValidator>()

            .AddScoped<AbstractValidator<AddProcedureRequest>, AddProcedureValidator>()
            .AddScoped<AbstractValidator<UpdateProcedureRequest>, UpdateProcedureValidator>()

            .AddScoped<AbstractValidator<AddVisitProcedureRequest>, AddVisitProcedureValidator>()
            .AddScoped<AbstractValidator<UpdateVisitProcedureRequest>, UpdateVisitProcedureValidator>()
        
            .AddScoped<AbstractValidator<AddMedicinesAssignedRequest>, AddMedicinesAssignedValidator>()
            .AddScoped<AbstractValidator<UpdateMedicinesAssignedValidateDTO>, UpdateMedicinesAssignedValidator>();
    }

    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        return services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName.Replace("+", "-"));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirment = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference =  new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id =  JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            o.AddSecurityRequirement(securityRequirment);
        });
    }
}