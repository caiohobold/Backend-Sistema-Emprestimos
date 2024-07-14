# Usar a imagem SDK para construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar e restaurar dependências
COPY ["EmprestimosAPI.csproj", "."]
RUN dotnet restore "EmprestimosAPI.csproj"

# Copiar o restante do código e construir
COPY . .
RUN dotnet build "EmprestimosAPI.csproj" -c Release -o /app/build

# Publicar a aplicação
RUN dotnet publish "EmprestimosAPI.csproj" -c Release -o /app/publish

# Usar a imagem SDK para a etapa final, garantindo que o dotnet-ef esteja disponível
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Instalar o dotnet-ef globalmente
RUN dotnet tool install --global dotnet-ef

# Adicionar .dotnet/tools ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Criar e configurar o entrypoint.sh diretamente no Dockerfile
RUN echo '#!/bin/bash' > entrypoint.sh
RUN echo 'set -e' >> entrypoint.sh
RUN echo 'cd /app' >> entrypoint.sh
RUN echo 'dotnet ef database update --project /app/EmprestimosAPI.csproj' >> entrypoint.sh
RUN echo 'dotnet EmprestimosAPI.dll' >> entrypoint.sh
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
