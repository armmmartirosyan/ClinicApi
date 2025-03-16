using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
//using Clinic.Infrastructure.Helpers;
//using Clinic.Core.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NotWorkingDateController(INotWorkingDaysService notWorkingDaysService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateNotWorkingDayRequest request)
    {
        try
        {
            long id = await notWorkingDaysService.CreateAsync(request);

            return Ok(new Response()
            {
                Data = id,
                Message = "Not working day created successfully.",
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

    [Authorize(Roles = "Doctor,Patient")]
    [HttpGet("{doctorId}")]
    public async Task<IActionResult> GetByDoctorId(long doctorId)
    {
        //Request.Headers.TryGetValue("Authorization", out var authHeader);
        //DecodedTokenDTO decodedToken = AuthHelper.DecodeToken(authHeader);

        var notWorkingDays = await notWorkingDaysService.GetByDoctorIdAsync(doctorId);

        return Ok(new Response()
        {
            Data = notWorkingDays,
            Message = "",
            Success = true
        });
    }

    [Authorize(Roles = "Patient")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var notWorkingDay = await notWorkingDaysService.GetByIdAsync(id);

            return Ok(new Response()
            {
                Data = notWorkingDay,
                Message = "",
                Success = true
            });
        }
        catch (Exception ex)
        {
            return NotFound(new Response()
            {
                Data = null,
                Message = ex.Message,
                Success = false
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateNotWorkingDateRequest request)
    {
        try
        {
            bool success = await notWorkingDaysService.UpdateAsync(id, request);

            return Ok(new Response()
            {
                Data = success,
                Message = "Not working day updated successfully.",
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await notWorkingDaysService.DeleteAsync(id);

            return Ok(new Response()
            {
                Data = null,
                Message = "The day deleted successfully.",
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
