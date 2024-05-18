global using System.Diagnostics;

global using System.Security.Claims;

global using System.IdentityModel.Tokens.Jwt;

global using Microsoft.Extensions.Configuration;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;

global using Microsoft.Extensions.Options;

global using Microsoft.EntityFrameworkCore;

global using Microsoft.AspNetCore.Http;

global using FluentValidation;

global using MassTransit;

global using Serilog;
global using LogContext = Serilog.Context.LogContext;

global using Shared.Utils;

global using Shared.Results;
global using Shared.Results.Errors;
global using Shared.Results.Extensions;

global using Shared.Options.Extensions;

global using Domain.Common;

global using Application.Caching;
global using Application.Caching.Extensions;

global using Application.DateTimeProvider;

global using Application.Messaging;

global using Persistence.Outbox;

global using Infrastructure.BackgroundJobs;

global using Modules.Hub.Communication.RegisterServiceAccountStateChange;

global using Modules.Hub.Domain;
global using Modules.Hub.Domain.Users;
global using Modules.Hub.Domain.ServiceAccounts;

global using Modules.Hub.Application.Abstractions;
global using Modules.Hub.Application.Emails;
global using Modules.Hub.Application.ServiceAccountCredentialsProvider;
global using Modules.Hub.Application.TokenService;

global using Modules.Hub.Persistence;

global using Modules.Hub.Infrastucture.Communication.RegisterServiceAccountStateChange;
