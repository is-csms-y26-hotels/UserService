using Itmo.Dev.Platform.Events;
using System.Transactions;
using UserService.Application.Abstractions.Persistence.Repositories;
using UserService.Application.Contracts;
using UserService.Application.Contracts.Events;
using UserService.Application.Contracts.Operations;
using UserService.Application.Exceptions;
using UserService.Application.Models.Users;

namespace UserService.Application.Users;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    // private readonly IEventPublisher _eventPublisher;
    public UsersService(IUsersRepository usersRepository, IEventPublisher eventPublisher)
    {
        _usersRepository = usersRepository;

        // _eventPublisher = eventPublisher;
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

        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
        User? existingUser = await _usersRepository.GetUserByEmail(createUserRequest.Email, cancellationToken);
        if (existingUser is not null)
            throw new UserAlreadyExistsException($"User with email {createUserRequest.Email} already exists");

        long userId = await _usersRepository.CreateUserAsync(user, cancellationToken);

        transaction.Complete();

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

        // await _eventPublisher.PublishAsync(evt, cancellationToken);
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