using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class SpecializationsService
    (
        ISpecializationsRepository specializationsRepository, 
        AbstractValidator<AddUpdateSpecializationRequest> addUpdateSpecializationValidator
    ) : ISpecializationsService
{
    public async Task<int> AddAsync(AddUpdateSpecializationRequest request)
    {
        ValidationResult validationResult = addUpdateSpecializationValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var specialization = new Specialization
        {
            Name = request.Name,
        };

        return await specializationsRepository.AddAsync(specialization);
    }

    public async Task<Specialization> GetByIdAsync(int id)
    {
        var specialization = await specializationsRepository.GetByIdAsync(id);

        if (specialization == null)
        {
            throw new Exception("Not found specialization with this ID.");
        }

        return specialization;
    }

    public async Task<IEnumerable<Specialization>> GetAllAsync()
    {
        return await specializationsRepository.GetAllAsync();
    }

    public async Task<bool> UpdateAsync(int id, AddUpdateSpecializationRequest request)
    {

        ValidationResult validationRes = addUpdateSpecializationValidator.Validate(request);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        var specialization = await specializationsRepository.GetByIdAsync(id);

        if (request.Name != null)
        {
            specialization!.Name = request.Name;
        }

        return await specializationsRepository.UpdateAsync(specialization!);
    }

    public async Task DeleteAsync(int id)
    {
        bool success = await specializationsRepository.DeleteAsync(id);

        if (!success)
        {
            throw new InvalidDataException("Failed deleting the specialization.");
        }
    }
}
