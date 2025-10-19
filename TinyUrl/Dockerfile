FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["TinyUrl/TinyUrl.csproj", "TinyUrl/"]
RUN dotnet restore "TinyUrl/TinyUrl.csproj"

COPY . .

WORKDIR "/app/TinyUrl"
RUN dotnet build "./TinyUrl.csproj" -c Release -o /app/build

COPY --chmod=755 entrypoint.sh /app/

EXPOSE 8080

ENTRYPOINT ["/app/entrypoint.sh"]