namespace AppointmentManager.Application.DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class CreateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateServiceDto : CreateServiceDto
{
}
