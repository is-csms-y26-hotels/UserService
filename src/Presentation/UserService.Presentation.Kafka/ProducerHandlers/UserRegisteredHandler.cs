using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Producer;
using Users.Kafka.Contracts;
using UserService.Application.Contracts.Events;
using UserService.Presentation.Kafka.Utilities;

namespace UserService.Presentation.Kafka.ProducerHandlers;

public class UserRegisteredHandler : IEventHandler<UserRegistrationEvent>
{
    private readonly IKafkaMessageProducer<UserRegistrationKey, UserRegistrationValue> _producer;

    public UserRegisteredHandler(IKafkaMessageProducer<UserRegistrationKey, UserRegistrationValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(UserRegistrationEvent evt, CancellationToken cancellationToken)
    {
        var key = new UserRegistrationKey
        {
            UserId = evt.UserId,
        };

        var value = new UserRegistrationValue
        {
            FirstName = evt.FirstName,
            LastName = evt.LastName,
            Email = evt.Email,
            Password = evt.Password,
            Birthdate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(evt.Birthdate),
            Sex = SexEnumMapper.ToProto(evt.Sex),
            Tel = evt.Tel,
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(evt.CreatedAt),
        };

        IAsyncEnumerable<KafkaProducerMessage<UserRegistrationKey, UserRegistrationValue>> message =
            AsyncEnumerable.Repeat(
                new KafkaProducerMessage<UserRegistrationKey, UserRegistrationValue>(key, value),
                1);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}