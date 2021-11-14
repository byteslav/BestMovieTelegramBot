FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BestMovieTelegramBot.csproj", "./"]
RUN dotnet restore "BestMovieTelegramBot.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "BestMovieTelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BestMovieTelegramBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BestMovieTelegramBot.dll"]
