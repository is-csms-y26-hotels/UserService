namespace UserService.Application.Models.Users;

public record UserWithoutConfidentialFields
(
    long UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTime Birthdate,
    Sex Sex,
    string? Tel,
    DateTime CreatedAt);