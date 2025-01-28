using UserService.Application.Models.Users;

namespace UserService.Application.Abstractions.Persistence.Repositories;

public interface IUsersRepository
{
    public Task<long> CreateUserAsync(User user, CancellationToken cancellationToken);

    public Task<UserWithoutConfidentialFields> GetUserWithoutConfidentialFieldsByIdAsync(long userId, CancellationToken cancellationToken);

    public Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
}