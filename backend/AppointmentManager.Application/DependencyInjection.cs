using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppointmentManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
