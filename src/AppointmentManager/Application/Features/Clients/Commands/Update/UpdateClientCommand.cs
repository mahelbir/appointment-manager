using Application.Features.Clients.Rules;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Clients.Commands.Update;

public class UpdateClientCommand : IRequest<UpdatedClientResponse>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, UpdatedClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ClientBusinessRules _clientBusinessRules;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IClientRepository clientRepository, ClientBusinessRules clientBusinessRules,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _clientBusinessRules = clientBusinessRules;
            _mapper = mapper;
        }

        public async Task<UpdatedClientResponse> Handle(UpdateClientCommand request,
            CancellationToken cancellationToken)
        {
            var client = await _clientBusinessRules.ShouldBeExistId(request.Id);

            client = _mapper.Map(request, client);

            await _clientRepository.UpdateAsync(client, cancellationToken);

            var response = _mapper.Map<UpdatedClientResponse>(client);
            return response;
        }
    }
}