using AppointmentManager.Domain.Enums;

namespace AppointmentManager.Application.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty; 
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public int CreatedByUserId { get; set; }
}

public class CreateAppointmentDto
{
    public int ClientId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAppointmentDto
{
    public DateTime StartAt { get; set; }
    public string? Notes { get; set; }
    public AppointmentStatus? Status { get; set; }
}
