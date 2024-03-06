global using FluentValidation;

global using MediatR;

global using Shared.Results;
global using Shared.Results.Errors;
global using Shared.Results.Extensions;
global using Shared.Results.FluentValidation;

global using Application.EventBus;

global using Application.Messaging;

global using Modules.Hub.IntegrationEvents;

global using Modules.Hub.Domain;
global using Modules.Hub.Domain.Users;
global using Modules.Hub.Domain.ServiceAccounts;
global using Modules.Hub.Domain.ServiceAccounts.DomainEvents;

global using Modules.Hub.Application.Abstractions;
global using Modules.Hub.Application.TokenService;
