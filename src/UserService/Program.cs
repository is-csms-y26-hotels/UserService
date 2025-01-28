#pragma warning disable CA1506

using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Observability;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserService.Application.Extensions;
using UserService.Infrastructure.Persistence.Extensions;
using UserService.Presentation.Grpc.Extensions;
using UserService.Presentation.Kafka.Extensions;

namespace UserService;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();

        builder.Services.AddOptions<JsonSerializerSettings>();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

        builder.Services.AddPlatform();
        builder.AddPlatformObservability();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructurePersistence();
        builder.Services.AddHostedServices();
        builder.Services.AddPresentationGrpc();
        builder.Services.AddPresentationKafka(builder.Configuration);

        builder.Services.AddPlatformEvents(b => b.AddPresentationKafkaHandlers());

        builder.Services.AddUtcDateTimeProvider();

        WebApplication app = builder.Build();

        app.UseRouting();

        app.UsePlatformObservability();

        app.UsePresentationGrpc();

        app.Run();
    }
}