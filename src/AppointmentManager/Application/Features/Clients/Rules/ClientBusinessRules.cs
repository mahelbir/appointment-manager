using Application.Features.Clients.Constants;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Features.Clients.Rules;

public class ClientBusinessRules: BaseBusinessRules
{
    public async Task ShouldBeExists(Client? client)
    {
        if (client == null)
            throw new BusinessException(ClientMessages.DontExists);
    }
}