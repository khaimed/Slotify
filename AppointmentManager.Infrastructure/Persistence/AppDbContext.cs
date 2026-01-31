using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Domain.Entities;
using AppointmentManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppointmentManager.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Basic configurations directly here for simplicity
        builder.Entity<User>()
            .Property(e => e.Role)
            .HasConversion<string>();

        builder.Entity<Appointment>()
            .Property(e => e.Status)
            .HasConversion<string>();

        // Relationships and constraints
        builder.Entity<Appointment>()
            .HasOne(a => a.Client)
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
