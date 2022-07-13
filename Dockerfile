FROM mcr.microsoft.com/dotnet/runtime:6.0.4 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0.302 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR "/src/PinatBot"
RUN dotnet build "PinatBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PinatBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PinatBot.dll"]
