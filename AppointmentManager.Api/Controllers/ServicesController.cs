using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentManager.Api.Controllers;

[Authorize] // Maybe allow anonymous read? Requirement says "Protéger toutes les routes sauf login/register"
[ApiController] // So all protected.
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _service;

    public ServicesController(IServiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [Authorize(Roles = "Admin")] // Requirement: Admin handles services
    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create(CreateServiceDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateServiceDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> Toggle(int id)
    {
        await _service.ToggleActiveAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
