﻿using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Repositories;

public interface IVisitProcedureRepository
{
    Task<long> AddAsync(VisitsProcedure visitProcedure);
    Task<List<VisitsProcedure>> GetAllAsync();
    Task<VisitsProcedure?> GetByIdAsync(long id);
    Task<bool> UpdateAsync(VisitsProcedure visitProcedure);
    Task<bool> DeleteAsync(long id);
    Task<bool> IsValidProcedureIdAsync(long id);
    Task<bool> IsValidVisitIdAsync(long id);
    Task<bool> AddProcedureImageAsync(List<ProcedureImage> procedureImages);
    Task<List<ProcedureImage>> GetImagesByVisitProcedureIdAsync(long id);
    Task<bool> DeleteImageByUrlAsync(string url);
}
