using Itmo.Dev.Platform.Events;
using UserService.Application.Models.Users;

namespace UserService.Application.Contracts.Events;

public record UserRegistrationEvent(
    long UserId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    DateTime Birthdate,
    Sex Sex,
    string? Tel,
    DateTime CreatedAt) : IEvent;