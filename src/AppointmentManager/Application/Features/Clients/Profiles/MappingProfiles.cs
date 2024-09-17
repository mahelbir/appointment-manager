using Application.Features.Clients.Queries.GetById;
using Application.Features.Clients.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Clients.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Client, GetByIdClientResponse>().ReverseMap();
        
        CreateMap<Client, GetListClientListItemDto>().ReverseMap();
        CreateMap<IPaginate<Client>, GetListResponse<GetListClientListItemDto>>().ReverseMap();
        
    }
}