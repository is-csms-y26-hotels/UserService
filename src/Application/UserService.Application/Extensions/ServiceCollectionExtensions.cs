using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts;
using UserService.Application.Users;

namespace UserService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddSingleton<IUsersService, UsersService>();
        return collection;
    }
}