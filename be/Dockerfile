FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CinemaMS.API/CinemaMS.API.csproj", "CinemaMS.API/"]
COPY ["CinemaMS.Application/CinemaMS.Application.csproj", "CinemaMS.Application/"]
COPY ["CinemaMS.Domain/CinemaMS.Domain.csproj", "CinemaMS.Domain/"]
COPY ["CinemaMS.Infrastructure/CinemaMS.Infrastructure.csproj", "CinemaMS.Infrastructure/"]
RUN dotnet restore "./CinemaMS.API/CinemaMS.API.csproj"
COPY . .
WORKDIR "/src/CinemaMS.API"
RUN dotnet build "./CinemaMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CinemaMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CinemaMS.API.dll"]
