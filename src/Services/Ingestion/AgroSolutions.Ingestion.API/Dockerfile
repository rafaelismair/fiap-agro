FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/BuildingBlocks/AgroSolutions.Common/AgroSolutions.Common.csproj", "src/BuildingBlocks/AgroSolutions.Common/"]
COPY ["src/BuildingBlocks/AgroSolutions.MessageBus/AgroSolutions.MessageBus.csproj", "src/BuildingBlocks/AgroSolutions.MessageBus/"]

COPY ["src/Services/Ingestion/AgroSolutions.Ingestion.Domain/AgroSolutions.Ingestion.Domain.csproj", "src/Services/Ingestion/AgroSolutions.Ingestion.Domain/"]
COPY ["src/Services/Ingestion/AgroSolutions.Ingestion.Application/AgroSolutions.Ingestion.Application.csproj", "src/Services/Ingestion/AgroSolutions.Ingestion.Application/"]
COPY ["src/Services/Ingestion/AgroSolutions.Ingestion.Infrastructure/AgroSolutions.Ingestion.Infrastructure.csproj", "src/Services/Ingestion/AgroSolutions.Ingestion.Infrastructure/"]
COPY ["src/Services/Ingestion/AgroSolutions.Ingestion.API/AgroSolutions.Ingestion.API.csproj", "src/Services/Ingestion/AgroSolutions.Ingestion.API/"]

RUN dotnet restore "src/Services/Ingestion/AgroSolutions.Ingestion.API/AgroSolutions.Ingestion.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ingestion/AgroSolutions.Ingestion.API"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AgroSolutions.Ingestion.API.dll"]
