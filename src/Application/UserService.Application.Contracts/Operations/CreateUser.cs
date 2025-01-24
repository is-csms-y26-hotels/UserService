using UserService.Application.Models;

namespace UserService.Application.Contracts.Operations;

public static class CreateUser
{
    public readonly record struct Request(
        long UserId,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        DateTime Birthdate,
        Sex Sex,
        DateTime CreatedAt,
        string? Tel);
}
