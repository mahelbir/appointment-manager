using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ClientRepository: EfRepositoryBase<Client, int, BaseDbContext>, IClientRepository
{
    public ClientRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }
}