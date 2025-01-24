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

    public async Task<long> CreateAsync(CreateUser.Request request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Birthdate,
            request.Sex,
            request.Tel,
            request.CreatedAt);

        // TODO. Transaction over DB operation and Kafka. Use inbox/outbox?
        long userId = await _usersRepository.CreateUserAsync(user, cancellationToken);

        var evt = new UserRegistrationEvent(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Birthdate,
            request.Sex,
            request.Tel,
            request.CreatedAt);

        await _eventPublisher.PublishAsync(evt, cancellationToken);

        // TODO. Return createdUser instead of userId?
        return userId;
    }
}