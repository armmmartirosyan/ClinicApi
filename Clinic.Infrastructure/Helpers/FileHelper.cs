using Clinic.Core.Interfaces.Helpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Clinic.Infrastructure.Helpers;

public class FileHelper(AbstractValidator<IFormFile> imageValidator) : IFileHelper
{
    public async Task<List<string>> WriteImagesAsync(List<IFormFile>? files)
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

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string imageUploadsDir = GetImageUploadsDir();
            string filePath = Path.Combine(imageUploadsDir, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            imagePaths.Add(uniqueFileName);
        }

        return imagePaths;
    }
    
    public async Task<string?> WriteImageAsync(IFormFile? file)
    {
        if (file == null || file.Length <= 0)
        {
            return null;
        }

        ValidationResult imageValidationRes = imageValidator.Validate(file);

        if (!imageValidationRes.IsValid)
        {
            return null;
        }

        string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        string imageUploadsDir = GetImageUploadsDir();
        string filePath = Path.Combine(imageUploadsDir, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return uniqueFileName;
    }

    public bool DeleteImage(string url)
    {
        string imageUploadsDir = GetImageUploadsDir();
        string path = Path.Combine(imageUploadsDir, url);

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

    public static string GetImageUploadsDir()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string rootDir = Directory.GetParent(currentDir).FullName;
        string parentOfRootDir = Directory.GetParent(rootDir).FullName;
        
        string uploadsDir = Path.Combine(parentOfRootDir, "Uploads", "Images");

        if (!Directory.Exists(uploadsDir))
        {
            Directory.CreateDirectory(uploadsDir);
        }

        return uploadsDir;
    }
}
