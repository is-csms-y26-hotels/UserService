using Grpc.Core;
using Users.UsersService.Contracts;
using UserService.Application.Contracts;
using UserService.Application.Contracts.Operations;
using UserService.Presentation.Grpc.Controllers.Utilities;

namespace UserService.Presentation.Grpc.Controllers;

public class GrpcUserController : UsersService.UsersServiceBase
{
    private readonly IUsersService _userService;

    public GrpcUserController(IUsersService userService)
    {
        _userService = userService;
    }

    public override async Task<CreateUserResponse> Create(
        CreateUserRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new CreateUser.Request(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Birthdate.ToDateTime(),
            SexEnumMapper.ToModel(request.Sex),
            request.CreatedAt.ToDateTime(),
            request.Tel);

        // TODO. Retuern User instead of id?
        long registeredUserId = await _userService.CreateAsync(applicationRequest, context.CancellationToken);

        var response = new CreateUserResponse
        {
            UserId = registeredUserId,
        };

        return response;
    }
}
