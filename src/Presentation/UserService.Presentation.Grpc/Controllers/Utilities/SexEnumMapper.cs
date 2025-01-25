using UserService.Application.Models.Users;

namespace UserService.Presentation.Grpc.Controllers.Utilities;

public static class SexEnumMapper
{
    public static Users.UsersService.Contracts.Sex ToProto(Sex sex)
    {
        return sex switch
        {
            Sex.Unspecified => Users.UsersService.Contracts.Sex.Unspecified,
            Sex.Male => Users.UsersService.Contracts.Sex.Male,
            Sex.Female => Users.UsersService.Contracts.Sex.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(sex), sex, null),
        };
    }

    public static Sex ToModel(Users.UsersService.Contracts.Sex sex)
    {
        return sex switch
        {
            Users.UsersService.Contracts.Sex.Unspecified => Sex.Unspecified,
            Users.UsersService.Contracts.Sex.Male => Sex.Male,
            Users.UsersService.Contracts.Sex.Female => Sex.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(sex), sex, null),
        };
    }
}
