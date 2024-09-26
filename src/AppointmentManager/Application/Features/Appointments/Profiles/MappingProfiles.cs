using Application.Extensions;
using Application.Features.Appointments.Commands.Book;
using Application.Features.Appointments.Commands.Busy;
using Application.Features.Appointments.Commands.Cancel;
using Application.Features.Appointments.Commands.Confirm;
using Application.Features.Appointments.Commands.Update;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.GetList;
using Application.Features.Appointments.Queries.GetCalendar;
using Application.Features.Appointments.Queries.GetDetailedCalendar;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Appointments.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Appointment, CalendarAppointment>().ReverseMap();
        
        CreateMap<Client, BookAppointmentCommand.BookAppointmentCommandClient>().ReverseMap();
        CreateMap<Appointment, BookAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, BookedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, BusyAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, BusyAppointmentResponse>().ReverseMap();
        
        CreateMap<Client, UpdateAppointmentCommand.UpdateAppointmentCommandClient>().ReverseMap();
        CreateMap<Appointment, UpdateAppointmentCommand>().ReverseMap();
        CreateMap<Appointment, UpdatedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, CanceledAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, ConfirmedAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, GetListAppointmentListItemDto>().ReverseMap();
        CreateMap<IPaginate<Appointment>, GetListResponse<GetListAppointmentListItemDto>>().ReverseMap();
        
        CreateMap<Appointment, GetByIdAppointmentResponse>().ReverseMap();
        
        CreateMap<Appointment, GetCalendarAppointmentListItemDto>()
            .ForMember(dest => dest.Props, opt => opt.MapFrom(src => src.Status.GetProps()))
            .ReverseMap()
            .ForPath(src => src.Status.GetProps(), opt => opt.MapFrom(dest => dest.Props));
        
        CreateMap<Appointment, GetDetailedCalendarAppointmentListItemDto>().ReverseMap();
    }
}