FROM mcr.microsoft.com/dotnet/sdk:8.0.204@sha256:7861b15f318949cf9214d9ad5382b680a0e22e3b8673180707aa0c594ab75656 AS build
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

FROM mcr.microsoft.com/dotnet/runtime:8.0.3@sha256:c9804086710f1325e63cf2ed3f83deb10a3a5d07cc0e762b3bb27f0d8b7d6a7b
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PinatBot.dll"]
