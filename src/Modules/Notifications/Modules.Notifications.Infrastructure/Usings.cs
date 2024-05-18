global using Microsoft.Extensions.Configuration;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;

global using Microsoft.Extensions.Options;

global using Microsoft.EntityFrameworkCore;

global using FluentValidation;

global using Shared.Options.Extensions;

global using Shared.Utils;

global using Application.EventBus;

global using Application.DateTimeProvider;

global using Persistence.Inbox;

global using Infrastructure.BackgroundJobs;

global using Infrastructure.Idempotence.Extensions;

global using Modules.Notifications.Persistence;

global using Modules.Notifications.Infrastructure.EmailSenderClient;
global using Modules.Notifications.Infrastructure.TemplateRenderService;
