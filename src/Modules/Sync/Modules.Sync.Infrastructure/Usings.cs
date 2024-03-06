global using System.Diagnostics;

global using System.Runtime.CompilerServices;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;

global using Microsoft.EntityFrameworkCore;

global using FluentValidation;

global using MassTransit;

global using Newtonsoft.Json;

global using Polly;
global using Polly.Retry;

global using Quartz;

global using Serilog;

global using Shared.Options.Extensions;

global using Shared.Results;
global using Shared.Results.Extensions;

global using Shared.Utils;

global using Application.Caching;

global using Application.DateTimeProvider;

global using Infrastructure.BackgroundJobs;

global using Infrastructure.Idempotence.Extensions;

global using Modules.Hub.Communication.GetAccountCredentials;

global using Modules.Sync.Domain;
global using Modules.Sync.Domain.ServiceAccounts;
global using Modules.Sync.Domain.ServiceAccountSyncStates;

global using Modules.Sync.Application.Abstractions;
global using Modules.Sync.Application.Emails;
global using Modules.Sync.Application.ServiceAccountCredentialsProvider;

global using Modules.Sync.Persistence;
