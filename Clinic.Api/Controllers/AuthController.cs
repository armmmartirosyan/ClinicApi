using Clinic.Core.Interfaces;
using Clinic.Core.Models.Request;
using Clinic.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public IActionResult SignIn(SignInRequest request)
    {
        _authService.SignIn(request);
        return Ok();
    }
}
