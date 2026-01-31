namespace AppointmentManager.Application.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateClientDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    // Email is optional in some contexts but let's make it optional or required. User prompt says "Email" in Client entity.
    public string Email { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateClientDto : CreateClientDto
{
}
