﻿namespace YashilBozor.API.Configurations;

public static partial class HostConfiguration
{
    public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder
            .AddLogger()
            .AddValidators()
            .AddMappers()
            .AddBusinessLogic()
            .AddExposers()
            .AddCors()
            .AddDevTools()
            .AddConfigurationApiUrlName();

        return new(builder);
    }

    public static async ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        //await app.MigrateDatabaseAsync();
        await app.UseSeedData();

        app
            .UseCors();
        app
            .UseCustomMiddleWare()
            .UseExposers()
            .UseDevTools();
        
        return app;
    }
}
