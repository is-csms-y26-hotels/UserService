using UserService.Application.Contracts.Operations;

namespace UserService.Application.Contracts;

public interface IUsersService
{
    public Task<long> CreateAsync(CreateUser.Request request, CancellationToken cancellationToken);
}