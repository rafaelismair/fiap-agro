FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/BuildingBlocks/AgroSolutions.Common/AgroSolutions.Common.csproj", "src/BuildingBlocks/AgroSolutions.Common/"]
COPY ["src/Services/Identity/AgroSolutions.Identity.Domain/AgroSolutions.Identity.Domain.csproj", "src/Services/Identity/AgroSolutions.Identity.Domain/"]
COPY ["src/Services/Identity/AgroSolutions.Identity.Application/AgroSolutions.Identity.Application.csproj", "src/Services/Identity/AgroSolutions.Identity.Application/"]
COPY ["src/Services/Identity/AgroSolutions.Identity.Infrastructure/AgroSolutions.Identity.Infrastructure.csproj", "src/Services/Identity/AgroSolutions.Identity.Infrastructure/"]
COPY ["src/Services/Identity/AgroSolutions.Identity.API/AgroSolutions.Identity.API.csproj", "src/Services/Identity/AgroSolutions.Identity.API/"]

RUN dotnet restore "src/Services/Identity/AgroSolutions.Identity.API/AgroSolutions.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/AgroSolutions.Identity.API"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AgroSolutions.Identity.API.dll"]
