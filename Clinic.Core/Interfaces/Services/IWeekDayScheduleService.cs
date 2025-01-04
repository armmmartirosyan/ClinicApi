using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IWeekDayScheduleService
{
    Task<long> CreateAsync(CreateWeekDayScheduleRequest request);
    Task<IEnumerable<WeekDaySchedule>> GetSchedulesByDoctorAsync(long doctorId);
    Task<WeekDaySchedule> GetByIdAsync(long id);
    Task<bool> UpdateAsync(long id, UpdateWeekDayScheduleRequest request);
    Task<bool> DeleteAsync(long id);
}
