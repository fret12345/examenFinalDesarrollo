# ===============================
# BUILD
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar todo el proyecto al contenedor
COPY . .

# Restaurar dependencias usando la solución
RUN dotnet restore "Financiera.slnx"

# CORRECCIÓN: Apuntar a la carpeta 'Financiera' que es la correcta
WORKDIR "/src/Financiera"
RUN dotnet publish "Financiera.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===============================
# RUNTIME
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Librerías necesarias
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

# Copiar la app publicada
COPY --from=build /app/publish .

# Render usa el puerto dinámico
ENV ASPNETCORE_URLS=http://+:${PORT}

# Exponer puerto
EXPOSE 8080

# CORRECCIÓN: Ejecutar el DLL correcto del proyecto
ENTRYPOINT ["dotnet", "Financiera.dll"]