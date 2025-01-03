using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IAuthService
{
    Task<string> SignInAsync(SignInRequest request);
    Task<long> RegisterAsync(RegisterRequest request);
}
