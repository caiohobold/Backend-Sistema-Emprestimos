#!/bin/bash
set -e

# Rodar migrations
dotnet ef database update

# Iniciar a aplicação
dotnet EmprestimosAPI.dll