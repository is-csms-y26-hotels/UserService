using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Users.UsersService.Contracts;
using UserService.Application.Contracts;
using UserService.Application.Contracts.Operations;
using UserService.Application.Models.Users;
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
        var applicationRequest = new UserRequests.CreateUserRequest(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Birthdate.ToDateTime(),
            SexEnumMapper.ToModel(request.Sex),
            request.CreatedAt.ToDateTime(),
            request.Tel);

        long registeredUserId = await _userService.CreateAsync(applicationRequest, context.CancellationToken);

        var response = new CreateUserResponse
        {
            UserId = registeredUserId,
        };

        return response;
    }

    public override async Task<GetUserWithoutConfidentialInfoResponse> GetUserWithoutConfidentialInfo(
        GetUserWithoutConfidentialInfoRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new UserRequests.GetUserRequest(
            request.UserId);
        UserWithoutConfidentialFields userInfo = await _userService.GetUserWithoutConfidentialFieldsAsync(
            applicationRequest,
            context.CancellationToken);

        var response = new GetUserWithoutConfidentialInfoResponse
        {
            UserId = userInfo.UserId,
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Email = userInfo.Email,
            Birthdate = Timestamp.FromDateTime(userInfo.Birthdate.ToUniversalTime()),
            Sex = SexEnumMapper.ToProto(userInfo.Sex),
            Tel = userInfo.Tel,
            CreatedAt = Timestamp.FromDateTime(userInfo.CreatedAt.ToUniversalTime()),
        };

        return response;
    }
}
