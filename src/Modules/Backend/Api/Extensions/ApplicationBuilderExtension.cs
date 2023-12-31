﻿using Backend.Api.Middleware;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Extensions;

internal static class ApplicationBuilderExtension
{
    internal static IApplicationBuilder ApplyMigrations(this IApplicationBuilder builder)
    {
        using (IServiceScope scope = builder.ApplicationServices.CreateScope())
        {
            BackendApplicationDbContext context = scope.ServiceProvider.GetRequiredService<BackendApplicationDbContext>();
            //context.Database.Migrate();
        }

        return builder;
    }

    internal static IApplicationBuilder UseCustomExpcetionHandling(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CustomExceptionHandlingMiddleware>();
        return builder;
    }
}
