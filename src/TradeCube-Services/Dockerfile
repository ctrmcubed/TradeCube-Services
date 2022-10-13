FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

WORKDIR /src
COPY ["Equias/", "Equias/"]
COPY ["Enegen/", "Enegen/"]
COPY ["Fidectus/", "Fidectus/"]
COPY ["Shared/", "Shared/"]
COPY ["TradeCube-Services/", "TradeCube-Services/"]

RUN dotnet restore "TradeCube-Services/TradeCube-Services.csproj"

WORKDIR "/src/TradeCube-Services"
RUN dotnet build "TradeCube-Services.csproj" -c Release -o /app
RUN dotnet publish "TradeCube-Services.csproj" -c Release -o /app

COPY ["TradeCube-Services/appsettings.*", "/app/"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=base /app ./

ENTRYPOINT ["dotnet", "TradeCube-Services.dll"]
