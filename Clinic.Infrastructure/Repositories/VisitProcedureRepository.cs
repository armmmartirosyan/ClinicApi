﻿using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;
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

    public async Task<bool> AddProcedureImageAsync(List<ProcedureImage> procedureImages)
    {
        dbContext.ProcedureImages.AddRange(procedureImages);

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<InfiniteScrollDTO<VisitsProcedure>> GetAllAsync(int page, int pageSize, DecodedTokenDTO decodedToken)
    {
        var totalItems = await dbContext
            .VisitsProcedures
            .Where(m => decodedToken.UserId == (decodedToken.Role == "Doctor" ? m.Visit.DoctorId : m.Visit.PatientId))
            .CountAsync();
        var allowNext = (page * pageSize) < totalItems;
        
        List<VisitsProcedure> procedures = await dbContext.VisitsProcedures
            .Where(m => decodedToken.UserId == (decodedToken.Role == "Doctor" ? m.Visit.DoctorId : m.Visit.PatientId))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(dbContext.VisitsProcedures, vp => vp.Id, vp => vp.Id, (vp, _) => new VisitsProcedure
            {
                Id = vp.Id,
                Notes = vp.Notes,
                VisitId = vp.VisitId,
                CreatedAt = vp.CreatedAt,
                ProcedureId = vp.ProcedureId,
                Visit = vp.Visit,
                Procedure = vp.Procedure,
                ProcedureImages = vp.ProcedureImages,
                MedicinesAssigneds = vp.MedicinesAssigneds,
            })
            .ToListAsync();
        
        return new InfiniteScrollDTO<VisitsProcedure>()
        {
            AllowNext = allowNext,
            Data = procedures
        };
    }

    public async Task<VisitsProcedure?> GetByIdAsync(long id)
    {
        return await dbContext.VisitsProcedures.Join(dbContext.VisitsProcedures, vp => vp.Id, vp => vp.Id, (vp, _) => new VisitsProcedure
            {
                Id = vp.Id,
                Notes = vp.Notes,
                VisitId = vp.VisitId,
                CreatedAt = vp.CreatedAt,
                ProcedureId = vp.ProcedureId,
                Visit = vp.Visit,
                Procedure = vp.Procedure,
                ProcedureImages = vp.ProcedureImages,
                MedicinesAssigneds = vp.MedicinesAssigneds,
            }).FirstAsync(s => s.Id == id);
    }

    public async Task<List<ProcedureImage>> GetImagesByVisitProcedureIdAsync(long id)
    {
        return await dbContext.ProcedureImages
            .Where(pi => pi.VisitProcedureId == id)
            .ToListAsync();
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

        var procedureImages = await dbContext.ProcedureImages.Where(p => p.VisitProcedureId == id).ToListAsync();

        dbContext.ProcedureImages.RemoveRange(procedureImages);
        dbContext.VisitsProcedures.Remove(visitProcedure);
        return await dbContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteMedicinesAsync(long id)
    {
        var medicines = await dbContext.MedicinesAssigneds.Where(p => p.VisitProcedureId == id).ToListAsync();

        if (medicines.Count > 0)
        {
            dbContext.MedicinesAssigneds.RemoveRange(medicines);
            return await dbContext.SaveChangesAsync() > 0;
        }

        return true;
    }

    public async Task<bool> DeleteImageByUrlAsync(string url)
    {
        var procedureImage = await dbContext.ProcedureImages.FirstOrDefaultAsync(pi => pi.Url == url);

        if (procedureImage == null) return false;

        dbContext.ProcedureImages.Remove(procedureImage);
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
