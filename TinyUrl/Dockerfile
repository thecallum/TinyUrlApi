FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TinyUrl/TinyUrl.csproj", "TinyUrl/"]
RUN dotnet restore "TinyUrl/TinyUrl.csproj"
COPY . .
WORKDIR "/src/TinyUrl"
RUN dotnet build "./TinyUrl.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TinyUrl.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TinyUrl.dll"]

# Copy entrypoint script
#COPY entrypoint.sh /app/
#RUN chmod +x /app/entrypoint.sh
#
##EXPOSE 8080
#
#ENTRYPOINT ["/app/entrypoint.sh"]


#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
## Copy csproj and restore
#COPY src/*.csproj ./
#RUN dotnet restore
#
## Copy everything and build
#COPY src/ ./
#RUN dotnet publish -c Release -o /app/publish
#
## Runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0
#WORKDIR /app
#
## Install netcat for postgres wait (optional, for second script version)
#RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*
#
## Copy published app
#COPY --from=build /app/publish .
#
## Copy entrypoint script
#COPY entrypoint.sh /app/
#RUN chmod +x /app/entrypoint.sh
#
#EXPOSE 8080
#
#ENTRYPOINT ["/app/entrypoint.sh"]