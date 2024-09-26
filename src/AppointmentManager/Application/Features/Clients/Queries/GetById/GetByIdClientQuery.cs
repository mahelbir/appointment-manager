using Application.Features.Clients.Rules;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Queries.GetById;

public class GetByIdClientQuery : IRequest<GetByIdClientResponse>
{
    public int Id { get; set; }

    public class GetByIdClientQueryHandler : IRequestHandler<GetByIdClientQuery, GetByIdClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ClientBusinessRules _clientBusinessRules;
        private readonly IMapper _mapper;

        public GetByIdClientQueryHandler(IClientRepository clientRepository, ClientBusinessRules clientBusinessRules,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _clientBusinessRules = clientBusinessRules;
            _mapper = mapper;
        }

        public async Task<GetByIdClientResponse> Handle(GetByIdClientQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientBusinessRules.ShouldBeExistId(request.Id);

            var response = _mapper.Map<GetByIdClientResponse>(client);
            return response;
        }
    }
}