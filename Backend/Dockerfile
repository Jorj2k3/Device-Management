FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["DeviceManagement.Api/DeviceManagement.Api/DeviceManagement.Api.csproj", "DeviceManagement.Api/"]
RUN dotnet restore "DeviceManagement.Api/DeviceManagement.Api.csproj"

COPY . .
WORKDIR "/src/DeviceManagement.Api/DeviceManagement.Api"
RUN dotnet publish "DeviceManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DeviceManagement.Api.dll"]