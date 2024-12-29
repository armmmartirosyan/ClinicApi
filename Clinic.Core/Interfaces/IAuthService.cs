using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces;

public interface IAuthService
{
    Task SignIn(SignInRequest request);
}
