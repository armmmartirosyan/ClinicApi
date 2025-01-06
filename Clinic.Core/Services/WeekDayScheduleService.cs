using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;

public class WeekDayScheduleService
    (
        IWeekDayScheduleRepository weekDayScheduleRepository,
        AbstractValidator<CreateWeekDayScheduleRequest> createWeekDayScheduleValidator,
        AbstractValidator<UpdateWeekDayScheduleRequest> updateWeekDayScheduleValidator
    ) : IWeekDayScheduleService
{
    public async Task<long> CreateAsync(CreateWeekDayScheduleRequest request)
    {
        ValidationResult validationResult = createWeekDayScheduleValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var schedule = new WeekDaySchedule
        {
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            BreakStartTime = request.BreakStartTime,
            BreakEndTime = request.BreakEndTime,
            DoctorId = request.DoctorId,
            WeekDayId = request.WeekDayId
        };

        return await weekDayScheduleRepository.CreateAsync(schedule);
    }

    public async Task<WeekDaySchedule> GetByIdAsync(long id)
    {
        return await weekDayScheduleRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<WeekDaySchedule>> GetSchedulesByDoctorAsync(long doctorId)
    {
        return await weekDayScheduleRepository.GetSchedulesByDoctorAsync(doctorId);
    }

    public async Task<bool> UpdateAsync(long id, UpdateWeekDayScheduleRequest request)
    {
        var schedule = await weekDayScheduleRepository.GetByIdAsync(id);

        if (schedule == null)
        {
            throw new InvalidDataException("Schedule not found.");
        }

        ValidationResult validationRes = updateWeekDayScheduleValidator.Validate(request);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        if (request.StartTime.HasValue) schedule.StartTime = request.StartTime.Value;
        if (request.EndTime.HasValue) schedule.EndTime = request.EndTime.Value;
        if (request.BreakStartTime.HasValue) schedule.BreakStartTime = request.BreakStartTime.Value;
        if (request.BreakEndTime.HasValue) schedule.BreakEndTime = request.BreakEndTime.Value;

        return await weekDayScheduleRepository.UpdateAsync(schedule);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await weekDayScheduleRepository.DeleteAsync(id);
    }
}
