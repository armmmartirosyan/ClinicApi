using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class VisitProcedureRepository(ClinicDbContext dbContext) : IVisitProcedureRepository
{
    public async Task<long> AddAsync(VisitsProcedure visitProcedure)
    {
        var res = await dbContext.VisitsProcedures.AddAsync(visitProcedure);
        await dbContext.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<List<VisitsProcedure>> GetAllAsync()
    {
        return await dbContext.VisitsProcedures.ToListAsync();
    }

    public async Task<VisitsProcedure?> GetByIdAsync(long id)
    {
        return await dbContext.VisitsProcedures.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(VisitsProcedure visitProcedure)
    {
        dbContext.VisitsProcedures.Update(visitProcedure);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var visitProcedure = await GetByIdAsync(id);
        if (visitProcedure == null) return false;

        dbContext.VisitsProcedures.Remove(visitProcedure);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsValidProcedureIdAsync(long id)
    {
        return await dbContext.Procedures.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> IsValidVisitIdAsync(long id)
    {
        return await dbContext.Visits.AnyAsync(p => p.Id == id);
    }
}
