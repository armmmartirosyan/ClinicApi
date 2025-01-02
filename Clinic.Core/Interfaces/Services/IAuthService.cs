using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;


namespace Clinic.Core.Interfaces.Services;

public interface IAuthService
{
    Task<SignInResponse> SignInAsync(SignInRequest request);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
}
