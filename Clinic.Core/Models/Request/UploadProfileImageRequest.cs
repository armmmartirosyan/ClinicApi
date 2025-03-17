using Microsoft.AspNetCore.Http;

namespace Clinic.Core.Models.Request;

public class UploadProfileImageRequest
{
    public required IFormFile Image { get; set; }
}
