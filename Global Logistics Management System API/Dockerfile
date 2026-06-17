FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy BOTH projects to satisfy cross-project references
COPY ["Global Logistics Management System API/Global Logistics Management System API.csproj", "Global Logistics Management System API/"]
COPY ["Global Logistics Management System/Global Logistics Management System.csproj", "Global Logistics Management System/"]
RUN dotnet restore "Global Logistics Management System API/Global Logistics Management System API.csproj"

# Copy all files and compile from the subfolder context
COPY . .
WORKDIR "/src/Global Logistics Management System API"
RUN dotnet build "Global Logistics Management System API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Global Logistics Management System API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Global Logistics Management System API.dll"]