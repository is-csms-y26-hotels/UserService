namespace UserService.Application.Models.Users;

public record User
(
    long UserId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    DateTime Birthdate,
    Sex Sex,
    string? Tel,
    DateTime CreatedAt);