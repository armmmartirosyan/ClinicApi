using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IAuthService
{
    Task<string> SignInAsync(SignInRequest request);
    Task<string> RegisterAsync(RegisterRequest request);
    Task<UserType?> GetUserTypeByName(string name);
}
