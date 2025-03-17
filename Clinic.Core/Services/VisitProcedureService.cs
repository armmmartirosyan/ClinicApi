using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;
using Clinic.Core.Interfaces.Helpers;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class VisitProcedureService
    (
        IFileHelper fileHelper,
        IVisitProcedureRepository visitProcedureRepository,
        AbstractValidator<AddVisitProcedureRequest> addVisitProcedureValidator,
        AbstractValidator<UpdateVisitProcedureRequest> updateVisitProcedureValidator
    ) : IVisitProcedureService
{
    public async Task<long> AddAsync(AddVisitProcedureRequest request)
    {
        ValidationResult validationResult = addVisitProcedureValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var visitsProcedure = new VisitsProcedure
        {
            VisitId = request.VisitId,
            ProcedureId = request.ProcedureId,
            Notes = request.Notes,
        };

        long visitProcedureId = await visitProcedureRepository.AddAsync(visitsProcedure);

        List<string> uploadedFilePaths = await fileHelper.WriteImagesAsync(request.Images);

        if (uploadedFilePaths.Count > 0)
        {
            List<ProcedureImage> procedureImages = new List<ProcedureImage>();

            foreach (string imagePath in uploadedFilePaths)
            {
                procedureImages.Add(new ProcedureImage()
                {
                    VisitProcedureId = visitProcedureId,
                    Url = imagePath
                });
            }

            await visitProcedureRepository.AddProcedureImageAsync(procedureImages);
        }

        return visitProcedureId;
    }

    public async Task<VisitsProcedure> GetByIdAsync(long id)
    {
        var visitProcedure = await visitProcedureRepository.GetByIdAsync(id);

        if (visitProcedure == null)
        {
            throw new InvalidDataException("Not found visit procedure with this ID.");
        }

        return visitProcedure;
    }

    public async Task<InfiniteScrollDTO<VisitsProcedure>> GetAllAsync(int page, int pageSize)
    {
        return await visitProcedureRepository.GetAllAsync(page, pageSize);
    }

    public async Task<bool> UpdateAsync(long id, UpdateVisitProcedureRequest request)
    {
        var visitProcedure = await GetByIdAsync(id);

        ValidationResult validationRes = updateVisitProcedureValidator.Validate(request);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        visitProcedure.Notes = request.Notes;   

        return await visitProcedureRepository.UpdateAsync(visitProcedure!);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var procedureImages = await visitProcedureRepository.GetImagesByVisitProcedureIdAsync(id);

        bool success = await visitProcedureRepository.DeleteAsync(id);

        if (!success)
        { 
            throw new InvalidDataException("Failed deleting the visit procedure.");
        }

        foreach (var image in procedureImages)
        {
            fileHelper.DeleteImage(image.Url);
        }

        return success;
    }

    public async Task<bool> DeleteImageByUrlAsync(string url)
    {
        bool success = await visitProcedureRepository.DeleteImageByUrlAsync(url);

        fileHelper.DeleteImage(url);

        if (!success)
        {
            throw new InvalidDataException("Failed deleting the procedure image.");
        }

        return success;
    }

    public async Task UploadVisitProcedureImagesAsync(UploadProcedureImagesRequest request)
    {
        await GetByIdAsync(request.VisitProcedureId);

        List<string> uploadedFilePaths = await fileHelper.WriteImagesAsync(request.Images);

        if (uploadedFilePaths.Count > 0)
        {
            List<ProcedureImage> procedureImages = new List<ProcedureImage>();

            foreach (string imagePath in uploadedFilePaths)
            {
                procedureImages.Add(new ProcedureImage()
                {
                    VisitProcedureId = request.VisitProcedureId,
                    Url = imagePath
                });
            }

            await visitProcedureRepository.AddProcedureImageAsync(procedureImages);
        }
    }
}
