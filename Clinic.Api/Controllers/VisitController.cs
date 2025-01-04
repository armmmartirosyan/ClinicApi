using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class VisitController(IVisitService visitService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddVisit([FromBody] AddVisitRequest request)
    {
        try
        {
            var visitId = await visitService.AddVisitAsync(request);

            return Ok(new Response()
            {
                Data = visitId,
                Message = "Visit added successfully.",
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

    [HttpGet]
    public async Task<IActionResult> GetAllVisits()
    {
        var visits = await visitService.GetAllVisitsAsync();
        
        return Ok(new Response()
        {
            Data = visits,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisitById(long id)
    {
        var visit = await visitService.GetVisitByIdAsync(id);
        
        if (visit == null)
        {
            return NotFound(new Response()
            {
                Data = null,
                Message = "Not found visit with this ID.",
                Success = false
            });
        }

        return Ok(new Response()
        {
            Data = visit,
            Message = "",
            Success = true
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVisit(long id, [FromBody] UpdateVisitRequest request)
    {
        try
        {
            var success = await visitService.UpdateVisitAsync(id, request);

            return Ok(new Response()
            {
                Data = null,
                Message = "Visit updated successfully.",
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
    public async Task<IActionResult> DeleteVisit(long id)
    {
        var success = await visitService.DeleteVisitAsync(id);

        if (!success)
        {
            return NotFound(new Response()
            {
                Data = null,
                Message = "Not found visit with this ID.",
                Success = false
            });
        }

        return Ok(new Response()
        {
            Data = null,
            Message = "The visit successfully deleted.",
            Success = true
        });
    }
}
