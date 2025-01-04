using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Repositories;

public interface IWeekDayScheduleRepository
{
    Task<long> CreateAsync(WeekDaySchedule schedule);
    Task<IEnumerable<WeekDaySchedule>> GetSchedulesByDoctorAsync(long doctorId);
    Task<WeekDaySchedule> GetSchedulesByWeekDayAsync(int dayId);
    Task<WeekDaySchedule> GetByIdAsync(long id);
    Task<bool> UpdateAsync(WeekDaySchedule schedule);
    Task<bool> DeleteAsync(long id);
    Task<bool> IsValidDoctorIdAsync(long doctorId);
    Task<bool> IsValidWeekDayIdAsync(int weekDayId);
}
