FROM mcr.microsoft.com/dotnet/sdk:9.0.201@sha256:712ffb3919c095da6129ba5ed9d0e7660ab45b966140819d1d58e081bef64293 AS build
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

FROM mcr.microsoft.com/dotnet/runtime:9.0.3@sha256:ee983d3c1ecf618b8d101025748b8325f269cff288589485b918cb27d4cb9811
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PinatBot.dll"]
