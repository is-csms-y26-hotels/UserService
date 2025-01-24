using UserService.Application.Models;

namespace UserService.Presentation.Kafka.Utilities;

public static class SexEnumMapper
{
    public static Users.Kafka.Contracts.Sex ToProto(Sex sex)
    {
        return sex switch
        {
            Sex.Unspecified => Users.Kafka.Contracts.Sex.Unspecified,
            Sex.Male => Users.Kafka.Contracts.Sex.Male,
            Sex.Female => Users.Kafka.Contracts.Sex.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(sex), sex, null),
        };
    }

    public static Sex ToModel(Users.Kafka.Contracts.Sex sex)
    {
        return sex switch
        {
            Users.Kafka.Contracts.Sex.Unspecified => Sex.Unspecified,
            Users.Kafka.Contracts.Sex.Male => Sex.Male,
            Users.Kafka.Contracts.Sex.Female => Sex.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(sex), sex, null),
        };
    }
}
