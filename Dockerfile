FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EmprestimosAPI/EmprestimosAPI.csproj", "EmprestimosAPI/"]
RUN dotnet restore "./EmprestimosAPI/EmprestimosAPI.csproj"
COPY . .
WORKDIR "/src/EmprestimosAPI"
RUN dotnet build "./EmprestimosAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./EmprestimosAPI.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .
ENTRYPOINT ["./entrypoint.sh"]