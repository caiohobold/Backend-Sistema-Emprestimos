FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmprestimosAPI/EmprestimosAPI.csproj", "EmprestimosAPI/"]
RUN dotnet restore "./EmprestimosAPI/EmprestimosAPI.csproj"
COPY . .
WORKDIR "/src/EmprestimosAPI"
RUN dotnet build "./EmprestimosAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmprestimosAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./entrypoint.sh"]