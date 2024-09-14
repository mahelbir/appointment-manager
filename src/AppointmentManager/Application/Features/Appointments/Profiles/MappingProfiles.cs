using Application.Features.Appointments.Commands.Book;
using Application.Features.Appointments.Commands.Busy;
using Application.Features.Appointments.Commands.Cancel;
using Application.Features.Appointments.Commands.Confirm;
using Application.Features.Appointments.Commands.Update;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.GetList;
using Application.Features.Appointments.Queries.Calendar;
using Application.Features.Appointments.Queries.CalendarAdmin;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Appointments.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        
        CreateMap<Client, BookAppointmentCommand.BookAppointmentCommandClient>().ReverseMap();
        CreateMap<Appointment, BookAppointmentCommand>().ReverseMap();
        CreateMap<Client, BookedAppointmentResponse.BookedAppointmentResponseClient>().ReverseMap();
        CreateMap<Appointment, BookedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, BusyAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, BusyAppointmentResponse>().ReverseMap();
        
        CreateMap<Client, UpdateAppointmentCommand.UpdateAppointmentCommandClient>().ReverseMap();
        CreateMap<Appointment, UpdateAppointmentCommand>().ReverseMap();
        CreateMap<Client, UpdatedAppointmentResponse.UpdatedAppointmentResponseClient>().ReverseMap();
        CreateMap<Appointment, UpdatedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, CanceledAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, ConfirmedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, GetListAppointmentListItemDto>().ReverseMap();
        CreateMap<IPaginate<Appointment>, GetListResponse<GetListAppointmentListItemDto>>().ReverseMap();
        
        CreateMap<Client, GetByIdAppointmentResponse.ClientDto>().ReverseMap();
        CreateMap<Appointment, GetByIdAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, CalendarAppointmentItemDto>().ReverseMap();
        
        CreateMap<Client, CalendarAdminAppointmentItemDto.ClientDto>().ReverseMap();
        CreateMap<Appointment, CalendarAdminAppointmentItemDto>().ReverseMap();
    }
}