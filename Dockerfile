FROM mcr.microsoft.com/dotnet/sdk:8.0.403@sha256:cab0284cce7bc26d41055d0ac5859a69a8b75d9a201cd226999f4f00cc983f13 AS build
WORKDIR /src

COPY ["*.sln", "."]
COPY ["*.props", "."]
COPY ["PinatBot/PinatBot.csproj", "PinatBot/"]
COPY ["PinatBot.Caching/PinatBot.Caching.csproj", "PinatBot.Caching/"]
COPY ["PinatBot.Data/PinatBot.Data.csproj", "PinatBot.Data/"]
RUN dotnet restore

COPY ["PinatBot/", "PinatBot/"]
COPY ["PinatBot.Caching/", "PinatBot.Caching/"]
COPY ["PinatBot.Data/", "PinatBot.Data/"]
WORKDIR "/src/PinatBot"
RUN dotnet build -c Release --no-restore

FROM build AS publish
RUN dotnet publish -c Release --no-build -o /app

FROM mcr.microsoft.com/dotnet/runtime:8.0.10@sha256:a335dccd3231f7f9e2122691b21c634f96e187d3840c8b7dbad61ee09500e408
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PinatBot.dll"]
