using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces;

public interface IAuthService
{
    void SignIn(SignInRequest request);
}
