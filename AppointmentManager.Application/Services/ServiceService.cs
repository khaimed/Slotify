using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Application.DTOs;
using AppointmentManager.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManager.Application.Services;

public class ServiceService : IServiceService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ServiceService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
    {
        var entity = _mapper.Map<Service>(dto);
        _context.Services.Add(entity);
        await _context.SaveChangesAsync(default);
        return _mapper.Map<ServiceDto>(entity);
    }

    public async Task<ServiceDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        return entity == null ? null : _mapper.Map<ServiceDto>(entity);
    }

    public async Task<List<ServiceDto>> GetAllAsync()
    {
        var list = await _context.Services.ToListAsync();
        return _mapper.Map<List<ServiceDto>>(list);
    }

    public async Task UpdateAsync(int id, UpdateServiceDto dto)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity == null) return;

        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync(default);
    }

    public async Task ToggleActiveAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity == null) return;
        
        entity.IsActive = !entity.IsActive;
        await _context.SaveChangesAsync(default);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity != null)
        {
            _context.Services.Remove(entity);
            await _context.SaveChangesAsync(default);
        }
    }
}
