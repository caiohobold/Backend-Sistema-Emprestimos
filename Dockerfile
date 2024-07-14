FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar dependências
COPY ["EmprestimosAPI.csproj", "."]
RUN dotnet restore "EmprestimosAPI.csproj"

# Copiar o restante do código
COPY . .
WORKDIR "/src"
RUN dotnet build "EmprestimosAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmprestimosAPI.csproj" -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .

# Adicionar permissões de execução ao script entrypoint.sh como root
USER root
RUN chmod +x entrypoint.sh
USER app

ENTRYPOINT ["./entrypoint.sh"]
