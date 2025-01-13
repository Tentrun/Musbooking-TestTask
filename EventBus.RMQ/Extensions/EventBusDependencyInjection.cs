using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventBus.RMQ.Interfaces;
using EventBus.RMQ.JsonConverters;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RMQ.Extensions;

public static class EventBusDependencyInjection
{
    public static IServiceCollection AddRmqEventBus(this IServiceCollection services, IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? registrationConfigurator = null,
        Action<IRabbitMqBusFactoryConfigurator>? rmqBusConfigurator = null)
    {
        string? host = configuration["RMQ:Host"];
        string? username = configuration["RMQ:Username"];
        string? password = configuration["RMQ:Password"];

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Cannot configure RMQ host and/or username and/or password.");
        }
        
        services
            .AddHostedService<EventBusHostedService>()
            .AddSingleton<IEventBus, EventBus>()
            .AddMasstransitRabbitMq(host, username, password,
                registrationConfigurator, rmqBusConfigurator);

        return services;
    }

    private static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services, string host,
        string username, string password, Action<IBusRegistrationConfigurator>? configureService = null,
        Action<IRabbitMqBusFactoryConfigurator>? configureRabbitMqBus = null)
    {
        var rabbitUri = new Uri(host);

        services.AddMassTransit(config =>
        {
            config.AddDelayedMessageScheduler();
            config.SetKebabCaseEndpointNameFormatter();

            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseDelayedMessageScheduler();

                configureRabbitMqBus?.Invoke(cfg);

                cfg.Host(rabbitUri, hostCfg =>
                {
                    hostCfg.Username(username);
                    hostCfg.Password(password);
                });

                cfg.PurgeOnStartup = false;
                cfg.UseRawJsonSerializer();
                cfg.ConfigureJsonSerializerOptions(_ =>
                {
                    var jsonOptions = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    jsonOptions.Converters.Add(new DateTimeOffsetNullableConverter());
                    jsonOptions.Converters.Add(new IntBoolConverter());
                    jsonOptions.Converters.Add(new GuidAsStringConverter());
                    jsonOptions.Converters.Add(new DateOnlyConverter());
                    jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                    return jsonOptions;
                });

                cfg.ConfigureEndpoints(context);
            });

            configureService?.Invoke(config);
        });

        return services;
    }
}