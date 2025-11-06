# Million Properties API

API REST para la gestión de propiedades inmobiliarias desarrollada con .NET 8, MongoDB y arquitectura limpia.

## ?? Descripción

Million Properties API es una solución backend para la gestión integral de propiedades inmobiliarias. Permite consultar propiedades con filtros avanzados, obtener información detallada incluyendo propietarios, imágenes y historial de transacciones.

## ??? Arquitectura

El proyecto sigue los principios de **Clean Architecture** y está organizado en las siguientes capas:

- **MillionApi.Api**: Capa de presentación (Controllers, Middleware)
- **MillionApi.Application**: Lógica de aplicación (CQRS con MediatR, Queries, DTOs, Validaciones)
- **MillionApi.Domain**: Entidades de dominio y excepciones personalizadas
- **MillionApi.Infrastructure**: Acceso a datos (MongoDB, Repositorios, Seed Data)
- **MillionApi.Test.Unit**: Pruebas unitarias

## ?? Tecnologías

- **.NET 8**
- **MongoDB 3.5.0**
- **MediatR 13.1.0** - Patrón CQRS
- **AutoMapper 15.1.0** - Mapeo de objetos
- **FluentValidation 11.3.1** - Validación de modelos
- **Swagger/OpenAPI** - Documentación de API

## ?? Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MongoDB Server](https://www.mongodb.com/try/download/community) o una instancia de MongoDB en contenedor
- Visual Studio 2022+ o Visual Studio Code

## ?? Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/jospina-dev-blossom/MillionBack.git
cd MillionBack
```

### 2. Configurar MongoDB

#### Opción A: MongoDB Local

Instalar MongoDB Server y asegurarse de que esté corriendo en `localhost:27017`.

#### Opción B: MongoDB con Docker

```bash
docker run -d -p 27017:27017 --name mongodb \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=admin123 \
  mongo:latest
```

### 3. Configurar la cadena de conexión

Editar el archivo `appsettings.json` o `appsettings.Development.json`:

```json
{
  "MongoSettings": {
    "Connection": "mongodb://admin:admin123@localhost:27017",
    "DatabaseName": "MillionDb"
  }
}
```

### 4. Restaurar dependencias

```bash
dotnet restore
```

### 5. Compilar el proyecto

```bash
dotnet build
```

## ?? Ejecución

### Ejecutar desde la línea de comandos

```bash
cd MillionApi
dotnet run
```

### Ejecutar desde Visual Studio

1. Abrir la solución `MillionApi.sln`
2. Establecer `MillionApi.Api` como proyecto de inicio
3. Presionar `F5` o hacer clic en el botón "Run"

La API estará disponible en:
- **HTTPS**: `https://localhost:49404`
- **HTTP**: `http://localhost:49405`

## ?? Documentación Swagger

Una vez que la aplicación esté ejecutándose, acceder a la documentación interactiva de Swagger:

```
https://localhost:49404/swagger
```

Swagger proporciona:
- Documentación completa de todos los endpoints
- Posibilidad de probar la API directamente desde el navegador
- Esquemas de request/response
- Códigos de estado HTTP

## ?? Endpoints Principales

### GET /api/v1/properties

Obtiene una lista paginada de propiedades con filtros opcionales.

**Parámetros de consulta:**
- `Name` (opcional): Filtrar por nombre
- `Address` (opcional): Filtrar por dirección
- `MinPrice` (opcional): Precio mínimo
- `MaxPrice` (opcional): Precio máximo
- `PageNumber` (default: 1): Número de página
- `PageSize` (default: 10): Tamaño de página

**Ejemplo:**
```
GET /api/v1/properties?Name=Casa&MinPrice=100000&MaxPrice=500000&PageNumber=1&PageSize=10
```

### GET /api/v1/properties/{id}

Obtiene una propiedad por su ID con información completa (propietario, imágenes, historial).

**Parámetros:**
- `id`: ObjectId de MongoDB (24 caracteres hexadecimales)

**Ejemplo:**
```
GET /api/v1/properties/507f1f77bcf86cd799439011
```

## ?? Datos de Prueba (Seed Data)

La aplicación incluye un sistema de seed data que se ejecuta automáticamente al iniciar por primera vez. Este seed incluye:

- 5 propietarios (Owners)
- 20 propiedades con imágenes de Unsplash
- Imágenes adicionales para las primeras 10 propiedades
- Historial de transacciones (PropertyTraces) para todas las propiedades

Los datos se insertan solo si la base de datos está vacía.

## ?? Características Destacadas

### Manejo de Excepciones Personalizado

- Middleware global de excepciones
- Respuestas estructuradas con ProblemDetails (RFC 7807)
- Códigos de estado HTTP apropiados

### Validación con FluentValidation

- Validación automática de requests
- Reglas de negocio claras y mantenibles
- Mensajes de error descriptivos

### Optimización de Consultas

- Índices MongoDB para consultas eficientes
- Índices compuestos para búsquedas filtradas
- Índices de texto para búsquedas full-text

### Paginación

- Paginación nativa en todas las consultas de lista
- Metadata de paginación en las respuestas (TotalCount, PageNumber, PageSize)

## ?? Pruebas

### Ejecutar pruebas unitarias

```bash
dotnet test
```

## ?? Estructura del Proyecto

```
MillionApi/
??? MillionApi.Api/     # API Controllers y Middleware
? ??? Controllers/
?   ??? Middleware/
?   ??? Program.cs
??? MillionApi.Application/         # Lógica de aplicación
?   ??? Interfaces/
?   ??? Queries/
?   ??? Mappings/
?   ??? Validators/
??? MillionApi.Domain/    # Entidades de dominio
?   ??? AggregateModel/
?   ??? Exceptions/
??? MillionApi.Infrastructure/ # Acceso a datos
?   ??? Data/
?   ??? Repositories/
?   ??? SeedData/
??? MillionApi.Test.Unit/      # Pruebas unitarias
```

## ??? Configuración Adicional

### Variables de Entorno

Puedes configurar la aplicación mediante variables de entorno:

```bash
# Ejemplo en Linux/Mac
export MongoSettings__Connection="mongodb://admin:admin123@localhost:27017"
export MongoSettings__DatabaseName="MillionDb"

# Ejemplo en Windows (PowerShell)
$env:MongoSettings__Connection="mongodb://admin:admin123@localhost:27017"
$env:MongoSettings__DatabaseName="MillionDb"
```

### Configuración por Entorno

- `appsettings.json`: Configuración base
- `appsettings.Development.json`: Configuración para desarrollo
- `appsettings.Production.json`: Configuración para producción (crear si es necesario)

## ?? Notas de Desarrollo

### Agregar nuevas propiedades al filtro

Para agregar nuevos filtros de búsqueda, modificar:
1. `GetPropertiesRequest.cs` - Agregar propiedades
2. `GetPropertiesRequestValidator.cs` - Agregar validaciones
3. `PropertyRepository.cs` - Agregar filtros en el método `GetAsync`

### Agregar nuevos endpoints

1. Crear el Query/Command en `MillionApi.Application`
2. Crear el Handler correspondiente
3. Agregar validadores con FluentValidation
4. Crear el endpoint en el Controller
5. Documentar con XML comments para Swagger

## ?? Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## ?? Licencia

Este proyecto es privado y está bajo la licencia de Million Properties Team.

## ?? Contacto

Million Properties Team - [GitHub](https://github.com/jospina-dev-blossom)
