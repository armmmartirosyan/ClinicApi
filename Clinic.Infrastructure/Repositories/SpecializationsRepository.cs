using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class SpecializationsRepository(ClinicDbContext dbContext) : ISpecializationsRepository
{
    public async Task<int> AddAsync(Specialization specialization)
    {
        var res = await dbContext.Specializations.AddAsync(specialization);
        await dbContext.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<List<Specialization>> GetAllAsync()
    {
        return await dbContext.Specializations.ToListAsync();
    }

    public async Task<Specialization?> GetByIdAsync(int id)
    {
        return await dbContext.Specializations.FindAsync(id);
    }

    public async Task<Specialization?> GetByNameAsync(string name)
    {
        return await dbContext.Specializations.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<bool> UpdateAsync(Specialization specialization)
    {
        dbContext.Specializations.Update(specialization);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var specialization = await GetByIdAsync(id);
        if (specialization == null) return false;

        dbContext.Specializations.Remove(specialization);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> NameNotDuplicated(string name)
    {
        var existingSpecialization = await dbContext.Specializations.FirstOrDefaultAsync(p => p.Name == name);
        return existingSpecialization == null;
    }
}
