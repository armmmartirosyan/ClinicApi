using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class ProceduresRepository(ClinicDbContext dbContext) : IProceduresRepository
{
    public async Task<long> AddProcedureAsync(Procedure procedure)
    {
        var res = await dbContext.Procedures.AddAsync(procedure);
        await dbContext.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<List<Procedure>> GetAllProceduresAsync()
    {
        return await dbContext.Procedures.ToListAsync();
    }

    public async Task<Procedure?> GetProcedureByIdAsync(long id)
    {
        return await dbContext.Procedures.FindAsync(id);
    }

    public async Task<Procedure?> GetProcedureByNameAsync(string name)
    {
        return await dbContext.Procedures.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<bool> UpdateProcedureAsync(Procedure procedure)
    {
        dbContext.Procedures.Update(procedure);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteProcedureAsync(long id)
    {
        var procedure = await GetProcedureByIdAsync(id);
        if (procedure == null) return false;

        dbContext.Procedures.Remove(procedure);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> ProcedureNameNotDuplicated(string name)
    {
        var existingProcedure = await dbContext.Procedures.FirstOrDefaultAsync(p => p.Name == name);
        return existingProcedure == null;
    }
}
