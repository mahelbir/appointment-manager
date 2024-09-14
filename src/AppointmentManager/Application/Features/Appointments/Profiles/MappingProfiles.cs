using Application.Features.Appointments.Commands.Book;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.SlotList;
using Application.Features.Appointments.Queries.SlotListAdmin;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Appointments.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        
        CreateMap<Appointment, BookAppointmentCommand>().ReverseMap();
        CreateMap<Client, BookAppointmentCommand.ClientDto>().ReverseMap();
        CreateMap<Appointment, BookedAppointmentResponse>().ReverseMap();
        CreateMap<Client, BookedAppointmentResponse.ClientDto>().ReverseMap();
        
        CreateMap<Client, GetByIdAppointmentResponse.ClientDto>().ReverseMap();
        CreateMap<Appointment, GetByIdAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, SlotListAppointmentItemDto>().ReverseMap();
        
        CreateMap<Client, SlotListAdminAppointmentItemDto.ClientDto>().ReverseMap();
        CreateMap<Appointment, SlotListAdminAppointmentItemDto>().ReverseMap();
    }
}