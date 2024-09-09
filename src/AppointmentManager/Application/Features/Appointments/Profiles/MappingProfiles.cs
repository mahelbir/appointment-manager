using Application.Features.Appointments.Commands.Create;
using Application.Features.Appointments.Commands.Delete;
using Application.Features.Appointments.Commands.Update;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

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
        
        CreateMap<Appointment, GetListAppointmentListItemDto>().ReverseMap();
        CreateMap<Paginate<Appointment>, GetListResponse<GetListAppointmentListItemDto>>().ReverseMap();
        
    }
}