using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProceduresController(IProceduresService proceduresService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(AddProcedureRequest request)
    {
        try
        {
            long id = await proceduresService.AddAsync(request);

            return Ok(new Response()
            {
                Data = id,
                Message = "Procedure created successfully.",
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
        var procedures = await proceduresService.GetProceduresAsync();

        return Ok(new Response()
        {
            Data = procedures,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var procedure = await proceduresService.GetByIdAsync(id);

            return Ok(new Response()
            {
                Data = procedure,
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
    public async Task<IActionResult> Update(long id, UpdateProcedureRequest request)
    {
        try
        {
            bool success = await proceduresService.UpdateAsync(id, request);

            return Ok(new Response()
            {
                Data = success,
                Message = "Procedure updated successfully.",
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
            await proceduresService.DeleteAsync(id);

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
