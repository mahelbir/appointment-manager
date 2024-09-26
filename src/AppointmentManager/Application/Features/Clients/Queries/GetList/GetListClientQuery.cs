using Application.Extensions;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Clients.Queries.GetList;

public class GetListClientQuery: IRequest<GetListResponse<GetListClientListItemDto>>
{
    public PageRequest PageRequest { get; set; }
    
    public class GetListClientQueryHandler: IRequestHandler<GetListClientQuery, GetListResponse<GetListClientListItemDto>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetListClientQueryHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListClientListItemDto>> Handle(GetListClientQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Client> users = await _clientRepository.GetListAsync(
                index: request.PageRequest.PageIndexNormalized(),
                size: request.PageRequest.PageSizeNormalized(),
                enableTracking: false,
                cancellationToken: cancellationToken
            );

            var response = _mapper.Map<GetListResponse<GetListClientListItemDto>>(users);
            return response;
        }
    }
}