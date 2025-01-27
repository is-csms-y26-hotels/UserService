using Itmo.Dev.Platform.Events;
using UserService.Application.Abstractions.Persistence.Repositories;
using UserService.Application.Contracts;
using UserService.Application.Contracts.Events;
using UserService.Application.Contracts.Operations;
using UserService.Application.Models.Users;

namespace UserService.Application.Users;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    private readonly IEventPublisher _eventPublisher;

    public UsersService(IUsersRepository usersRepository, IEventPublisher eventPublisher)
    {
        _usersRepository = usersRepository;

        _eventPublisher = eventPublisher;
    }

    public async Task<long> CreateAsync(UserRequests.CreateUserRequest createUserRequest, CancellationToken cancellationToken)
    {
        var user = new User(
            createUserRequest.UserId,
            createUserRequest.FirstName,
            createUserRequest.LastName,
            createUserRequest.Email,
            createUserRequest.Password,
            createUserRequest.Birthdate,
            createUserRequest.Sex,
            createUserRequest.Tel,
            createUserRequest.CreatedAt);

        // TODO. Transaction over DB operation and Kafka. Use inbox/outbox?
        long userId = await _usersRepository.CreateUserAsync(user, cancellationToken);

        var evt = new UserRegistrationEvent(
            userId,
            createUserRequest.FirstName,
            createUserRequest.LastName,
            createUserRequest.Email,
            createUserRequest.Password,
            createUserRequest.Birthdate,
            createUserRequest.Sex,
            createUserRequest.Tel,
            createUserRequest.CreatedAt);

        await _eventPublisher.PublishAsync(evt, cancellationToken);

        // TODO. Return createdUser instead of userId?
        return userId;
    }

    public async Task<UserWithoutConfidentialFields> GetUserWithoutConfidentialFieldsAsync(
        UserRequests.GetUserRequest request,
        CancellationToken cancellationToken)
    {
        UserWithoutConfidentialFields user = await _usersRepository.GetUserWithoutConfidentialFieldsByIdAsync(request.UserId, cancellationToken);

        return user;
    }
}