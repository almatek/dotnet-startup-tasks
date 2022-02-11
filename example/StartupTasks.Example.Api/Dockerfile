FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Copy all files and restore package references
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./example ./example
COPY ./src ./src
RUN dotnet restore example/StartupTasks.Example.Api/StartupTasks.Example.Api.csproj

FROM build AS publish
RUN dotnet publish "example/StartupTasks.Example.Api/StartupTasks.Example.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StartupTasks.Example.Api.dll"]
