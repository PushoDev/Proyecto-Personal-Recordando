# Frontend Integration Guide

Este documento describe cómo desarrollar un frontend que se comunique con la API backend de `Proyecto Personal`.

## Resumen

El frontend consumirá una API ASP.NET Core que ofrece los siguientes servicios:

- Autenticación de usuarios (registro, login, refresh, revocación)
- Gestión de inventario de recursos
- Acortador de URLs y seguimiento de clicks

El frontend debe manejar:

- login y registro de usuario
- almacenamiento seguro del token JWT
- envío de `Authorization: Bearer <token>` en peticiones protegidas
- renovación de sesión con refresh token

## Base URL de la API

La API corre por defecto en:

```text
http://localhost:5241
```

### Documentación de la API

Swagger UI:

```text
http://localhost:5241/swagger
```

## Estructura recomendada del frontend

### Páginas / rutas

- `/login` - formulario de autenticación
- `/register` - formulario de registro
- `/dashboard` - área protegida con datos de inventario y URL shortener
- `/shortener` - pantalla de creación de URLs cortas
- `/resources` - listado y gestión de recursos

### Estados importantes

- `isAuthenticated` - indica si el usuario está autenticado
- `user` - datos básicos del usuario actual
- `token` - JWT de acceso
- `refreshToken` - token para renovar sesión
- `tokenExpiration` - fecha/hora de expiración del token

## Autenticación

### Login

Endpoint:

```http
POST /api/auth/login
```

Request body:

```json
{
  "email": "test@example.com",
  "password": "Test123!"
}
```

Respuesta:

```json
{
  "token": "<jwt>",
  "refreshToken": "<refresh_token>",
  "expiration": "2026-04-07T15:30:00Z",
  "user": {
    "id": "12345",
    "email": "test@example.com",
    "nombreCompleto": "Usuario Test",
    "activo": true,
    "fechaRegistro": "2026-04-07T14:00:00Z"
  }
}
```

### Registro

Endpoint:

```http
POST /api/auth/register
```

Request body:

```json
{
  "email": "test@example.com",
  "password": "Test123!",
  "confirmPassword": "Test123!",
  "nombreCompleto": "Usuario Test"
}
```

### Refresh token

Endpoint:

```http
POST /api/auth/refresh
```

Request body:

```json
{
  "refreshToken": "<refresh_token>"
}
```

### Revocar token

Endpoint:

```http
POST /api/auth/revoke
```

Request body:

```json
{
  "refreshToken": "<refresh_token>"
}
```

## Envío de tokens en el frontend

Para llamadas protegidas, el frontend debe incluir el header:

```http
Authorization: Bearer <token>
```

Ejemplo con `fetch`:

```js
const response = await fetch('http://localhost:5241/api/recursos', {
  method: 'GET',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`,
  },
});
```

## Manejo de refresh tokens

- Guardar `token` y `refreshToken` en almacenamiento seguro.
- Si una petición devuelve 401, intentar renovar el token con `POST /api/auth/refresh`.
- Si el refresh falla, redirigir al login.

## Endpoints clave del backend

### Recursos

- `GET /api/recursos` - listar recursos (requiere autenticación)
- `GET /api/recursos/{id}` - obtener recurso por id
- `PUT /api/recursos/{id}/stock` - descontar stock

### Acortador de URLs

- `POST /api/urlshortener/shorten` - crear URL corta
- `GET /api/urlshortener/{codigoCorto}` - redirige a la URL original

## Ejemplo de flujo del frontend

1. El usuario abre `/login`.
2. El frontend envía credenciales a `/api/auth/login`.
3. Si la respuesta es correcta, guarda `token` y `refreshToken`.
4. Redirige al dashboard protegido.
5. En cada petición protegida añade el header `Authorization`.
6. Cuando el token expira, usa `/api/auth/refresh`.

## Recomendaciones técnicas

- Usar `localStorage` o `sessionStorage` con cuidado; preferir mecanismos más seguros si hay acceso al navegador.
- Implementar un interceptor HTTP que renueve tokens automáticamente.
- Manejar errores 401 y 403 de forma centralizada.
- Usar formularios con validación clara de email y contraseña.
- Mostrar mensajes de error en pantalla cuando el backend devuelve fallos de autenticación.

## Consideraciones para el frontend

- El backend está listo para consumo con cualquier frontend moderno (React, Angular, Vue, Blazor).
- Las rutas protegidas deben comprobar si existe un token válido.
- El token JWT se valida en el servidor, no en el cliente.
- El frontend no debe confiar únicamente en la UI; siempre respetar el control de acceso del backend.
