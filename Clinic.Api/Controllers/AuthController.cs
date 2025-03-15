using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Clinic.Core.Models.DTO;
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
            string token = await authService.SignInAsync(request);

            return Ok(new Response()
            {
                Data = token,
                Message = "",
                Success = true
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new Response()
            {
                Data = null,
                Message = ex.Message,
                Success = false
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            string token = await authService.RegisterAsync(request);

            return Ok(new Response()
            {
                Data = token,
                Message = "Registration successful.",
                Success = true
            });
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(new Response()
            {
                Data = null,
                Message = ex.Message,
                Success = false
            });
        }
    }
    
    [HttpGet("{name}")]
    public async Task<IActionResult> UserType(string name)
    {
        try
        {
            UserType? userType = await authService.GetUserTypeByName(name);

            return Ok(new Response()
            {
                Data = userType,
                Message = "",
                Success = true
            });
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(new Response()
            {
                Data = null,
                Message = ex.Message,
                Success = false
            });
        }
    }
    
    [HttpGet("{page}")]
    public async Task<IActionResult> Doctors(int page)
    {
        try
        {
            int pageSize = 1;
            InfiniteScrollDTO<User> data = await authService.GetDoctors(page, pageSize);

            return Ok(new Response()
            {
                Data = data,
                Message = "",
                Success = true
            });
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(new Response()
            {
                Data = null,
                Message = ex.Message,
                Success = false
            });
        }
    }
}
