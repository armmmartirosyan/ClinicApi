using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class WeekDayScheduleRepository(ClinicDbContext dbContext) : IWeekDayScheduleRepository
{
    public async Task<long> CreateAsync(WeekDaySchedule schedule)
    {
        var res = await dbContext.WeekDaySchedules.AddAsync(schedule);

        return res.Entity.Id;
    }

    public async Task<IEnumerable<WeekDaySchedule>> GetSchedulesByDoctorAsync(long doctorId)
    {
        return await dbContext.WeekDaySchedules
            .Where(s => s.DoctorId == doctorId)
            .Include(s => s.WeekDay)
            .ToListAsync();
    }

    public async Task<WeekDaySchedule> GetSchedulesByWeekDayAsync(int dayId)
    {
        return await dbContext.WeekDaySchedules
            .FirstOrDefaultAsync(s => s.WeekDayId == dayId);
    }

    public async Task<WeekDaySchedule> GetByIdAsync(long id)
    {
        return await dbContext.WeekDaySchedules
            .Include(s => s.WeekDay)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> UpdateAsync(WeekDaySchedule schedule)
    {
        dbContext.WeekDaySchedules.Update(schedule);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var schedule = await dbContext.WeekDaySchedules.FindAsync(id);
        if (schedule == null) return false;

        dbContext.WeekDaySchedules.Remove(schedule);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsValidDoctorIdAsync(long doctorId)
    {
        var doctorType = await dbContext.UserTypes.FirstOrDefaultAsync(t => t.Name == "Doctor");

        return await dbContext.Users.AnyAsync(ut => ut.Id == doctorId && ut.TypesId == doctorType.Id);
    }

    public async Task<bool> IsValidWeekDayIdAsync(int weekDayId)
    {
        return await dbContext.WeekDays.AnyAsync(ut => ut.Id == weekDayId);
    }
}
