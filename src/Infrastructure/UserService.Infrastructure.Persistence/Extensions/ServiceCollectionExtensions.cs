using Itmo.Dev.Platform.Persistence.Abstractions.Extensions;
using Itmo.Dev.Platform.Persistence.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Abstractions.Persistence;
using UserService.Application.Abstractions.Persistence.Repositories;
using UserService.Infrastructure.Persistence.Plugins;
using UserService.Infrastructure.Persistence.Repositories;

namespace UserService.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddPlatformPersistence(persistence => persistence
            .UsePostgres(postgres => postgres
                .WithConnectionOptions(b => b.BindConfiguration("Infrastructure:Persistence:Postgres"))
                .WithMigrationsFrom(typeof(IAssemblyMarker).Assembly)
                .WithDataSourcePlugin<MappingPlugin>()));

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddScoped<IUsersRepository, UsersRepository>();

        return collection;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection collection)
    {
        return collection.AddHostedService<MigrationsBackgroundService>();
    }
}