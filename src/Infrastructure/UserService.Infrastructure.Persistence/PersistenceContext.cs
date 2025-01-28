using UserService.Application.Abstractions.Persistence;
using UserService.Application.Abstractions.Persistence.Repositories;

namespace UserService.Infrastructure.Persistence;

public class PersistenceContext : IPersistenceContext
{
    public IUsersRepository Users { get; }

    public PersistenceContext(IUsersRepository users)
    {
        Users = users;
    }
}