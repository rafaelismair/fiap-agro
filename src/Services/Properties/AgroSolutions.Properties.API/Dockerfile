FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/BuildingBlocks/AgroSolutions.Common/AgroSolutions.Common.csproj", "src/BuildingBlocks/AgroSolutions.Common/"]
COPY ["src/Services/Properties/AgroSolutions.Properties.Domain/AgroSolutions.Properties.Domain.csproj", "src/Services/Properties/AgroSolutions.Properties.Domain/"]
COPY ["src/Services/Properties/AgroSolutions.Properties.Application/AgroSolutions.Properties.Application.csproj", "src/Services/Properties/AgroSolutions.Properties.Application/"]
COPY ["src/Services/Properties/AgroSolutions.Properties.Infrastructure/AgroSolutions.Properties.Infrastructure.csproj", "src/Services/Properties/AgroSolutions.Properties.Infrastructure/"]
COPY ["src/Services/Properties/AgroSolutions.Properties.API/AgroSolutions.Properties.API.csproj", "src/Services/Properties/AgroSolutions.Properties.API/"]

RUN dotnet restore "src/Services/Properties/AgroSolutions.Properties.API/AgroSolutions.Properties.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Properties/AgroSolutions.Properties.API"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AgroSolutions.Properties.API.dll"]
