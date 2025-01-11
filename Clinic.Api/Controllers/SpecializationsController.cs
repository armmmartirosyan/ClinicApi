using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SpecializationsController(ISpecializationsService specializationsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(AddUpdateSpecializationRequest request)
    {
        try
        {
            long id = await specializationsService.AddAsync(request);

            return Ok(new Response()
            {
                Data = id,
                Message = "Specialization added successfully.",
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
        var specializations = await specializationsService.GetAllAsync();

        return Ok(new Response()
        {
            Data = specializations,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var specialization = await specializationsService.GetByIdAsync(id);

            return Ok(new Response()
            {
                Data = specialization,
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
    public async Task<IActionResult> Update(int id, AddUpdateSpecializationRequest request)
    {
        try
        {
            bool success = await specializationsService.UpdateAsync(id, request);

            return Ok(new Response()
            {
                Data = success,
                Message = "Specialization updated successfully.",
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
            await specializationsService.DeleteAsync(id);

            return Ok(new Response()
            {
                Data = null,
                Message = "Specialization deleted successfully.",
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
