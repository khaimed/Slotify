using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Application.DTOs;
using AppointmentManager.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManager.Application.Services;

public class ClientService : IClientService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClientService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        var entity = _mapper.Map<Client>(dto);
        _context.Clients.Add(entity);
        await _context.SaveChangesAsync(default);
        return _mapper.Map<ClientDto>(entity);
    }

    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Clients.FindAsync(id);
        if (entity == null) return null;
        return _mapper.Map<ClientDto>(entity);
    }

    public async Task<List<ClientDto>> GetAllAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var query = _context.Clients.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.FullName.Contains(search) || x.Phone.Contains(search) || x.Email.Contains(search));
        }

        var list = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<List<ClientDto>>(list);
    }

    public async Task UpdateAsync(int id, UpdateClientDto dto)
    {
        var entity = await _context.Clients.FindAsync(id);
        if (entity == null) return; // Or throw NotFoundException

        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync(default);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Clients.FindAsync(id);
        if (entity != null)
        {
            _context.Clients.Remove(entity);
            await _context.SaveChangesAsync(default);
        }
    }
}
