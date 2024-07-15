# Usar a imagem SDK para constru��o e publica��o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Instalar o dotnet-ef globalmente durante a fase de build
RUN dotnet tool install --global dotnet-ef

# Adicionar .dotnet/tools ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiar e restaurar depend�ncias
COPY ["EmprestimosAPI/EmprestimosAPI.csproj", "EmprestimosAPI/"]
RUN dotnet restore "EmprestimosAPI/EmprestimosAPI.csproj"

# Copiar o restante do c�digo e construir
COPY EmprestimosAPI/ EmprestimosAPI/
WORKDIR /src/EmprestimosAPI
RUN dotnet build "EmprestimosAPI.csproj" -c Release -o /app/build

# Publicar a aplica��o
RUN dotnet publish "EmprestimosAPI.csproj" -c Release -o /app/publish

# Usar a imagem SDK para a etapa final para garantir que o SDK esteja dispon�vel
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copiar a ferramenta dotnet-ef para a imagem final
COPY --from=build /root/.dotnet /root/.dotnet

# Adicionar .dotnet/tools ao PATH na imagem final
ENV PATH="$PATH:/root/.dotnet/tools"

# Criar e configurar o entrypoint.sh diretamente no Dockerfile
RUN echo '#!/bin/bash' > entrypoint.sh
RUN echo 'set -e' >> entrypoint.sh
RUN echo 'dotnet-ef database update --project /src/EmprestimosAPI/EmprestimosAPI.csproj' >> entrypoint.sh
RUN echo 'dotnet EmprestimosAPI.dll' >> entrypoint.sh
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
