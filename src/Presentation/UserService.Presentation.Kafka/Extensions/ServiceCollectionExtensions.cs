using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Kafka.Contracts;

namespace UserService.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string producerKey = "Presentation:Kafka:Producers";

        collection.AddPlatformKafka(kafka => kafka
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddProducer(b => b
                .WithKey<UserRegistrationKey>()
                .WithValue<UserRegistrationValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:UserRegistration"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()));

        return collection;
    }
}