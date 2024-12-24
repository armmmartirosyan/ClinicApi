using Clinic.Core.Interfaces;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Services;

public class AuthService : IAuthService
{
    public void SignIn(SignInRequest request)
    {
        Console.WriteLine(request);
    }
}
