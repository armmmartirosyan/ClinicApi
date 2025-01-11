using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class VisitProcedureController(IVisitProcedureService visitProcedureService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(AddVisitProcedureRequest request)
    {
        try
        {
            long id = await visitProcedureService.AddAsync(request);

            return Ok(new Response()
            {
                Data = id,
                Message = "Visit procedure added successfully.",
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
    public async Task<IActionResult> GetAll()
    {
        var visitProcedures = await visitProcedureService.GetAllAsync();

        return Ok(new Response()
        {
            Data = visitProcedures,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var visitProcedure = await visitProcedureService.GetByIdAsync(id);

            return Ok(new Response()
            {
                Data = visitProcedure,
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
    public async Task<IActionResult> Update(long id, UpdateVisitProcedureRequest request)
    {
        try
        {
            bool success = await visitProcedureService.UpdateAsync(id, request);

            return Ok(new Response()
            {
                Data = success,
                Message = "Visit procedure updated successfully.",
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
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await visitProcedureService.DeleteAsync(id);

            return Ok(new Response()
            {
                Data = null,
                Message = "Visit procedure deleted successfully.",
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
