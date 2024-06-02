using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.Json;

using Carter;

using Serilog;

using Application.Caching;

using Infrastructure.Caching;

using Modules.Hub.Infrastucture;

using Modules.Notifications.Infrastructure;

using Modules.Sync.Infrastructure;

using App.Authentication;
using App.Extensions;
using App.Middlewares;
using App.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, config) =>
	config.ReadFrom.Configuration(ctx.Configuration));

var configuration = builder.Configuration;

// Add services to the container.
builder.Services
	.Configure<JsonOptions>(options =>
	{
		var jsonSerializerOptions = options.SerializerOptions;
		jsonSerializerOptions.Converters.Add(new EntityIdJsonConverter());
		jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
	})
	.AddAuthorization()
	.AddCors()
	.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetConnectionString("Redis"))
	.AddProblemDetails()
	.AddSession(options =>
	{
		options.IdleTimeout = TimeSpan.FromMinutes(1);
	})
	.AddCarter(new DependencyContextAssemblyCatalog([Modules.Hub.Presentation.AssemblyMarker.Assembly]))
	.AddJwtAuthentication(configuration)
	.AddBackgroundJobs()
	.AddEventBus()
	.AddHubModule(configuration)
	.AddNotificationsModule(configuration)
	.AddSyncModule(configuration)
	.AddTransient<CorrelationIdMiddleware>()
	.AddSingleton<ICachingService, CachingService>();

var app = builder.Build();

app.UseDatabaseInitializer();

app.MapCarter();

app.UseCors(builder =>
	builder.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod())
	.UseHttpsRedirection()
	.UseMiddleware<CorrelationIdMiddleware>()
	.UseSerilogRequestLogging()
	.UseAuthentication()
	.UseAuthorization()
	.UseSession();

app.Run();

// DO NOT CHANGE - required for integration tests
public partial class Program;
