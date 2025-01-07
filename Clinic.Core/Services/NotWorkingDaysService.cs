using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;

public class NotWorkingDaysService
    (
        INotWorkingDaysRepository notWorkingDaysRepository,
        AbstractValidator<CreateNotWorkingDayRequest> createNotWorkingDateValidator,
        AbstractValidator<UpdateNotWorkingDateRequest> updateNotWorkingDateValidator
    ) : INotWorkingDaysService
{
    public async Task<long> CreateAsync(CreateNotWorkingDayRequest request)
    {
        ValidationResult validationResult = createNotWorkingDateValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var notWorkingDay = new NotWorkingDay
        {
            NotWorkDate = request.NotWorkDate,
            DoctorId = request.DoctorId,
        };

        return await notWorkingDaysRepository.CreateAsync(notWorkingDay);
    }

    public async Task<NotWorkingDay> GetByIdAsync(long id)
    {
        return await notWorkingDaysRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<NotWorkingDay>> GetByDoctorIdAsync(long doctorId)
    {
        return await notWorkingDaysRepository.GetByDoctorIdAsync(doctorId);
    }

    public async Task<bool> UpdateAsync(long id, UpdateNotWorkingDateRequest request)
    {
        var notWorkingDay = await notWorkingDaysRepository.GetByIdAsync(id);

        if (notWorkingDay == null)
        {
            throw new InvalidDataException("Day not found.");
        }

        ValidationResult validationRes = updateNotWorkingDateValidator.Validate(request);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        notWorkingDay.NotWorkDate = request.NotWorkDate;

        return await notWorkingDaysRepository.UpdateAsync(notWorkingDay);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await notWorkingDaysRepository.DeleteAsync(id);
    }
}
