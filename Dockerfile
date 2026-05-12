FROM mcr.microsoft.com/dotnet/sdk:10.0.300@sha256:dc8430e6024d454edadad1e160e1973be3cabbb7125998ef190d9e5c6adf7dbb AS build
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

FROM mcr.microsoft.com/dotnet/runtime:10.0.7@sha256:8fb7ff015fcf0ebc6e90105bd6db06875954e6dc3d374b9dbb34c732867d13e4
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PinatBot.dll"]
