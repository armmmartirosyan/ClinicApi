using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.DTO;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;

public class NotWorkingDaysService
    (
        INotWorkingDaysRepository notWorkingDaysRepository,
        AbstractValidator<CreateNotWorkingDayRequestValidator> createNotWorkingDateValidator,
        AbstractValidator<UpdateNotWorkingDateValidateDTO> updateNotWorkingDateValidator
    ) : INotWorkingDaysService
{
    public async Task<long> CreateAsync(CreateNotWorkingDayRequest request, long doctorId)
    {
        ValidationResult validationResult = createNotWorkingDateValidator.Validate(new CreateNotWorkingDayRequestValidator()
        {
            DoctorId = doctorId,
            NotWorkDate = request.NotWorkDate
        });

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var notWorkingDay = new NotWorkingDay
        {
            NotWorkDate = request.NotWorkDate,
            DoctorId = doctorId,
        };

        return await notWorkingDaysRepository.CreateAsync(notWorkingDay);
    }

    public async Task<NotWorkingDay> GetByIdAsync(long id)
    {
        NotWorkingDay notWorkingDay = await notWorkingDaysRepository.GetByIdAsync(id);

        if (notWorkingDay == null)
        {
            throw new Exception("Not found day with this ID.");
        }

        return notWorkingDay;
    }

    public async Task<IEnumerable<NotWorkingDay>> GetByDoctorIdAsync(long doctorId)
    {
        return await notWorkingDaysRepository.GetByDoctorIdAsync(doctorId);
    }

    public async Task<bool> UpdateAsync(long id, UpdateNotWorkingDateRequest request)
    {
        var validationObject = new UpdateNotWorkingDateValidateDTO ()
        {
            Id = id,
            NotWorkDate = request.NotWorkDate
        };

        ValidationResult validationRes = updateNotWorkingDateValidator.Validate(validationObject);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        NotWorkingDay notWorkingDay = await notWorkingDaysRepository.GetByIdAsync (id);

        notWorkingDay.NotWorkDate = request.NotWorkDate;

        return await notWorkingDaysRepository.UpdateAsync(notWorkingDay);
    }

    public async Task DeleteAsync(long id)
    {
        bool success =  await notWorkingDaysRepository.DeleteAsync(id);

        if (!success)
        {
            throw new InvalidDataException("Failed deleting the day.");
        }
    }
}
