using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Application.DTOs;
using AppointmentManager.Domain.Entities;
using AppointmentManager.Domain.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManager.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IServiceService _serviceService;

    public AppointmentService(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser, IServiceService serviceService)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
        _serviceService = serviceService;
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        // 1. Get Service Duration to calculate EndAt
        var service = await _serviceService.GetByIdAsync(dto.ServiceId);
        if (service == null) throw new Exception("Service not found");

        var startAt = dto.StartAt;
        var endAt = startAt.AddMinutes(service.DurationMinutes);
        var userId = _currentUser.UserId;

        // 2. Check Overlap
        var hasOverlap = await _context.Appointments.AnyAsync(a => 
            a.CreatedByUserId == userId && 
            a.Status != AppointmentStatus.Cancelled &&
            a.StartAt < endAt && 
            a.EndAt > startAt);

        if (hasOverlap)
        {
            throw new Exception("Appointment overlaps with an existing one.");
        }

        // 3. Create
        var entity = _mapper.Map<Appointment>(dto);
        entity.EndAt = endAt;
        entity.CreatedByUserId = userId;
        entity.Status = AppointmentStatus.Scheduled;

        _context.Appointments.Add(entity);
        await _context.SaveChangesAsync(default);

        // Reload to include navigation properties if needed or just map
        // For simplicity returning mapped entity, might lack Client/Service names if not loaded
        // Let's load references
        await _context.Entry(entity).Reference(e => e.Client).LoadAsync();
        await _context.Entry(entity).Reference(e => e.Service).LoadAsync();

        return _mapper.Map<AppointmentDto>(entity);
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);
            
        return entity == null ? null : _mapper.Map<AppointmentDto>(entity);
    }

    public async Task<List<AppointmentDto>> GetAllAsync(DateTime? dateFrom, DateTime? dateTo, AppointmentStatus? status, int? clientId, int? serviceId)
    {
        var query = _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .AsQueryable();

        if (dateFrom.HasValue) query = query.Where(a => a.StartAt >= dateFrom.Value);
        if (dateTo.HasValue) query = query.Where(a => a.StartAt <= dateTo.Value); // or EndAt <= dateTo? usually StartAt
        if (status.HasValue) query = query.Where(a => a.Status == status.Value);
        if (clientId.HasValue) query = query.Where(a => a.ClientId == clientId.Value);
        if (serviceId.HasValue) query = query.Where(a => a.ServiceId == serviceId.Value);

        // Filter by user permissions? "User gère ses propres rendez-vous".
        // If User, filter by CreatedByUserId. If Admin, allow all?
        // Logic handled here:
        if (_currentUser.Role != "Admin")
        {
             query = query.Where(a => a.CreatedByUserId == _currentUser.UserId);
        }

        var list = await query.OrderBy(a => a.StartAt).ToListAsync();
        return _mapper.Map<List<AppointmentDto>>(list);
    }

    public async Task UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var entity = await _context.Appointments.FindAsync(id);
        if (entity == null) return;
        
        // Authorization check
        if (_currentUser.Role != "Admin" && entity.CreatedByUserId != _currentUser.UserId)
            throw new UnauthorizedAccessException();

        if (dto.StartAt != default)
        {
            // Re-calculate EndAt if start changes
            // Need service duration
            var service = await _context.Services.FindAsync(entity.ServiceId);
            if (service != null)
            {
                 var endAt = dto.StartAt.AddMinutes(service.DurationMinutes);
                 
                 // Check overlap excluding itself
                 var hasOverlap = await _context.Appointments.AnyAsync(a => 
                    a.Id != id &&
                    a.CreatedByUserId == entity.CreatedByUserId && 
                    a.Status != AppointmentStatus.Cancelled &&
                    a.StartAt < endAt && 
                    a.EndAt > dto.StartAt);

                 if (hasOverlap) throw new Exception("Appointment overlaps.");

                 entity.StartAt = dto.StartAt;
                 entity.EndAt = endAt;
            }
        }
        
        if (dto.Notes != null) entity.Notes = dto.Notes;
        if (dto.Status.HasValue) entity.Status = dto.Status.Value;

        await _context.SaveChangesAsync(default);
    }

    public async Task PerformActionAsync(int id, string action)
    {
        // simplistic state machine
        var entity = await _context.Appointments.FindAsync(id);
        if (entity == null) return;

        if (action.ToLower() == "cancel")
        {
            entity.Status = AppointmentStatus.Cancelled;
        }
        // other actions
        await _context.SaveChangesAsync(default);
    }

    public async Task CancelAsync(int id)
    {
        await PerformActionAsync(id, "cancel");
    }

    public async Task<List<AppointmentDto>> GetAvailabilityAsync(DateTime date, int userId)
    {
        // Return all non-cancelled appointments for that day
        // Start of day, end of day
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var list = await _context.Appointments
            .Where(a => a.CreatedByUserId == userId &&
                        a.Status != AppointmentStatus.Cancelled &&
                        a.StartAt < endOfDay && 
                        a.EndAt > startOfDay) 
            .OrderBy(a => a.StartAt)
            .ToListAsync();
            
        // Map to DTO (Simpler than custom slot object)
        return _mapper.Map<List<AppointmentDto>>(list);
    }
}
