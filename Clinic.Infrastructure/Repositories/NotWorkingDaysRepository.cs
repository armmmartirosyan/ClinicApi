using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class NotWorkingDaysRepository(ClinicDbContext dbContext) : INotWorkingDaysRepository
{
    public async Task<long> CreateAsync(NotWorkingDay notWorkingDay)
    {
        var res = await dbContext.NotWorkingDays.AddAsync(notWorkingDay);
        await dbContext.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<IEnumerable<NotWorkingDay>> GetByDoctorIdAsync(long doctorId)
    {
        return await dbContext.NotWorkingDays.Where(s => s.DoctorId == doctorId).ToListAsync();
    }

    public async Task<NotWorkingDay> GetByIdAsync(long id)
    {
        return await dbContext.NotWorkingDays.FirstAsync(s => s.Id == id);
    }

    public async Task<bool> UpdateAsync(NotWorkingDay notWorkingDay)
    {
        dbContext.NotWorkingDays.Update(notWorkingDay);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var notWorkingDay = await dbContext.NotWorkingDays.FindAsync(id);
        if (notWorkingDay == null) return false;

        dbContext.NotWorkingDays.Remove(notWorkingDay);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsValidDoctorIdAsync(long doctorId)
    {
        var doctorType = await dbContext.UserTypes.FirstOrDefaultAsync(t => t.Name == "Doctor");

        return await dbContext.Users.AnyAsync(ut => ut.Id == doctorId && ut.TypesId == doctorType.Id);
    }

    //public async Task<bool> IsValidWeekDayIdAsync(int weekDayId)
    //{
    //    return await dbContext.WeekDays.AnyAsync(ut => ut.Id == weekDayId);
    //}

    //public async Task<bool> WeekDayNotRegistered(int weekDayId, long doctorId)
    //{
    //    bool weekDayScheduleExists = await dbContext.WeekDaySchedules.AnyAsync(w => w.WeekDayId == weekDayId && w.DoctorId == doctorId);

    //    return !weekDayScheduleExists;
    //}
}
