﻿using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.DTO;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using Clinic.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeekDayScheduleController(IWeekDayScheduleService weekDayScheduleService) : ControllerBase
{
    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateWeekDayScheduleRequest request)
    {
        try
        {
            Request.Headers.TryGetValue("Authorization", out var authHeader);
            DecodedTokenDTO decodedToken = AuthHelper.DecodeToken(authHeader);
            
            long weekDayScheduleId = await weekDayScheduleService.CreateAsync(request, decodedToken.UserId);

            return Ok(new Response()
            {
                Data = weekDayScheduleId,
                Message = "Week day schedule created successfully.",
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
        var schedules = await weekDayScheduleService.GetSchedulesByDoctorAsync(doctorId);

        return Ok(new Response()
        {
            Data = schedules,
            Message = "",
            Success = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var schedule = await weekDayScheduleService.GetByIdAsync(id);

        if (schedule == null)
        {
            return NotFound(new Response()
            {
                Data = null,
                Message = "Not found schedule with this ID.",
                Success = false
            });
        }

        return Ok(new Response()
        {
            Data = schedule,
            Message = "",
            Success = true
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateWeekDayScheduleRequest request)
    {
        try
        {
            bool success = await weekDayScheduleService.UpdateAsync(id, request);

            return Ok(new Response()
            {
                Data = success,
                Message = "Week day schedule updated successfully.",
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
        bool success = await weekDayScheduleService.DeleteAsync(id);

        if (!success)
        {
            return BadRequest(new Response()
            {
                Data = null,
                Message = "Failed deleting schedule.",
                Success = false
            });
        }

        return Ok(new Response()
        {
            Data = null,
            Message = "Schedule deleted successfully.",
            Success = true
        });
    }
}
