# Proyecto Personal - API Endpoints

## Gestión de Inventario

### Obtener Recurso
```http
GET /api/recursos/{id}
```

**Respuesta 200:**
```json
{
  "id": 1,
  "nombre": "Producto A",
  "stock": 95,
  "umbralMinimo": 10,
  "urlOriginal": null,
  "codigoCorto": null,
  "clicks": 0,
  "estaEnEstadoCritico": false
}
```

### Descontar Stock
```http
PUT /api/recursos/{id}/stock
Content-Type: application/json

{
  "cantidad": 5
}
```

**Respuesta 200:**
```json
{
  "id": 1,
  "nombre": "Producto A",
  "stock": 90,
  "umbralMinimo": 10,
  "urlOriginal": null,
  "codigoCorto": null,
  "clicks": 0,
  "estaEnEstadoCritico": false
}
```

## Acortador de URLs

### Crear URL Corta
```http
POST /api/urlshortener/shorten
Content-Type: application/json

{
  "urlOriginal": "https://www.ejemplo.com/pagina-larga",
  "nombre": "Página de Ejemplo",
  "stockInicial": 100,
  "umbralMinimo": 10
}
```

**Respuesta 201:**
```json
{
  "id": 2,
  "nombre": "Página de Ejemplo",
  "stock": 100,
  "umbralMinimo": 10,
  "urlOriginal": "https://www.ejemplo.com/pagina-larga",
  "codigoCorto": "Ab3Xy9",
  "clicks": 0,
  "estaEnEstadoCritico": false
}
```

### Redirigir a URL Original
```http
GET /api/urlshortener/{codigoCorto}
```
**Respuesta:** Redirección 302 a la URL original (registra el click)

### Obtener Estadísticas
```http
GET /api/urlshortener/{codigoCorto}/stats
```

**Respuesta 200:**
```json
{
  "codigoCorto": "Ab3Xy9",
  "totalClicks": 42,
  "clicksUltimaHora": 3,
  "recursoId": 2,
  "nombreRecurso": "Página de Ejemplo",
  "clicksPorDia": [
    {
      "fecha": "2026-04-07T00:00:00",
      "cantidadClicks": 15
    }
  ]
}
```

## Códigos de Estado HTTP

- **200 OK**: Operación exitosa
- **201 Created**: Recurso creado
- **302 Found**: Redirección (para URLs cortas)
- **400 Bad Request**: Datos inválidos
- **404 Not Found**: Recurso no encontrado
- **409 Conflict**: Stock insuficiente
- **500 Internal Server Error**: Error del servidor

## Swagger UI

En desarrollo, accede a `http://localhost:5000` para ver la documentación interactiva de la API.