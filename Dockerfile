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

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar o dotnet-ef globalmente
RUN dotnet tool install --global dotnet-ef

# Adicionar .dotnet/tools ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Criar e configurar o entrypoint.sh diretamente no Dockerfile
RUN echo '#!/bin/bash' > entrypoint.sh
RUN echo 'set -e' >> entrypoint.sh
RUN echo '' >> entrypoint.sh
RUN echo '# Rodar migrations' >> entrypoint.sh
RUN echo 'dotnet ef database update' >> entrypoint.sh
RUN echo '' >> entrypoint.sh
RUN echo '# Iniciar a aplicação' >> entrypoint.sh
RUN echo 'dotnet EmprestimosAPI.dll' >> entrypoint.sh
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
