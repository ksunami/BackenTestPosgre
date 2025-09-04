# ---------- base ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar SOLO csproj (cache)
COPY MarcasAutos.Domain/MarcasAutos.Domain.csproj ./MarcasAutos.Domain/
COPY MarcasAutos.Application/MarcasAutos.Application.csproj ./MarcasAutos.Application/
COPY MarcasAutos.Infrastructure/MarcasAutos.Infrastructure.csproj ./MarcasAutos.Infrastructure/
COPY MarcasAutosAPI/MarcasAutos.API.csproj ./MarcasAutosAPI/

RUN dotnet restore ./MarcasAutosAPI/MarcasAutos.API.csproj

# Copiar el resto del código
COPY MarcasAutos.Domain ./MarcasAutos.Domain
COPY MarcasAutos.Application ./MarcasAutos.Application
COPY MarcasAutos.Infrastructure ./MarcasAutos.Infrastructure
COPY MarcasAutosAPI ./MarcasAutosAPI

WORKDIR /src/MarcasAutosAPI
RUN dotnet publish MarcasAutos.API.csproj -c Release -o /app/publish

# ---------- final ----------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarcasAutos.API.dll"]
