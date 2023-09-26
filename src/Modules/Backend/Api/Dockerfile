#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.306 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Modules/Backend/Api/Backend.Api.csproj", "src/Modules/Backend/Api/"]
COPY ["src/BuildingBlocks/Application/BuildingBlocks.Application.csproj", "src/BuildingBlocks/Application/"]
COPY ["src/BuildingBlocks/Infrastructure/BuildingBlocks.Infrastructure.csproj", "src/BuildingBlocks/Infrastructure/"]
COPY ["src/BuildingBlocks/Domain/BuildingBlocks.Domain.csproj", "src/BuildingBlocks/Domain/"]
COPY ["src/Modules/Backend/Application/Backend.Application.csproj", "src/Modules/Backend/Application/"]
COPY ["src/Modules/Backend/Infrastructure/Backend.Infrastructure.csproj", "src/Modules/Backend/Infrastructure/"]
COPY ["src/Modules/Backend/Domain/Backend.Domain.csproj", "src/Modules/Backend/Domain/"]
RUN dotnet restore "src/Modules/Backend/Api/Backend.Api.csproj"
COPY . .
WORKDIR "/src/src/Modules/Backend/Api"
RUN dotnet build "Backend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.dll"]