using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class MedicinesAssignedRepository(ClinicDbContext dbContext) : IMedicinesAssignedRepository
{
    public async Task<long> AddAsync(MedicinesAssigned medicinesAssigned)
    {
        var res = await dbContext.MedicinesAssigneds.AddAsync(medicinesAssigned);
        await dbContext.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<List<MedicinesAssigned>> GetAllAsync()
    {
        return await dbContext.MedicinesAssigneds.ToListAsync();
    }

    public async Task<MedicinesAssigned?> GetByIdAsync(long id)
    {
        return await dbContext.MedicinesAssigneds.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(MedicinesAssigned medicinesAssigned)
    {
        dbContext.MedicinesAssigneds.Update(medicinesAssigned);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var medicinesAssigned = await GetByIdAsync(id);
        if (medicinesAssigned == null) return false;

        dbContext.MedicinesAssigneds.Remove(medicinesAssigned);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsValidUserIdAsync(long userId)
    {
        return await dbContext.Users.AnyAsync(ut => ut.Id == userId);
    }

    public async Task<bool> IsValidDoctorId(long doctorId)
    {
        var doctorType = await dbContext.UserTypes.FirstOrDefaultAsync(t => t.Name == "Doctor");

        return await dbContext.Users.AnyAsync(ut => ut.Id == doctorId && ut.TypesId == doctorType.Id);
    }

    public async Task<bool> IsValidMedicineAssignedId(long id)
    {
        return await dbContext.MedicinesAssigneds.AnyAsync(ut => ut.Id == id);
    }

    public async Task<bool> IsValidVisirProcedureIdAsync(long visitProcedureId)
    {
        return await dbContext.VisitsProcedures.AnyAsync(ut => ut.Id == visitProcedureId);
    }
}
