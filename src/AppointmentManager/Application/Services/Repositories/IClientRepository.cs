using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IClientRepository:IAsyncRepository<Client, int>
{
    
}