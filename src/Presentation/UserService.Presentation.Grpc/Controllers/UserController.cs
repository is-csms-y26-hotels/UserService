using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Contracts;
using UserService.Application.Contracts.TmpDtosMoveToGateway;
using UserService.Presentation.Grpc.Controllers.Utilities;

namespace UserService.Presentation.Grpc.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUsersService _userService;

    public UserController(IUsersService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Reqisters a new user.
    /// </summary>
    /// <returns>The created order.</returns>
    /// <response code="200">Returns the registered user.</response>
    /// <response code="400">If the input is invalid.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> RegisterUserAsync(
        [FromBody] UserDto userDto,
        CancellationToken cancellationToken)
    {
        // TODO. Retuern User instead of id?
        long registeredUserId = await _userService.CreateAsync(ProtoMapper.UserDtoToCreateUserRequest(userDto), cancellationToken);
        return Ok(registeredUserId);
    }
}
