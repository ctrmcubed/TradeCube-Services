FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS base

WORKDIR /src
COPY ["TradeCube-Services/", "TradeCube-Services/"]
COPY ["Equias/", "Equias/"]
COPY ["Shared/", "Shared/"]

RUN dotnet restore "TradeCube-Services/TradeCube-Services.csproj"

WORKDIR "/src/TradeCube-Services"
RUN dotnet build "TradeCube-Services.csproj" -c Release -o /app
RUN dotnet publish "TradeCube-Services.csproj" -c Release -o /app

COPY ["TradeCube-Services/appsettings.*", "/app/"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=base /app ./

ENTRYPOINT ["dotnet", "TradeCube-Services.dll"]
