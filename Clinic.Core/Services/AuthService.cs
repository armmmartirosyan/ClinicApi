using Clinic.Core.Interfaces;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task SignIn(SignInRequest request)
    {
        await authRepository.Login();
        Console.WriteLine(request);
    }
}
