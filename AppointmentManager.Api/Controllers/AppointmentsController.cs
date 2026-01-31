using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Application.DTOs;
using AppointmentManager.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;
    private readonly ICurrentUserService _currentUser;

    public AppointmentsController(IAppointmentService service, ICurrentUserService currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    [HttpGet("appointments")]
    public async Task<ActionResult<List<AppointmentDto>>> GetAll(
        [FromQuery] DateTime? dateFrom, 
        [FromQuery] DateTime? dateTo, 
        [FromQuery] AppointmentStatus? status, 
        [FromQuery] int? clientId, 
        [FromQuery] int? serviceId)
    {
        return Ok(await _service.GetAllAsync(dateFrom, dateTo, status, clientId, serviceId));
    }

    [HttpGet("appointments/{id}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost("appointments")]
    public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("appointments/{id}")]
    public async Task<IActionResult> Update(int id, UpdateAppointmentDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("appointments/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _service.CancelAsync(id); // Changes status to Cancelled
        return NoContent();
    }
    
    [HttpGet("availability")]
    public async Task<ActionResult<List<AppointmentDto>>> GetAvailability([FromQuery] DateTime date)
    {
        // Return busy slots for the current user (connected user context)
        // Usually needed when creating an appointment for self/resource.
        // Assuming availability of the resource (User).
        return Ok(await _service.GetAvailabilityAsync(date, _currentUser.UserId));
    }
}
