using Microsoft.AspNetCore.Http;

namespace Clinic.Core.Interfaces.Helpers;

public interface IFileHelper
{
    Task<List<string>> WriteImageAsync(List<IFormFile>? files);
    bool DeleteImage(string url);
    string GetImageUploadsDir();
}
