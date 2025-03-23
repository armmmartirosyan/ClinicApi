using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class VisitRepository(ClinicDbContext dbContext) : IVisitRepository
{
    public async Task<long> AddVisitAsync(Visit visit)
    {
        var res = await dbContext.Visits.AddAsync(visit);
        await dbContext.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<List<Visit>> GetAllVisitsAsync(DecodedTokenDTO decodedToken, long  doctorId)
    {
        return await dbContext.Visits
            .Where(v => decodedToken.Role == "Doctor" ? v.DoctorId == decodedToken.UserId :  
                (doctorId != 0 && doctorId >= 1) ? v.DoctorId == doctorId : 
                v.PatientId == decodedToken.UserId)
            .Join(dbContext.Visits, v => v.Id, v => v.Id, (v, _) => new Visit
            {
                Id = v.Id,
                Notes = v.Notes,
                CreatedAt = v.CreatedAt,
                DoctorId = v.DoctorId,
                PatientId = v.PatientId,
                StartScheduledDate = v.StartScheduledDate,
                EndScheduledDate = v.EndScheduledDate,
                StartActualDate = v.StartActualDate,
                EndActualDate = v.EndActualDate,
                StatusId = v.StatusId,
                VisitsProcedures = v.VisitsProcedures,
                Doctor = v.Doctor,
                Patient = v.Patient,
                Status = v.Status
            })
            .ToListAsync();
        
    }

    public async Task<Visit?> GetVisitByIdAsync(long id)
    {
        return await dbContext.Visits.FindAsync(id);
    }

    public async Task<bool> UpdateVisitAsync(Visit visit)
    {
        dbContext.Visits.Update(visit);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteVisitAsync(long id)
    {
        var visit = await GetVisitByIdAsync(id);
        if (visit == null) return false;

        dbContext.Visits.Remove(visit);
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

    public async Task<bool> IsValidStatusIdAsync(int? statusId)
    {
        return await dbContext.VisitStatuses.AnyAsync(vs => vs.Id == statusId);
    }

    public async Task<VisitStatus> GetVisitStatusAsync(string name)
    {
        return await dbContext.VisitStatuses.FirstOrDefaultAsync(vs => vs.Name == name);
    }
}
