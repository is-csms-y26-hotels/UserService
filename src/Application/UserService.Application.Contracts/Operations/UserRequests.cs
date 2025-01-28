using UserService.Application.Models.Users;

namespace UserService.Application.Contracts.Operations;

public static class UserRequests
{
    public readonly record struct CreateUserRequest(
        long? UserId,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        DateTime Birthdate,
        Sex Sex,
        DateTime CreatedAt,
        string? Tel);

    public readonly record struct GetUserRequest(
        long UserId);
}
