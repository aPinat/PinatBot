FROM mcr.microsoft.com/dotnet/sdk:9.0.305@sha256:bb42ae2c058609d1746baf24fe6864ecab0686711dfca1f4b7a99e367ab17162 AS build
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

FROM mcr.microsoft.com/dotnet/runtime:9.0.9@sha256:6e6ccf450a40a99748e93bf18dd6739893881f5dd831ae23b586565749b1643d
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PinatBot.dll"]
