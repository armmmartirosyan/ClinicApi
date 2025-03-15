using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Clinic.Core.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class VisitProcedureController(IVisitProcedureService visitProcedureService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromForm]  AddVisitProcedureRequest request)
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

    [HttpPost]
    public async Task<IActionResult> UploadImages([FromForm] UploadProcedureImagesRequest request)
    {
        try
        {
            await visitProcedureService.UploadVisitProcedureImagesAsync(request);

            return Ok(new Response()
            {
                Data = null,
                Message = "Images uploaded successfully.",
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
    public async Task<IActionResult> GetAll(int page)
    {
        int pageSize = 5;
        var data = await visitProcedureService.GetAllAsync(page, pageSize);

        return Ok(new Response()
        {
            Data = data,
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

    [HttpDelete("{url}")]
    public async Task<IActionResult> DeleteProcedureImage(string url)
    {
        try
        {
            await visitProcedureService.DeleteImageByUrlAsync(url);

            return Ok(new Response()
            {
                Data = null,
                Message = "Procedure image deleted successfully.",
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
