using Microsoft.EntityFrameworkCore;
using AppointmentManager.Domain.Entities;

namespace AppointmentManager.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Client> Clients { get; }
    DbSet<Service> Services { get; }
    DbSet<Appointment> Appointments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
