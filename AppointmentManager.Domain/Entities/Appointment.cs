using AppointmentManager.Domain.Enums;

namespace AppointmentManager.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? Notes { get; set; }
    
    public int CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
