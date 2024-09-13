using Application.Features.Appointments.Commands.Create;
using Application.Features.Appointments.Commands.Delete;
using Application.Features.Appointments.Commands.Update;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.SlotList;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Appointments.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Appointment, CreateAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, CreatedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, UpdateAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, UpdatedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, DeleteAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, DeletedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, GetByIdAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, SlotListAppointmentItemDto>().ReverseMap();
    }
}