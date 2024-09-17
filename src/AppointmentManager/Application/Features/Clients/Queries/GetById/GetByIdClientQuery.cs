using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Queries.GetById;

public class GetByIdClientQuery: IRequest<GetByIdClientResponse>
{
    public int Id { get; set; }
    
    public class GetByIdClientQueryHandler : IRequestHandler<GetByIdClientQuery, GetByIdClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetByIdClientQueryHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdClientResponse> Handle(GetByIdClientQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetAsync(
                predicate: c => c.Id == request.Id,
                cancellationToken: cancellationToken
            );
            return _mapper.Map<GetByIdClientResponse>(client);
        }
    }
}