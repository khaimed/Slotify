using AppointmentManager.Application.DTOs;
using AppointmentManager.Domain.Enums;

namespace AppointmentManager.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
}

public interface IClientService
{
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<ClientDto?> GetByIdAsync(int id);
    Task<List<ClientDto>> GetAllAsync(int page = 1, int pageSize = 10, string? search = null);
    Task UpdateAsync(int id, UpdateClientDto dto);
    Task DeleteAsync(int id);
}

public interface IServiceService
{
    Task<ServiceDto> CreateAsync(CreateServiceDto dto);
    Task<ServiceDto?> GetByIdAsync(int id);
    Task<List<ServiceDto>> GetAllAsync();
    Task UpdateAsync(int id, UpdateServiceDto dto);
    Task ToggleActiveAsync(int id);
    Task DeleteAsync(int id);
}

public interface IAppointmentService
{
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task<List<AppointmentDto>> GetAllAsync(DateTime? dateFrom, DateTime? dateTo, AppointmentStatus? status, int? clientId, int? serviceId);
    Task UpdateAsync(int id, UpdateAppointmentDto dto);
    Task PerformActionAsync(int id, string action); // e.g. "cancel"
    Task CancelAsync(int id);
    Task<List<AppointmentDto>> GetAvailabilityAsync(DateTime date, int userId);
}
