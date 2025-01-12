using Clinic.Core.Interfaces.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Clinic.Infrastructure.Validators;

public class ImageValidator : AbstractValidator<IFormFile>
{
    public List<string> allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg", ".webp", ".svg" };

    public ImageValidator(IProceduresRepository procedureRepository)
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .Must(HaveAllowedExtension).WithMessage("Invalid file extension")
           .Must(HaveAllowedSize).WithMessage("Invalid file size");
    }

    private bool HaveAllowedExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);

        return allowedExtensions.Contains(extension);
    }
    private bool HaveAllowedSize(IFormFile file)
    {
        var fileSize = file.Length;
        var maxFileSize =  5 *  1024 * 1024;

        return maxFileSize > fileSize;
    }
}
