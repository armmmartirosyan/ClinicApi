using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.DTO;
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
        return await dbContext.NotWorkingDays
            .Where(s => s.DoctorId == doctorId)
            .OrderByDescending(s => s.NotWorkDate)
            .ToListAsync();
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

    public async Task<bool> DateNotRegisteredAlready(long doctorId, DateOnly date)
    {
        var alreadyRegistered = await dbContext.NotWorkingDays.FirstOrDefaultAsync(d => 
            d.NotWorkDate == date && d.DoctorId == doctorId);

        return alreadyRegistered == null;
    }

    public async Task<bool> DateNotRegisteredAlreadyUpdate(UpdateNotWorkingDateValidateDTO dto)
    {
        var currentDate = await dbContext.NotWorkingDays.FirstAsync(d => d.Id == dto.Id);

        if (currentDate == null) return false;

        var alreadyRegistered = await dbContext.NotWorkingDays.FirstOrDefaultAsync(d =>
            d.NotWorkDate == dto.NotWorkDate && d.DoctorId == currentDate.DoctorId);

        return alreadyRegistered == null;
    }
}
