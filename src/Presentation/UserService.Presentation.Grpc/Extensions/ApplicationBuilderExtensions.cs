using Microsoft.AspNetCore.Builder;
using UserService.Presentation.Grpc.Controllers;

namespace UserService.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePresentationGrpc(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(routeBuilder =>
        {
            routeBuilder.MapGrpcService<UserController>();
            routeBuilder.MapGrpcReflectionService();
        });

        return builder;
    }
}