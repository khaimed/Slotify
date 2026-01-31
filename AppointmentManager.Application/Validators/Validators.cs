using FluentValidation;
using AppointmentManager.Application.DTOs;

namespace AppointmentManager.Application.Validators;

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
}

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(x => x.ClientId).GreaterThan(0);
        RuleFor(x => x.ServiceId).GreaterThan(0);
        RuleFor(x => x.StartAt).GreaterThan(DateTime.MinValue);
    }
}
