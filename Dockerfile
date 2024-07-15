# Usar a imagem SDK para constru��o e publica��o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Instalar o dotnet-ef globalmente durante a fase de build
RUN dotnet tool install --global dotnet-ef

# Adicionar .dotnet/tools ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiar e restaurar depend�ncias
COPY ["EmprestimosAPI.csproj", "."]
RUN dotnet restore "EmprestimosAPI.csproj"

# Copiar o restante do c�digo e construir
COPY . .
RUN dotnet build "EmprestimosAPI.csproj" -c Release -o /app/build

# Publicar a aplica��o
RUN dotnet publish "EmprestimosAPI.csproj" -c Release -o /app/publish

# Usar a imagem ASP.NET Core Runtime para a etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Copiar a ferramenta dotnet-ef para a imagem final
COPY --from=build /root/.dotnet /root/.dotnet

# Adicionar .dotnet/tools ao PATH na imagem final
ENV PATH="$PATH:/root/.dotnet/tools"

# Criar e configurar o entrypoint.sh diretamente no Dockerfile
RUN echo '#!/bin/bash' > entrypoint.sh
RUN echo 'set -e' >> entrypoint.sh
RUN echo 'cd /app' >> entrypoint.sh
RUN echo 'dotnet-ef database update --project /app/EmprestimosAPI.csproj' >> entrypoint.sh
RUN echo 'dotnet EmprestimosAPI.dll' >> entrypoint.sh
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
