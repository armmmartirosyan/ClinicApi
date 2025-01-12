using Clinic.Core.Interfaces.Helpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Clinic.Infrastructure.Helpers;

public class FileHelper(AbstractValidator<IFormFile> imageValidator) : IFileHelper
{
    public async Task<List<string>> WriteImageAsync(List<IFormFile>? files)
    {
        if (files == null || !files.Any(file => file.Length > 0))
        {
            return new List<string>();
        }

        var imagePaths = new List<string>();

        foreach (var file in files)
        {
            ValidationResult imageValidationRes = imageValidator.Validate(file);

            if (!imageValidationRes.IsValid)
            {
                continue;
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fileNameCombinedFolder = Path.Combine("Uploads", "Images", uniqueFileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileNameCombinedFolder);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            imagePaths.Add(uniqueFileName);
        }

        return imagePaths;
    }

    public bool DeleteImage(string url)
    {
        var path = Path.Combine("Uploads", "Images", url);

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"File {path} deleted successfully.");
                return true;
            }
            else
            {
                Console.WriteLine($"File {path} does not exist.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            return false;
        }
    }
}
