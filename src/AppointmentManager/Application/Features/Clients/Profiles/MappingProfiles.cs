using Application.Features.Clients.Commands.Update;
using Application.Features.Clients.Queries.GetById;
using Application.Features.Clients.Queries.GetList;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using AutoMapper;

namespace Application.Features.Clients.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Client, UpdateClientCommand>().ReverseMap();
        CreateMap<Client, UpdatedClientResponse>().ReverseMap();
        
        CreateMap<Client, GetByIdClientResponse>().ReverseMap();
        
        CreateMap<Client, GetListClientListItemDto>().ReverseMap();
        CreateMap<IPaginate<Client>, GetListResponse<GetListClientListItemDto>>().ReverseMap();
    }
}