using UserService.Application.Models.Users;

namespace UserService.Application.Abstractions.Persistence.Repositories;

public interface IUsersRepository
{
    public Task<long> CreateUserAsync(User user, CancellationToken cancellationToken);
}