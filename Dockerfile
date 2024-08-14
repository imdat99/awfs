#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Treasure/Treasure.csproj", "Treasure/"]
COPY ["Treasure.Service/Treasure.Service.csproj", "Treasure.Service/"]
COPY ["Treasure.Data/Treasure.Data.csproj", "Treasure.Data/"]
RUN dotnet restore "./Treasure/Treasure.csproj"
COPY . .
WORKDIR "/src/Treasure"
RUN dotnet build "./Treasure.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Treasure.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Treasure.dll"]