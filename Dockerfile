# Utiliza una imagen base de ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Utiliza una imagen base de SDK de .NET Core para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos del proyecto y restaura las dependencias
COPY ["desafio-backend.csproj", "desafio-backend/"]
RUN dotnet restore "desafio-backend/desafio-backend.csproj"

# Copia el resto de los archivos del proyecto y construye la aplicación
COPY . .
WORKDIR "/src/desafio-backend"
RUN dotnet build "desafio-backend.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "desafio-backend.csproj" -c Release -o /app/publish

# Configura la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "desafio-backend.dll"]
