using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface INotWorkingDaysService
{
    Task<long> CreateAsync(CreateNotWorkingDayRequest request, long doctorId);
    Task<NotWorkingDay> GetByIdAsync(long id);
    Task<IEnumerable<NotWorkingDay>> GetByDoctorIdAsync(long doctorId);
    Task<bool> UpdateAsync(long id, UpdateNotWorkingDateRequest request);
    Task DeleteAsync(long id);
}
