FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Shop-Web7.0-WebApp/Shop-Web7.0-WebApp.csproj", "Shop-Web7.0-WebApp/"]
RUN dotnet restore "Shop-Web7.0-WebApp/Shop-Web7.0-WebApp.csproj"
COPY . .
WORKDIR "/src/Shop-Web7.0-WebApp"
RUN dotnet build "Shop-Web7.0-WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shop-Web7.0-WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop-Web7.0-WebApp.dll"]
