using Microsoft.AspNetCore.Http;

namespace Clinic.Core.Interfaces.Helpers;

public interface IFileHelper
{
    Task<List<string>> WriteImagesAsync(List<IFormFile>? files);
    Task<string?> WriteImageAsync(IFormFile? file);
    bool DeleteImage(string url);
}
