using AppointmentManager.Api.Services;
using AppointmentManager.Application;
using AppointmentManager.Application.Common.Interfaces;
using AppointmentManager.Infrastructure;
using AppointmentManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppointmentManager API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection(); // optional locally
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Migrate and Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        
        // Seed
        if (!context.Users.Any())
        {
            // Create Admin
            var admin = new AppointmentManager.Domain.Entities.User
            {
                FullName = "Admin User",
                Email = "admin@example.com",
                Role = AppointmentManager.Domain.Enums.Role.Admin,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
            };
            context.Users.Add(admin);
            
            // Create Services
            context.Services.AddRange(
                new AppointmentManager.Domain.Entities.Service { Name = "Consultation", DurationMinutes = 30, Price = 50 },
                new AppointmentManager.Domain.Entities.Service { Name = "Therapy", DurationMinutes = 60, Price = 100 }
            );
            
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating or seeding the database.");
    }
}

app.Run();
