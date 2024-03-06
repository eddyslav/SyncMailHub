FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/Common/Application/Application.csproj", "src/Common/Application/"]
COPY ["src/Common/Domain/Domain.csproj", "src/Common/Domain/"]
COPY ["src/Common/Infrastructure/Infrastructure.csproj", "src/Common/Infrastructure/"]
COPY ["src/Common/Persistence/Persistence.csproj", "src/Common/Persistence/"]

COPY ["src/Common/Shared.Options/Shared.Options.csproj", "src/Common/Shared.Options/"]
COPY ["src/Common/Shared.Results/Shared.Results.csproj", "src/Common/Shared.Results/"]
COPY ["src/Common/Shared.Results.FluentValidation/Shared.Results.FluentValidation.csproj", "src/Common/Shared.Results.FluentValidation/"]
COPY ["src/Common/Shared.Utils/Shared.Utils.csproj", "src/Common/Shared.Utils/"]

COPY ["src/Modules/Hub/Modules.Hub.Application/Modules.Hub.Application.csproj", "src/Modules/Hub/Modules.Hub.Application/"]
COPY ["src/Modules/Hub/Modules.Hub.Communication/Modules.Hub.Communication.csproj", "src/Modules/Hub/Modules.Hub.Communication/"]
COPY ["src/Modules/Hub/Modules.Hub.Domain/Modules.Hub.Domain.csproj", "src/Modules/Hub/Modules.Hub.Domain/"]
COPY ["src/Modules/Hub/Modules.Hub.Infrastucture/Modules.Hub.Infrastucture.csproj", "src/Modules/Hub/Modules.Hub.Infrastucture/"]
COPY ["src/Modules/Hub/Modules.Hub.IntegrationEvents/Modules.Hub.IntegrationEvents.csproj", "src/Modules/Hub/Modules.Hub.IntegrationEvents/"]
COPY ["src/Modules/Hub/Modules.Hub.Persistence/Modules.Hub.Persistence.csproj", "src/Modules/Hub/Modules.Hub.Persistence/"]
COPY ["src/Modules/Hub/Modules.Hub.Presentation/Modules.Hub.Presentation.csproj", "src/Modules/Hub/Modules.Hub.Presentation/"]

COPY ["src/Modules/Notifications/Modules.Notifications.Infrastructure/Modules.Notifications.Infrastructure.csproj", "src/Modules/Notifications/Modules.Notifications.Infrastructure/"]
COPY ["src/Modules/Notifications/Modules.Notifications.Application/Modules.Notifications.Application.csproj", "src/Modules/Notifications/Modules.Notifications.Application/"]
COPY ["src/Modules/Notifications/Modules.Notifications.Persistence/Modules.Notifications.Persistence.csproj", "src/Modules/Notifications/Modules.Notifications.Persistence/"]

COPY ["src/Modules/Sync/Modules.Sync.Application/Modules.Sync.Application.csproj", "src/Modules/Sync/Modules.Sync.Application/"]
COPY ["src/Modules/Sync/Modules.Sync.Domain/Modules.Sync.Domain.csproj", "src/Modules/Sync/Modules.Sync.Domain/"]
COPY ["src/Modules/Sync/Modules.Sync.Infrastructure/Modules.Sync.Infrastructure.csproj", "src/Modules/Sync/Modules.Sync.Infrastructure/"]
COPY ["src/Modules/Sync/Modules.Sync.Persistence/Modules.Sync.Persistence.csproj", "src/Modules/Sync/Modules.Sync.Persistence/"]

COPY ["src/App/App.csproj", "src/App/"]

RUN dotnet restore "./src/App/App.csproj"

COPY . .

WORKDIR "/src/src/App"

RUN dotnet build "./App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.dll"]
