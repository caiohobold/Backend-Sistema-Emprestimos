FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar depend�ncias
COPY ["EmprestimosAPI.csproj", "."]
RUN dotnet restore "EmprestimosAPI.csproj"

# Copiar o restante do c�digo
COPY . .
WORKDIR "/src"
RUN dotnet build "EmprestimosAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmprestimosAPI.csproj" -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .

# Adicionar permiss�es de execu��o ao script entrypoint.sh como root
USER root
RUN chmod +x entrypoint.sh
USER app

ENTRYPOINT ["./entrypoint.sh"]
