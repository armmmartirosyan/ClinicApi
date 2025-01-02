using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        try
        {
            var response = await authService.SignInAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
