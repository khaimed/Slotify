using AutoMapper;
using AppointmentManager.Application.DTOs;
using AppointmentManager.Domain.Entities;

namespace AppointmentManager.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Service, ServiceDto>();
        CreateMap<CreateServiceDto, Service>();
        CreateMap<UpdateServiceDto, Service>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.ClientName, opt => opt.MapFrom(s => s.Client != null ? s.Client.FullName : string.Empty))
            .ForMember(d => d.ServiceName, opt => opt.MapFrom(s => s.Service != null ? s.Service.Name : string.Empty));
            
        CreateMap<CreateAppointmentDto, Appointment>();
        // Update handling manual or mapped carefully
    }
}
