using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace WebApi.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString(DateTime.UtcNow.ToString("yyyy-MM-dd")),
            });
        });

        return services;
    }
}