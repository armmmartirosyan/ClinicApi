using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;
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

    public async Task<InfiniteScrollDTO<MedicinesAssigned>> GetAllAsync(int page, int pageSize)
    {
        var totalItems = await dbContext.MedicinesAssigneds.CountAsync();
        var allowNext = (page * pageSize) < totalItems;
        
        List<MedicinesAssigned> medicinesAssigned =  await dbContext.MedicinesAssigneds
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(dbContext.MedicinesAssigneds, m => m.Id, m => m.Id, (m, _) => new MedicinesAssigned
            {
                Id = m.Id,
                Notes = m.Notes,
                DoctorId = m.DoctorId,
                PatientId = m.PatientId,
                Doctor = m.Doctor,
                Patient = m.Patient,
                Name = m.Name,
                Dose = m.Dose,
                StartDate = m.StartDate,
                DayCount = m.DayCount,
                Quantity = m.Quantity,
                VisitProcedureId = m.VisitProcedureId,
                VisitProcedure = m.VisitProcedure
            })
            .ToListAsync();
        
        return new InfiniteScrollDTO<MedicinesAssigned>()
        {
            AllowNext = allowNext,
            Data = medicinesAssigned
        };
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
