using MassTransit;

using Newtonsoft.Json;

using Modules.Hub.Infrastucture;

using Modules.Notifications.Infrastructure;

using Modules.Sync.Infrastructure;

namespace App.Extensions;

internal static class EventBusServiceCollectionExtensions
{
	public static IServiceCollection AddEventBus(this IServiceCollection services) =>
		services.AddMassTransit(busConfiguration =>
		{
			busConfiguration.SetKebabCaseEndpointNameFormatter();

			busConfiguration.AddHubConsumers();
			busConfiguration.AddNotificationsConsumers();
			busConfiguration.AddSyncConsumers();

			busConfiguration.UsingInMemory((ctx, configuration) =>
			{
				configuration.UseNewtonsoftJsonSerializer();
				configuration.UseNewtonsoftJsonDeserializer();

				configuration.ConfigureNewtonsoftJsonSerializer(jsonOptions =>
				{
					jsonOptions.TypeNameHandling = TypeNameHandling.All;
					return jsonOptions;
				});

				configuration.ConfigureNewtonsoftJsonDeserializer(jsonOptions =>
				{
					jsonOptions.TypeNameHandling = TypeNameHandling.All;
					return jsonOptions;
				});

				configuration.ConfigureEndpoints(ctx);
			});
		});
}
