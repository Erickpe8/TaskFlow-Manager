# TaskFlow.Api

API ASP.NET Core enfocada en autenticación y autorización básica con JWT y políticas de permisos.

## Ejecución con Docker

1. Copia el archivo `.env.example` a `.env` (o usa el `.env` incluido) y ajusta las variables si lo deseas.
2. Construye e inicia los servicios:
   ```bash
   docker compose up --build
   ```
3. La API quedará disponible en `http://localhost:8080` y el SQL Server en `localhost:1433`.

## Credenciales sembradas

Usuario docente creado por el seeding:

- Email: `doc_js_galindo@fesc.edu.co`
- Password: `0123456789`
- Rol: `Teacher` con permisos de administración.
