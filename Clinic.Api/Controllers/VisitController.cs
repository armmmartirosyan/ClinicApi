﻿using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Clinic.Infrastructure.Helpers;
using Clinic.Core.Models.DTO;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class VisitController(IVisitService visitService) : ControllerBase
{
    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddVisitRequest request)
    {
        try
        {
            Request.Headers.TryGetValue("Authorization", out var authHeader);
            DecodedTokenDTO decodedToken = AuthHelper.DecodeToken(authHeader);
            
            var visitId = await visitService.AddVisitAsync(request, decodedToken.UserId);

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
    
    [Authorize(Roles = "Doctor,Patient")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAll(long id)
    {
        Request.Headers.TryGetValue("Authorization", out var authHeader);
        DecodedTokenDTO decodedToken = AuthHelper.DecodeToken(authHeader);
        
        var visits = await visitService.GetAllVisitsAsync(decodedToken, id);
        
        return Ok(new Response()
        {
            Data = visits,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
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

    [Authorize(Roles = "Doctor,Patient")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateVisitRequest request)
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
    public async Task<IActionResult> Delete(long id)
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
