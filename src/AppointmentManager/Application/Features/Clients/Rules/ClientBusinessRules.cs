using Application.Features.Clients.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Features.Clients.Rules;

public class ClientBusinessRules : BaseBusinessRules
{
    private readonly IClientRepository _clientRepository;

    public ClientBusinessRules(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client> ShouldBeExistId(int id)
    {
        var client = await _clientRepository.GetAsync(
            predicate: c => c.Id == id
        );
        if (client == null)
            throw new BusinessException(ClientMessages.DontExists);
        return client;
    }
    
}