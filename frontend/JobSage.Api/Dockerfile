# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["frontend/JobSage.Api/JobSage.Api.csproj", "JobSage.Api/"]
COPY ["frontend/JobSage.Application/JobSage.Application.csproj", "JobSage.Application/"]
COPY ["frontend/JobSage.Domain/JobSage.Domain.csproj", "JobSage.Domain/"]
COPY ["frontend/JobSage.Infrastructure/JobSage.Infrastructure.csproj", "JobSage.Infrastructure/"]
COPY ["frontend/JobSage.SharedKernel/JobSage.SharedKernel.csproj", "JobSage.SharedKernel/"]
RUN dotnet restore "JobSage.Api/JobSage.Api.csproj"
COPY frontend/ .
WORKDIR "/src/JobSage.Api"
RUN dotnet build "JobSage.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JobSage.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobSage.Api.dll"]