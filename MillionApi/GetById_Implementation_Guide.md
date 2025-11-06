# Mejoras implementadas en el endpoint GetById

## Cambios realizados

### 1. **Nuevas Entidades Creadas**
- `Owner.cs`: Propietario de la propiedad con GUID como ID
- `PropertyImage.cs`: Imágenes de la propiedad
- `PropertyTrace.cs`: Historial de transacciones/trazas de la propiedad

### 2. **Validación GUID/ObjectId**
- El endpoint GetById ahora valida que el ID sea un ObjectId válido de MongoDB
- Si el formato es inválido, retorna un error 400 (Bad Request)
- Si la propiedad no existe, retorna un error 404 (Not Found)

### 3. **Optimización de Queries con Índices MongoDB**
Se crearon los siguientes índices para optimizar las consultas:

#### Properties Collection:
- `idx_properties_search`: Índice compuesto para Name, AddressProperty y PriceProperty
- `idx_properties_text_search`: Índice de texto para búsquedas en Name y AddressProperty
- `idx_properties_owner`: Índice para IdOwner (usado en joins)

#### PropertyImage Collection:
- `idx_propertyimage_idproperty`: Índice para IdProperty
- `idx_propertyimage_enabled`: Índice compuesto para IdProperty y Enabled

#### PropertyTrace Collection:
- `idx_propertytrace_idproperty`: Índice para IdProperty
- `idx_propertytrace_date`: Índice compuesto para IdProperty y DateSale (descendente)

#### Owner Collection:
- `idx_owner_id`: Índice único para IdOwner

### 4. **Nuevo DTO de Respuesta**
`PropertyDetailResponseDto` incluye:
- Información completa de la propiedad
- Datos del propietario (Owner)
- Lista de imágenes (PropertyImage)
- Historial de trazas (PropertyTrace)

### 5. **Nuevo Método de Repositorio**
`GetByIdWithDetailsAsync`: Obtiene la propiedad con todas sus relaciones en una sola llamada optimizada usando los índices creados.

## Uso del Endpoint

### Request
```http
GET /api/v1/properties/{id}
```

**Parámetro:**
- `id`: ObjectId de MongoDB (24 caracteres hexadecimales)

### Responses

#### 200 OK
```json
{
  "id": "507f1f77bcf86cd799439011",
  "name": "Casa Moderna en el Centro",
  "addressProperty": "Calle Principal 789, Centro",
  "priceProperty": 250000.00,
  "imageUrl": "https://example.com/properties/casa1.jpg",
  "codeInternal": 1001,
  "year": 2020,
  "owner": {
    "idOwner": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Juan Pérez",
    "address": "Calle Principal 123, Ciudad",
    "photo": "https://example.com/photos/juan.jpg",
    "birthday": "1980-05-15T00:00:00Z"
  },
  "images": [
    {
      "idPropertyImage": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "idProperty": "507f1f77bcf86cd799439011",
      "file": "https://example.com/properties/casa1-img1.jpg",
      "enabled": true
    }
  ],
  "traces": [
    {
      "idPropertyTrace": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "idProperty": "507f1f77bcf86cd799439011",
      "dateSale": "2023-06-20T00:00:00Z",
   "name": "Actualización de Valor",
      "value": 260000.00,
      "tax": 26000.00
    }
  ]
}
```

#### 400 Bad Request
```json
{
  "type": "BadRequestException",
  "title": "Bad Request",
  "status": 400,
  "detail": "Property ID must be a valid ObjectId format."
}
```

#### 404 Not Found
```json
{
  "type": "NotFoundException",
  "title": "Not Found",
  "status": 404,
  "detail": "Property with id '507f1f77bcf86cd799439011' was not found."
}
```

## Scripts de MongoDB para Verificación

### Verificar índices creados
```javascript
// Para Properties
db.Properties.getIndexes()

// Para PropertyImage
db.PropertyImage.getIndexes()

// Para PropertyTrace
db.PropertyTrace.getIndexes()

// Para Owner
db.Owner.getIndexes()
```

### Verificar performance de queries
```javascript
// Explicar plan de ejecución para GetById con detalles
db.Properties.find({ "_id": ObjectId("507f1f77bcf86cd799439011") }).explain("executionStats")

// Verificar uso de índice en búsqueda de Owner
db.Owner.find({ "IdOwner": "3fa85f64-5717-4562-b3fc-2c963f66afa6" }).explain("executionStats")

// Verificar uso de índice en búsqueda de imágenes
db.PropertyImage.find({ "IdProperty": "507f1f77bcf86cd799439011" }).explain("executionStats")

// Verificar uso de índice en búsqueda de traces ordenados
db.PropertyTrace.find({ "IdProperty": "507f1f77bcf86cd799439011" }).sort({ "DateSale": -1 }).explain("executionStats")
```

### Insertar datos de prueba
```javascript
// Owner 1
db.Owner.insertOne({
  "_id": UUID("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
  "Name": "Juan Pérez",
  "Address": "Calle Principal 123, Ciudad",
  "Photo": "https://example.com/photos/juan.jpg",
  "Birthday": ISODate("1980-05-15T00:00:00Z")
})

// Property 1
var propertyId = ObjectId()
db.Properties.insertOne({
  "_id": propertyId,
  "IdOwner": UUID("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
  "Name": "Casa Moderna en el Centro",
  "AddressProperty": "Calle Principal 789, Centro",
  "PriceProperty": NumberDecimal("250000.00"),
  "ImageUrl": "https://example.com/properties/casa1.jpg",
  "CodeInternal": 1001,
  "Year": 2020
})

// PropertyImage 1
db.PropertyImage.insertOne({
"_id": UUID("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
  "IdProperty": propertyId.str,
  "File": "https://example.com/properties/casa1-img1.jpg",
  "Enabled": true
})

// PropertyTrace 1
db.PropertyTrace.insertOne({
  "_id": UUID("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
  "IdProperty": propertyId.str,
  "DateSale": ISODate("2023-06-20T00:00:00Z"),
  "Name": "Actualización de Valor",
  "Value": NumberDecimal("260000.00"),
  "Tax": NumberDecimal("26000.00")
})
```

## Consideraciones de Performance

1. **Índices Compuestos**: Se utilizan índices compuestos para optimizar queries que filtran por múltiples campos
2. **Índices de Texto**: Para búsquedas de texto en Name y Address
3. **Índices para Joins**: Índices en las claves foráneas (IdOwner, IdProperty) para optimizar las búsquedas relacionales
4. **Orden Descendente**: El índice de PropertyTrace incluye DateSale en orden descendente para obtener las trazas más recientes primero

## Seed Data

Para cargar datos de prueba, puedes usar la clase `MongoDbSeedData` en Program.cs:

```csharp
// En Program.cs, después de app.Build()
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await MongoDbSeedData.SeedAsync(services);
}
```

## Testing

### Ejemplos con curl

```bash
# Obtener propiedad por ID válido
curl -X GET "https://localhost:7001/api/v1/properties/507f1f77bcf86cd799439011" -H "accept: application/json"

# Intentar con ID inválido (debe retornar 400)
curl -X GET "https://localhost:7001/api/v1/properties/invalid-id" -H "accept: application/json"

# Intentar con ID que no existe (debe retornar 404)
curl -X GET "https://localhost:7001/api/v1/properties/507f1f77bcf86cd799439099" -H "accept: application/json"
```
