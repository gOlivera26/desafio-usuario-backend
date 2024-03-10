# Utilizar la imagen oficial del SDK de .NET Core 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Copiar solo el archivo csproj para restaurar las dependencias
COPY *.csproj ./

RUN dotnet restore

# Copiar el resto de los archivos y construir la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de imagen de tiempo de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-env
WORKDIR /app

# Copiar los artefactos de construcción de la etapa de construcción
COPY --from=build-env /app/out .

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "desafio-backend.dll"]


