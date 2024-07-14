FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar dependências
COPY ["EmprestimosAPI/EmprestimosAPI.csproj", "EmprestimosAPI/"]
RUN dotnet restore "EmprestimosAPI/EmprestimosAPI.csproj"

# Copiar o restante do código
COPY . .
WORKDIR "/src/EmprestimosAPI"
RUN dotnet build "EmprestimosAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmprestimosAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

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
