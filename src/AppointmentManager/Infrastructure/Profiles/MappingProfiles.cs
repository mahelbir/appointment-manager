using AutoMapper;
using Domain.Models;

namespace Infrastructure.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Google.Apis.Calendar.v3.Data.Channel, GoogleCalendarChannel>().ReverseMap();

        CreateMap<Google.Apis.Calendar.v3.Data.Event, CalendarEvent>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.ColorId))
            .ForMember(dest => dest.StartDate,
                opt => opt.MapFrom(src => src.Start.DateTimeDateTimeOffset.Value.UtcDateTime))
            .ForMember(dest => dest.EndDate,
                opt => opt.MapFrom(src => src.End.DateTimeDateTimeOffset.Value.UtcDateTime))
            .ReverseMap()
            .ForPath(src => src.Summary, opt => opt.MapFrom(dest => dest.Title))
            .ForPath(src => src.ColorId, opt => opt.MapFrom(dest => dest.Color))
            .ForPath(src => src.Start.DateTimeDateTimeOffset.Value.UtcDateTime,
                opt => opt.MapFrom(dest => dest.StartDate))
            .ForPath(src => src.End.DateTimeDateTimeOffset.Value.UtcDateTime, opt => opt.MapFrom(dest => dest.EndDate));
    }
}