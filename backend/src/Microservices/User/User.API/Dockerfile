# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8086
EXPOSE 8087

#Copy auth server cert and trust it
USER root
COPY certificates/localhost.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates
USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Microservices/User/User.API/User.API.csproj", "src/Microservices/User/User.API/"]
COPY ["src/Microservices/User/User.Application/User.Application.csproj", "src/Microservices/User/User.Application/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Microservices/User/User.Shared/User.Shared.csproj", "src/Microservices/User/User.Shared/"]
RUN dotnet restore "./src/Microservices/User/User.API/User.API.csproj"
COPY . .
WORKDIR "/src/src/Microservices/User/User.API"
RUN dotnet build "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.API.dll"]
