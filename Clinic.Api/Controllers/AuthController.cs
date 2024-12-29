using Clinic.Core.Interfaces;
using Clinic.Core.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        await authService.SignIn(request);
        return Ok();
    }
}
