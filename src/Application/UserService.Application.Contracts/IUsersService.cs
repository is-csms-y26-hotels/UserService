using UserService.Application.Contracts.Operations;
using UserService.Application.Models.Users;

namespace UserService.Application.Contracts;

public interface IUsersService
{
    public Task<long> CreateAsync(UserRequests.CreateUserRequest createUserRequest, CancellationToken cancellationToken);

    public Task<UserWithoutConfidentialFields> GetUserWithoutConfidentialFieldsAsync(
        UserRequests.GetUserRequest request,
        CancellationToken cancellationToken);
}