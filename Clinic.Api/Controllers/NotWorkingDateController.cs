using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
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
                Message = "Not working date created successfully.",
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

    [HttpGet("{doctorId}")]
    public async Task<IActionResult> GetByDoctorId(long doctorId)
    {
        var notWorkingDays = await notWorkingDaysService.GetByDoctorIdAsync(doctorId);

        return Ok(new Response()
        {
            Data = notWorkingDays,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var notWorkingDay = await notWorkingDaysService.GetByIdAsync(id);

        if (notWorkingDay == null)
        {
            return NotFound(new Response()
            {
                Data = null,
                Message = "Not found day with this ID.",
                Success = false
            });
        }

        return Ok(new Response()
        {
            Data = notWorkingDay,
            Message = "",
            Success = true
        });
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
        bool success = await notWorkingDaysService.DeleteAsync(id);

        if (!success)
        {
            return BadRequest(new Response()
            {
                Data = null,
                Message = "Failed deleting the day.",
                Success = false
            });
        }

        return Ok(new Response()
        {
            Data = null,
            Message = "The day deleted successfully.",
            Success = true
        });
    }
}
