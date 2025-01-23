FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AdLerBackend.API/AdLerBackend.API.csproj", "AdLerBackend.API/"]
COPY ["AdLerBackend.Application/AdLerBackend.Application.csproj", "AdLerBackend.Application/"]
COPY ["AdLerBackend.Domain/AdLerBackend.Domain.csproj", "AdLerBackend.Domain/"]
COPY ["AdLerBackend.Infrastructure/AdLerBackend.Infrastructure.csproj", "AdLerBackend.Infrastructure/"]

RUN dotnet restore "AdLerBackend.API/AdLerBackend.API.csproj"
COPY . .
WORKDIR "/src/AdLerBackend.API"
RUN dotnet publish "AdLerBackend.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install curl and clean up
RUN apt-get update && apt-get install -y curl && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

HEALTHCHECK --start-period=30s CMD curl -f http://localhost/api/health || exit 1

COPY --from=build /app/publish .
CMD ["dotnet", "AdLerBackend.API.dll"]
