# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar todos os projetos para a mesma estrutura que está na raiz
COPY ProjetoRl.API/ProjetoRl.API.csproj ProjetoRl.API/
COPY ProjetoRl.Domain/ProjetoRl.Domain.csproj ProjetoRl.Domain/
COPY ProjetoRl.Commom/Commom.csproj ProjetoRl.Commom/
COPY Infra/Data/Repositories/MongoDB/Implementation/RepositoriesMongoDB.csproj Infra/Data/Repositories/MongoDB/Implementation/

# Restaurar dependências
WORKDIR /app/ProjetoRl.API
RUN dotnet restore ProjetoRl.API.csproj

# Copiar todo o código da API e dos projetos dependentes
COPY . .

# Compilar e publicar
RUN dotnet publish ProjetoRl.API/ProjetoRl.API.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjetoRl.API.dll"]
