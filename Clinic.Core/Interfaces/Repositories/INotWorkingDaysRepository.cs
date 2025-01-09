using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Interfaces.Repositories;

public interface INotWorkingDaysRepository
{
    Task<long> CreateAsync(NotWorkingDay notWorkingDay);
    Task<IEnumerable<NotWorkingDay>> GetByDoctorIdAsync(long doctorId);
    Task<NotWorkingDay> GetByIdAsync(long id);
    Task<bool> UpdateAsync(NotWorkingDay notWorkingDay);
    Task<bool> DeleteAsync(long id);
    Task<bool> IsValidDoctorIdAsync(long doctorId);
    Task<bool> DateNotRegisteredAlready(long doctorId, DateOnly date);
    Task<bool> DateNotRegisteredAlreadyUpdate(UpdateNotWorkingDateValidateDTO dto);
}
