using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class AddVisitValidator : AbstractValidator<AddVisitRequest>
{
    private readonly IVisitRepository _visitRepository;
    private readonly ClinicDbContext _dbContext;
    public AddVisitValidator(IVisitRepository visitRepository, ClinicDbContext dbContext)
    {
        _visitRepository = visitRepository;
        _dbContext = dbContext;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.DoctorId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(BeAValidDoctorId).WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        //2025-01-06T15:30:50
        RuleFor(v => v.StartScheduledDate)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v.EndScheduledDate)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v)
            .Must(v => v.StartScheduledDate <= v.EndScheduledDate)
            .WithMessage("End schedule date can't be earlier than start schedule date.");

        RuleFor(x => x)
            .Must(BeWithinWorkingHours)
            .WithMessage("Scheduled time must be within the doctor's working hours.")
            .Must(NotBeDuringBreakTime)
            .WithMessage("Scheduled time must not overlap with the doctor's break time.")
            .Must(NotOverlapWithExistingRegistrations)
            .WithMessage("Scheduled time conflicts with an existing registration.");
    }

    private async Task<bool> BeAValidUserId(long userId, CancellationToken cancellationToken)
    {
        return await _visitRepository.IsValidUserIdAsync(userId);
    }

    private async Task<bool> BeAValidDoctorId(long doctorId, CancellationToken cancellationToken)
    {
        return await _visitRepository.IsValidDoctorId(doctorId);
    }

    private bool BeAValidDate(DateTime date)
    {
        return date != default(DateTime);
    }
    
    //
    private bool BeWithinWorkingHours(AddVisitRequest request)
    {
        var doctor = _dbContext.Users
            .Where(d => d.Id == request.DoctorId)
            .Select(d => new { d.WeekDaySchedules, d.NotWorkingDays })
            .FirstOrDefault();

        if (doctor == null) return false;

        var scheduledDate = request.StartScheduledDate.Date;
        if (doctor.NotWorkingDays.Any(n => n.NotWorkDate == DateOnly.FromDateTime(scheduledDate)))
        {
            return false; // Doctor is not working on this day
        }

        var weekday = (int)scheduledDate.DayOfWeek; // Sunday = 0, Monday = 1, ...

        if (weekday == 0)
        {
            weekday = 7;
        }
        
        var schedule = doctor.WeekDaySchedules.FirstOrDefault(s => s.WeekDayId == weekday);
        if (schedule == null) return false; // No working hours defined for this day

        var startTime =  scheduledDate.Date.Add(schedule.StartTime.ToTimeSpan());
        var endTime = scheduledDate.Date.Add(schedule.EndTime.ToTimeSpan());

        return request.StartScheduledDate.TimeOfDay >= startTime.TimeOfDay &&
               request.EndScheduledDate.TimeOfDay <= endTime.TimeOfDay;
    }
    
    private bool NotBeDuringBreakTime(AddVisitRequest request)
    {
        var doctor = _dbContext.Users
            .Where(d => d.Id == request.DoctorId)
            .Select(d => d.WeekDaySchedules)
            .FirstOrDefault();

        if (doctor == null) return false;

        var weekday = (int)request.StartScheduledDate.DayOfWeek;
        
        if (weekday == 0)
        {
            weekday = 7;
        }
        
        var schedule = doctor.FirstOrDefault(s => s.WeekDayId == weekday);
        if (schedule == null) return false;

        var breakStart = request.StartScheduledDate.Date.Add(schedule.BreakStartTime.ToTimeSpan());
        var breakEnd = request.StartScheduledDate.Date.Add(schedule.BreakEndTime.ToTimeSpan());

        return !(request.StartScheduledDate.TimeOfDay < breakEnd.TimeOfDay &&
                 request.EndScheduledDate.TimeOfDay > breakStart.TimeOfDay);
    }
    
    private bool NotOverlapWithExistingRegistrations(AddVisitRequest request)
    {
        return !_dbContext.Visits
            .Any(r => r.DoctorId == request.DoctorId &&
                      r.StartScheduledDate < request.EndScheduledDate &&
                      r.EndScheduledDate > request.StartScheduledDate);
    }
}
