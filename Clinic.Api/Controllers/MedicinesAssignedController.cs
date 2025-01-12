using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MedicinesAssignedController(IMedicinesAssignedService medicinesAssignedService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(AddMedicinesAssignedRequest request)
    {
        try
        {
            long id = await medicinesAssignedService.AddAsync(request);

            return Ok(new Response()
            {
                Data = id,
                Message = "Medicine assigned successfully.",
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
        var medicinesAssigned = await medicinesAssignedService.GetAllAsync();

        return Ok(new Response()
        {
            Data = medicinesAssigned,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var medicineAssigned = await medicinesAssignedService.GetByIdAsync(id);

            return Ok(new Response()
            {
                Data = medicineAssigned,
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
    public async Task<IActionResult> Update(int id, UpdateMedicinesAssignedRequest request)
    {
        try
        {
            bool success = await medicinesAssignedService.UpdateAsync(id, request);

            return Ok(new Response()
            {
                Data = success,
                Message = "Assigned medicine updated successfully.",
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
            await medicinesAssignedService.DeleteAsync(id);

            return Ok(new Response()
            {
                Data = null,
                Message = "Assigned medicine deleted successfully.",
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
