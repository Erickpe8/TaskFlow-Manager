# 📋 TaskFlow Manager

<div align="center">

![TaskFlow Logo](https://img.shields.io/badge/TaskFlow-Manager-blue?style=for-the-badge&logo=trello)

**Sistema Web Full-Stack de Gestión de Tareas con Tablero Kanban Interactivo**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-17+-DD0031?style=flat&logo=angular)](https://angular.io/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?style=flat&logo=typescript)](https://www.typescriptlang.org/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat&logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**Proyecto Final - DAAR**  
*Facultad de Ingeniería | Fundación de Estudios Superiores Comfanorte*

[🚀 Demo](#demo) • [📖 Documentación](#documentación) • [🛠️ Instalación](#instalación) • [👥 Contribuidores](#contribuidores)

</div>

---

## 📑 Tabla de Contenidos

- [Descripción General](#-descripción-general)
- [Características Principales](#-características-principales)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Documentación Técnica](#-documentación-técnica)
- [Funcionalidades Implementadas](#-funcionalidades-implementadas)
- [API Endpoints](#-api-endpoints)
- [Comandos Útiles](#-comandos-útiles)
- [Dockerización](#-dockerización)
- [Contribuidores](#-contribuidores)
- [Licencia](#-licencia)
- [Contacto](#-contacto)

---

## 🎯 Descripción General

**TaskFlow Manager** es un sistema web full-stack profesional diseñado para la gestión eficiente de tareas mediante un **tablero Kanban completamente interactivo**. Desarrollado como proyecto final para la asignatura DAAR, este sistema implementa las mejores prácticas de desarrollo moderno y arquitectura limpia.

El proyecto nace de la necesidad de contar con una herramienta robusta, escalable y visualmente atractiva para organizar flujos de trabajo, permitiendo a equipos y usuarios individuales gestionar sus tareas de manera intuitiva y profesional.

### 🎓 Contexto Académico

Este proyecto representa la culminación de conocimientos adquiridos en:
- Desarrollo de Aplicaciones Avanzadas en Red (DAAR)
- Arquitecturas cliente-servidor modernas
- Patrones de diseño y buenas prácticas
- Seguridad en aplicaciones web
- Integración de tecnologías full-stack

### 🌟 ¿Por qué TaskFlow Manager?

- ✅ **Arquitectura Moderna**: Backend con .NET 10 Minimal API y Frontend con Angular 17+
- ✅ **Seguridad Robusta**: Autenticación JWT con control de roles
- ✅ **Experiencia Kanban**: Tablero interactivo con drag & drop
- ✅ **Exportaciones Profesionales**: Generación de reportes en PDF y Excel
- ✅ **Dockerización Completa**: Listo para despliegue en producción
- ✅ **Código Limpio**: Arquitectura bien estructurada y documentada
- ✅ **API REST Documentada**: OpenAPI/Swagger integrado

---

## ✨ Características Principales

### 🎨 Frontend (Angular 17+)

- **Tablero Kanban Interactivo**
  - Arrastrar y soltar tareas entre columnas (Drag & Drop)
  - Actualización en tiempo real de estados
  - Interfaz responsive y moderna
  - Animaciones fluidas

- **Gestión de Tareas**
  - Crear, editar y eliminar tareas
  - Asignar prioridades y fechas límite
  - Visualización por categorías
  - Búsqueda y filtrado avanzado

- **Sistema de Autenticación**
  - Login seguro con JWT
  - Registro de nuevos usuarios
  - Recuperación de contraseña
  - Protección de rutas con Guards

- **Dashboard Personalizado**
  - Métricas y estadísticas
  - Tareas pendientes y completadas
  - Gráficos de productividad
  - Vista de calendario

### ⚙️ Backend (.NET 10 Minimal API)

- **API REST Completa**
  - Endpoints RESTful bien estructurados
  - Versionado de API
  - Validación de datos robusta
  - Manejo de errores centralizado

- **Seguridad Avanzada**
  - Autenticación JWT (JSON Web Tokens)
  - Control de roles (Admin, User, Guest)
  - Hash de contraseñas con BCrypt
  - Protección contra ataques comunes (CSRF, XSS)

- **Base de Datos**
  - Entity Framework Core 10
  - SQL Server como motor de base de datos
  - Migraciones automáticas
  - Relaciones optimizadas

- **Arquitectura Limpia**
  - Separación de responsabilidades
  - Inyección de dependencias
  - Patrón Repository
  - DTOs para transferencia de datos

### 📊 Funcionalidades Avanzadas

- **Exportaciones**
  - Generación de reportes en PDF
  - Exportación de datos a Excel
  - Personalización de templates
  - Descarga masiva de información

---

## 🏗️ Arquitectura del Sistema

TaskFlow Manager implementa una **arquitectura cliente-servidor desacoplada**, permitiendo escalabilidad y mantenimiento independiente de cada capa.

```
┌─────────────────────────────────────────────────────────────┐
│                      TASKFLOW MANAGER                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌──────────────────────┐      ┌──────────────────────┐    │
│  │                      │      │                      │    │
│  │   FRONTEND           │◄────►│   BACKEND            │    │
│  │   Angular 17+        │ HTTP │   .NET 10            │    │
│  │                      │ REST │   Minimal API        │    │
│  │  ┌────────────────┐  │      │  ┌────────────────┐  │    │
│  │  │  Components    │  │      │  │  Controllers   │  │    │
│  │  │  - Auth        │  │      │  │  - Auth        │  │    │
│  │  │  - Tasks       │  │      │  │  - Tasks       │  │    │
│  │  │  - Kanban      │  │      │  │  - Users       │  │    │
│  │  └────────────────┘  │      │  └────────────────┘  │    │
│  │                      │      │                      │    │
│  │  ┌────────────────┐  │      │  ┌────────────────┐  │    │
│  │  │  Services      │  │      │  │  Services      │  │    │
│  │  │  - API Client  │  │      │  │  - Business    │  │    │
│  │  │  - Auth        │  │      │  │  - Validation  │  │    │
│  │  └────────────────┘  │      │  └────────────────┘  │    │
│  │                      │      │                      │    │
│  │  ┌────────────────┐  │      │  ┌────────────────┐  │    │
│  │  │  Guards        │  │      │  │  Repositories  │  │    │
│  │  │  - Auth Guard  │  │      │  │  - Task Repo   │  │    │
│  │  │  - Role Guard  │  │      │  │  - User Repo   │  │    │
│  │  └────────────────┘  │      │  └────────────────┘  │    │
│  │                      │      │                      │    │
│  └──────────────────────┘      └──────────┬───────────┘    │
│                                           │                │
│                                           ▼                │
│                                  ┌────────────────┐        │
│                                  │   DATABASE     │        │
│                                  │   SQL Server   │        │
│                                  │                │        │
│                                  │  - Users       │        │
│                                  │  - Tasks       │        │
│                                  │  - Categories  │        │
│                                  │  - Roles       │        │
│                                  └────────────────┘        │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### 📐 Patrón de Diseño

El proyecto implementa una **arquitectura en capas** inspirada en Clean Architecture:

**Frontend (Angular)**
```
Presentation Layer
    ├── Components (UI)
    ├── Services (Business Logic)
    ├── Guards (Security)
    └── Models (Data Structures)
```

**Backend (.NET)**
```
API Layer
    ├── Controllers (Endpoints)
    ├── DTOs (Data Transfer Objects)
    │
Business Logic Layer
    ├── Services (Business Rules)
    ├── Interfaces (Contracts)
    │
Data Access Layer
    ├── Repositories (Data Operations)
    ├── DbContext (EF Core)
    └── Models (Entities)
```

### 🔄 Flujo de Comunicación

1. **Usuario → Frontend**: Interacción con la interfaz Angular
2. **Frontend → Backend**: Petición HTTP REST (GET, POST, PUT, DELETE)
3. **Backend → Validación**: JWT y permisos de rol
4. **Backend → Base de Datos**: Consulta mediante EF Core
5. **Base de Datos → Backend**: Respuesta con datos
6. **Backend → Frontend**: JSON response
7. **Frontend → Usuario**: Actualización de vista

---

## 🛠️ Tecnologías Utilizadas

### Backend Stack

| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| **.NET** | 10.0 | Framework principal del backend |
| **C#** | 12 | Lenguaje de programación |
| **Entity Framework Core** | 10.0 | ORM para acceso a datos |
| **SQL Server** | 2022 | Sistema de gestión de base de datos |
| **JWT Bearer** | Latest | Autenticación y autorización |
| **Swagger/OpenAPI** | 3.0 | Documentación de API |
| **BCrypt.Net** | Latest | Hash de contraseñas |
| **AutoMapper** | Latest | Mapeo objeto-objeto |

### Frontend Stack

| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| **Angular** | 17+ | Framework SPA principal |
| **Angular CLI** | 21 | Herramientas de desarrollo |
| **TypeScript** | 5.0+ | Lenguaje tipado para JavaScript |
| **RxJS** | Latest | Programación reactiva |
| **Angular Material** | 17+ | Componentes UI |
| **Chart.js** | Latest | Gráficos y visualizaciones |
| **HTML5/CSS3** | - | Maquetación y estilos |
| **SCSS** | - | Preprocesador CSS |

### DevOps & Herramientas

| Herramienta | Propósito |
|------------|-----------|
| **Docker** | Containerización |
| **Git** | Control de versiones |
| **GitHub** | Repositorio remoto |
| **Postman** | Testing de API |
| **Visual Studio Code** | Editor de código |
| **Visual Studio 2022** | IDE para .NET |
| **SQL Server Management Studio** | Gestión de BD |

---

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado lo siguiente en tu sistema:

### Para el Backend (.NET)

- **.NET SDK 10.0** o superior
  ```bash
  # Verificar instalación
  dotnet --version
  ```
  [Descargar .NET 10](https://dotnet.microsoft.com/download)

- **SQL Server 2022** o superior
  - SQL Server Express (gratuito)
  - SQL Server Developer Edition (gratuito)
  [Descargar SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads)

- **SQL Server Management Studio (SSMS)** (opcional pero recomendado)
  [Descargar SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### Para el Frontend (Angular)

- **Node.js** 18.0 o superior (incluye npm)
  ```bash
  # Verificar instalación
  node --version
  npm --version
  ```
  [Descargar Node.js](https://nodejs.org/)

- **Angular CLI 21**
  ```bash
  # Instalar globalmente
  npm install -g @angular/cli
  
  # Verificar instalación
  ng version
  ```

### Opcionales (Desarrollo Avanzado)

- **Docker Desktop** (para containerización)
  [Descargar Docker](https://www.docker.com/products/docker-desktop)

- **Git** (control de versiones)
  ```bash
  git --version
  ```
  [Descargar Git](https://git-scm.com/)

---

## 🚀 Instalación y Configuración

### 1️⃣ Clonar el Repositorio

```bash
# HTTPS
git clone https://github.com/Erickpe8/TaskFlow-Manager.git

# SSH
git clone git@github.com:Erickpe8/TaskFlow-Manager.git

# Entrar al directorio
cd TaskFlow-Manager
```

### 2️⃣ Configuración del Backend

#### Paso 1: Navegar al directorio del backend

```bash
cd Backend
```

#### Paso 2: Restaurar dependencias

```bash
dotnet restore
```

#### Paso 3: Configurar la cadena de conexión

Edita el archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskFlowDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "TU_CLAVE_SECRETA_SUPER_SEGURA_DE_32_CARACTERES_MINIMO",
    "Issuer": "TaskFlowAPI",
    "Audience": "TaskFlowClient",
    "ExpiryInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Nota**: Si usas autenticación de SQL Server, modifica la cadena de conexión:

```
"Server=localhost;Database=TaskFlowDB;User Id=tu_usuario;Password=tu_contraseña;TrustServerCertificate=True"
```

#### Paso 4: Aplicar migraciones de base de datos

```bash
# Crear migración inicial (si no existe)
dotnet ef migrations add InitialCreate

# Actualizar base de datos
dotnet ef database update
```

Si no tienes instalado `dotnet ef`, instálalo:

```bash
dotnet tool install --global dotnet-ef
```

#### Paso 5: Ejecutar el backend

```bash
dotnet run
```

El backend estará disponible en:
- **HTTP**: `http://localhost:5208`
- **OpenAPI/Swagger**: `http://localhost:5208/openapi/v1.json`
- **Weather Forecast** (test): `http://localhost:5208/weatherforecast`

### 3️⃣ Configuración del Frontend

#### Paso 1: Navegar al directorio del frontend

```bash
# Desde la raíz del proyecto
cd Frontend
```

#### Paso 2: Instalar dependencias

```bash
npm install
```

**Nota**: Si encuentras errores de vulnerabilidades, puedes ejecutar:

```bash
npm audit fix
```

#### Paso 3: Configurar la URL de la API

Edita el archivo `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5208/api'
};
```

Y también `src/environments/environment.prod.ts` para producción:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://tu-dominio.com/api'
};
```

#### Paso 4: Ejecutar el frontend

```bash
ng serve
```

O para abrir automáticamente en el navegador:

```bash
ng serve --open
```

El frontend estará disponible en:
- **HTTP**: `http://localhost:4200`

### 4️⃣ Verificación de la Instalación

Una vez que ambos servicios estén corriendo, verifica:

✅ **Backend**: Abre `http://localhost:5208/weatherforecast` - deberías ver un JSON con datos de prueba

✅ **Frontend**: Abre `http://localhost:4200` - deberías ver la pantalla de login

✅ **Base de Datos**: Verifica en SSMS que la base de datos `TaskFlowDB` existe con sus tablas

---

## 📁 Estructura del Proyecto

### 🌲 Vista General

```
TaskFlow-Manager/
│
├── Backend/                          # API .NET 10
│   ├── Controllers/                  # Endpoints REST
│   │   ├── AuthController.cs
│   │   ├── TasksController.cs
│   │   └── UsersController.cs
│   │
│   ├── Models/                       # Entidades EF Core
│   │   ├── User.cs
│   │   ├── Task.cs
│   │   ├── Category.cs
│   │   └── Role.cs
│   │
│   ├── DTOs/                         # Data Transfer Objects
│   │   ├── Auth/
│   │   │   ├── LoginDto.cs
│   │   │   └── RegisterDto.cs
│   │   ├── Tasks/
│   │   │   ├── TaskDto.cs
│   │   │   └── CreateTaskDto.cs
│   │   └── Users/
│   │       └── UserDto.cs
│   │
│   ├── Services/                     # Lógica de negocio
│   │   ├── AuthService.cs
│   │   ├── TaskService.cs
│   │   └── UserService.cs
│   │
│   ├── Interfaces/                   # Contratos
│   │   ├── IAuthService.cs
│   │   ├── ITaskService.cs
│   │   └── IUserService.cs
│   │
│   ├── Repositories/                 # Acceso a datos
│   │   ├── TaskRepository.cs
│   │   └── UserRepository.cs
│   │
│   ├── Data/                         # Contexto EF Core
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   │
│   ├── Middleware/                   # Middleware personalizado
│   │   └── ErrorHandlingMiddleware.cs
│   │
│   ├── Utilities/                    # Utilidades
│   │   ├── JwtHelper.cs
│   │   └── PasswordHelper.cs
│   │
│   ├── Program.cs                    # Punto de entrada
│   ├── appsettings.json             # Configuración
│   ├── appsettings.Development.json
│   └── TaskFlow.Api.csproj          # Archivo de proyecto
│
├── Frontend/                         # Angular 17+
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/                # Módulo core
│   │   │   │   ├── services/
│   │   │   │   │   ├── auth.service.ts
│   │   │   │   │   ├── task.service.ts
│   │   │   │   │   └── api.service.ts
│   │   │   │   ├── guards/
│   │   │   │   │   ├── auth.guard.ts
│   │   │   │   │   └── role.guard.ts
│   │   │   │   ├── interceptors/
│   │   │   │   │   ├── auth.interceptor.ts
│   │   │   │   │   └── error.interceptor.ts
│   │   │   │   └── models/
│   │   │   │       ├── user.model.ts
│   │   │   │       └── task.model.ts
│   │   │   │
│   │   │   ├── auth/                # Módulo de autenticación
│   │   │   │   ├── login/
│   │   │   │   │   ├── login.component.ts
│   │   │   │   │   ├── login.component.html
│   │   │   │   │   └── login.component.scss
│   │   │   │   └── register/
│   │   │   │       ├── register.component.ts
│   │   │   │       ├── register.component.html
│   │   │   │       └── register.component.scss
│   │   │   │
│   │   │   ├── tasks/               # Módulo de tareas
│   │   │   │   ├── list/
│   │   │   │   │   ├── list.component.ts
│   │   │   │   │   ├── list.component.html
│   │   │   │   │   └── list.component.scss
│   │   │   │   ├── kanban/
│   │   │   │   │   ├── kanban.component.ts
│   │   │   │   │   ├── kanban.component.html
│   │   │   │   │   └── kanban.component.scss
│   │   │   │   └── detail/
│   │   │   │       ├── detail.component.ts
│   │   │   │       ├── detail.component.html
│   │   │   │       └── detail.component.scss
│   │   │   │
│   │   │   ├── shared/              # Módulo compartido
│   │   │   │   ├── components/
│   │   │   │   │   ├── navbar/
│   │   │   │   │   ├── footer/
│   │   │   │   │   └── loading/
│   │   │   │   └── pipes/
│   │   │   │       └── date-format.pipe.ts
│   │   │   │
│   │   │   ├── app.component.ts
│   │   │   ├── app.component.html
│   │   │   ├── app.component.scss
│   │   │   └── app.routes.ts
│   │   │
│   │   ├── assets/                  # Recursos estáticos
│   │   │   ├── images/
│   │   │   ├── icons/
│   │   │   └── styles/
│   │   │
│   │   ├── environments/            # Configuración por entorno
│   │   │   ├── environment.ts
│   │   │   └── environment.prod.ts
│   │   │
│   │   ├── index.html
│   │   ├── main.ts
│   │   └── styles.scss
│   │
│   ├── angular.json
│   ├── package.json
│   ├── tsconfig.json
│   └── README.md
│
├── .gitignore
├── README.md                         # Este archivo
└── LICENSE
```

### 📦 Descripción de Carpetas Principales

#### Backend

- **Controllers**: Contiene los controladores que exponen los endpoints REST
- **Models**: Entidades que representan las tablas de la base de datos
- **DTOs**: Objetos para transferir datos entre capas
- **Services**: Contiene la lógica de negocio de la aplicación
- **Repositories**: Acceso directo a la base de datos mediante EF Core
- **Data**: Contexto de Entity Framework y migraciones

#### Frontend

- **core**: Servicios centrales, guards, interceptores y modelos compartidos
- **auth**: Componentes relacionados con autenticación (login, registro)
- **tasks**: Componentes para gestión de tareas (lista, kanban, detalle)
- **shared**: Componentes, pipes y directivas reutilizables

---

## 📚 Documentación Técnica

### 🔐 Sistema de Autenticación JWT

El sistema implementa autenticación basada en **JSON Web Tokens (JWT)** con las siguientes características:

#### Flujo de Autenticación

1. **Login**: El usuario envía credenciales (email + password)
2. **Validación**: El backend verifica las credenciales contra la BD
3. **Generación de Token**: Si es válido, se genera un JWT firmado
4. **Almacenamiento**: El frontend guarda el token en localStorage
5. **Uso**:
```typescript
<app-kanban [tasks]="tasks" (taskMoved)="onTaskMoved($event)"></app-kanban>
```

**Código TypeScript**:
```typescript
export class KanbanComponent implements OnInit {
  @Input() tasks: Task[] = [];
  @Output() taskMoved = new EventEmitter<TaskMovedEvent>();

  columns = [
    { id: 'pending', title: 'Pendiente', status: TaskStatus.Pending },
    { id: 'inprogress', title: 'En Progreso', status: TaskStatus.InProgress },
    { id: 'completed', title: 'Completado', status: TaskStatus.Completed }
  ];

  drop(event: CdkDragDrop<Task[]>, newStatus: TaskStatus): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      
      const task = event.container.data[event.currentIndex];
      this.updateTaskStatus(task, newStatus);
    }
  }

  private updateTaskStatus(task: Task, newStatus: TaskStatus): void {
    this.taskMoved.emit({ task, newStatus });
  }
}
```

#### 2. TaskListComponent

**Propósito**: Vista de lista de tareas con opciones de ordenamiento

**Características**:
- Vista tabular con paginación
- Ordenamiento por columnas
- Filtros avanzados
- Acciones CRUD inline
- Búsqueda en tiempo real

**Código TypeScript**:
```typescript
export class TaskListComponent implements OnInit {
  tasks$: Observable<Task[]>;
  displayedColumns = ['title', 'status', 'priority', 'dueDate', 'actions'];
  searchTerm = '';
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private taskService: TaskService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.tasks$ = this.taskService.getTasks().pipe(
      map(tasks => this.filterTasks(tasks))
    );
  }

  onSearch(term: string): void {
    this.searchTerm = term;
    this.loadTasks();
  }

  editTask(task: Task): void {
    const dialogRef = this.dialog.open(TaskEditDialogComponent, {
      width: '600px',
      data: task
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  deleteTask(task: Task): void {
    if (confirm(`¿Eliminar la tarea "${task.title}"?`)) {
      this.taskService.deleteTask(task.id).subscribe(() => {
        this.loadTasks();
      });
    }
  }
}
```

#### 3. AuthGuard

**Propósito**: Protección de rutas que requieren autenticación

**Código Completo**:
```typescript
import { Injectable } from '@angular/core';
import { 
  CanActivate, 
  ActivatedRouteSnapshot, 
  RouterStateSnapshot, 
  Router 
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.authService.isAuthenticated()) {
      return true;
    }
    
    // Guardar URL intentada para redirigir después del login
    this.authService.redirectUrl = state.url;
    this.router.navigate(['/login']);
    return false;
  }
}
```

**Uso en rutas**:
```typescript
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'dashboard', 
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'tasks', 
    component: TaskListComponent,
    canActivate: [AuthGuard]
  }
];
```

#### 4. RoleGuard

**Propósito**: Protección de rutas por roles específicos

**Código Completo**:
```typescript
@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const requiredRoles = route.data['roles'] as string[];
    const userRole = this.authService.getUserRole();

    if (requiredRoles.includes(userRole)) {
      return true;
    }

    this.router.navigate(['/unauthorized']);
    return false;
  }
}
```

**Uso en rutas**:
```typescript
const routes: Routes = [
  { 
    path: 'admin/users', 
    component: UserManagementComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin'] }
  }
];
```

### 🔌 Servicios Backend Principales

#### AuthService.cs

**Responsabilidades**:
- Validación de credenciales
- Generación de tokens JWT
- Refresh de tokens
- Gestión de sesiones

**Métodos principales**:
```csharp
Task<AuthResponse> LoginAsync(LoginDto loginDto);
Task<AuthResponse> RegisterAsync(RegisterDto registerDto);
Task<bool> ValidateTokenAsync(string token);
Task RevokeTokenAsync(string token);
```

#### TaskService.cs

**Responsabilidades**:
- Lógica de negocio para tareas
- Validaciones
- Reglas de transición de estados

**Métodos principales**:
```csharp
Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto dto);
Task<bool> DeleteTaskAsync(int id);
Task<List<TaskDto>> GetUserTasksAsync(int userId);
Task<bool> ChangeStatusAsync(int taskId, TaskStatus newStatus);
```

---

## ⚡ Funcionalidades Implementadas

### 🛂 Módulo de Autenticación
- [x] Registro de usuarios con validación de datos
- [x] Inicio de sesión con email y contraseña
- [x] Generación de JWT con expiración configurable
- [x] Refresh token automático
- [x] Logout y revocación de tokens
- [x] Recuperación de contraseña vía email
- [x] Protección de rutas (AuthGuard + RoleGuard)
- [x] Interceptores HTTP para manejo de tokens y errores

---

### 🗂️ Módulo de Gestión de Tareas

#### 📝 CRUD Completo
- [x] Crear tareas con título, descripción, prioridad, fecha límite y categoría
- [x] Editar tareas existentes
- [x] Eliminar tareas (soft delete)
- [x] Visualizar detalle completo de cada tarea

#### 🗃️ Tablero Kanban Interactivo
- [x] Columnas personalizables  
- [x] Drag & Drop fluido  
- [x] Actualización automática de estado  
- [x] Contador de tareas por columna  
- [x] Filtros por categoría, estado y prioridad  

#### 🔥 Sistema de Prioridades
- [x] Pendiente
- [x] En progreso
- [x] Realizada
- [x] Indicadores visuales por color

#### 🏷️ Categorías
- [x] Asignar categorías a tareas  
- [x] Filtro por categorías  
- [x] Colores personalizados por categoría  

#### 🔍 Búsqueda y Filtros Avanzados
- [x] Búsqueda por título o descripción  
- [x] Filtro por estado  
- [x] Filtro por fecha de vencimiento  
- [x] Filtro por usuario asignado  
- [x] Filtro por categoría y prioridad  

---

### 👥 Módulo de Usuarios (Admin)
- [x] Listado completo de usuarios
- [x] Crear nuevos usuarios
- [x] Editar información
- [x] Asignar o cambiar roles
- [x] Activar o desactivar usuarios
- [x] Ver historial de actividad

---

### 📊 Dashboard y Reportes
- [x] Vista general con métricas
- [x] Gráficos de productividad
- [x] Conteo de tareas por estado
- [x] Estadísticas por usuario
- [x] Exportación a PDF
- [x] Exportación a Excel
- [x] Reportes personalizados


---

## 🌐 API Endpoints

### 🔐 Authentication

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/login` | Iniciar sesión | No |
| POST | `/api/auth/register` | Registrar nuevo usuario | No |
| POST | `/api/auth/refresh` | Renovar token JWT | Sí |
| POST | `/api/auth/logout` | Cerrar sesión | Sí |
| POST | `/api/auth/forgot-password` | Recuperar contraseña | No |

**Ejemplo - Login Request**:
```json
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "fullName": "John Doe",
    "role": "User"
  },
  "expiresIn": 3600
}
```

### 📝 Tasks

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/api/tasks` | Obtener todas las tareas | Sí |
| GET | `/api/tasks/{id}` | Obtener tarea específica | Sí |
| POST | `/api/tasks` | Crear nueva tarea | Sí |
| PUT | `/api/tasks/{id}` | Actualizar tarea | Sí |
| DELETE | `/api/tasks/{id}` | Eliminar tarea | Sí |
| PATCH | `/api/tasks/{id}/status` | Cambiar estado | Sí |
| GET | `/api/tasks/user/{userId}` | Tareas de un usuario | Sí |
| GET | `/api/tasks/kanban` | Vista Kanban | Sí |

**Ejemplo - Create Task Request**:
```json
POST /api/tasks
{
  "title": "Implementar autenticación",
  "description": "Configurar JWT en el backend",
  "priority": "High",
  "status": "Pending",
  "dueDate": "2024-12-31T23:59:59",
  "categoryId": 1,
  "userId": 1
}
```

**Response**:
```json
{
  "id": 42,
  "title": "Implementar autenticación",
  "description": "Configurar JWT en el backend",
  "priority": "High",
  "status": "Pending",
  "dueDate": "2024-12-31T23:59:59",
  "categoryId": 1,
  "userId": 1,
  "createdAt": "2024-11-22T10:30:00",
  "updatedAt": "2024-11-22T10:30:00"
}
```



## 💻 Comandos Útiles

### Backend (.NET)

```bash
# Compilar el proyecto
dotnet build

# Ejecutar en modo desarrollo
dotnet run

# Ejecutar con hot reload
dotnet watch run

# Crear migración
dotnet ef migrations add <NombreMigracion>

# Aplicar migraciones
dotnet ef database update

# Revertir migración
dotnet ef database update <MigracionAnterior>

# Eliminar última migración
dotnet ef migrations remove

# Generar script SQL de migraciones
dotnet ef migrations script

# Limpiar build
dotnet clean

# Restaurar paquetes NuGet
dotnet restore

# Publicar para producción
dotnet publish -c Release -o ./publish

# Ejecutar tests
dotnet test

# Ver información del proyecto
dotnet --info
```

### Frontend (Angular)

```bash
# Instalar dependencias
npm install

# Ejecutar servidor de desarrollo
ng serve

# Ejecutar y abrir navegador
ng serve --open

# Ejecutar en puerto específico
ng serve --port 4300

# Compilar para producción
ng build --configuration production

# Ejecutar tests unitarios
ng test

# Ejecutar tests e2e
ng e2e

# Generar componente
ng generate component nombre-componente
ng g c nombre-componente

# Generar servicio
ng generate service nombre-servicio
ng g s nombre-servicio

# Generar módulo
ng generate module nombre-modulo
ng g m nombre-modulo

# Generar guard
ng generate guard nombre-guard
ng g g nombre-guard

# Generar pipe
ng generate pipe nombre-pipe
ng g p nombre-pipe

# Generar directiva
ng generate directive nombre-directiva
ng g d nombre-directiva

# Analizar bundle size
ng build --stats-json
npm install -g webpack-bundle-analyzer
webpack-bundle-analyzer dist/stats.json

# Limpiar cache
npm cache clean --force
rm -rf node_modules package-lock.json
npm install

# Actualizar Angular CLI
npm install -g @angular/cli@latest

# Ver versión de Angular
ng version
```

### Git (Control de Versiones)

```bash
# Clonar repositorio
git clone https://github.com/Erickpe8/TaskFlow-Manager.git

# Ver estado
git status

# Agregar cambios
git add .

# Hacer commit
git commit -m "Descripción del cambio"

# Push a repositorio remoto
git push origin main

# Pull últimos cambios
git pull origin main

# Crear nueva rama
git checkout -b feature/nueva-funcionalidad

# Cambiar de rama
git checkout main

# Ver ramas
git branch

# Fusionar rama
git merge feature/nueva-funcionalidad

# Ver historial
git log --oneline

# Descartar cambios locales
git checkout -- .

# Ver diferencias
git diff
```

### Docker

```bash
# Construir imagen del backend
docker build -t taskflow-backend ./Backend

# Construir imagen del frontend
docker build -t taskflow-frontend ./Frontend

# Ejecutar contenedor backend
docker run -d -p 5208:80 --name backend taskflow-backend

# Ejecutar contenedor frontend
docker run -d -p 4200:80 --name frontend taskflow-frontend

# Ver contenedores corriendo
docker ps

# Ver logs de un contenedor
docker logs backend

# Detener contenedor
docker stop backend

# Eliminar contenedor
docker rm backend

# Ver imágenes
docker images

# Eliminar imagen
docker rmi taskflow-backend

# Docker Compose (si existe docker-compose.yml)
docker-compose up -d
docker-compose down
docker-compose logs -f
```

---

## 🐳 Dockerización

El proyecto está preparado para ser dockerizado completamente. A continuación se presentan los archivos necesarios:

### Dockerfile - Backend

```dockerfile
# Backend/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TaskFlow.Api.csproj", "./"]
RUN dotnet restore "TaskFlow.Api.csproj"
COPY . .
RUN dotnet build "TaskFlow.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskFlow.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]
```

### Dockerfile - Frontend

```dockerfile
# Frontend/Dockerfile

# Etapa 1: Construcción
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build --configuration production

# Etapa 2: Servidor Nginx
FROM nginx:alpine
COPY --from=build /app/dist/taskflow-frontend /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### nginx.conf (Frontend)

```nginx
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api {
        proxy_pass http://backend:80;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### docker-compose.yml (Raíz del Proyecto)

```yaml
version: '3.8'

services:
  # Base de datos SQL Server
  database:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: taskflow-db
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong!Password123
      - MSSQL_PID=Developer
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql
    networks:
      - taskflow-network

  # Backend API
  backend:
    build:
      context: ./Backend
      dockerfile: Dockerfile
    container_name: taskflow-backend
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=database;Database=TaskFlowDB;User Id=sa;Password=YourStrong!Password123;TrustServerCertificate=True
    ports:
      - "5208:80"
    depends_on:
      - database
    networks:
      - taskflow-network
    restart: unless-stopped

  # Frontend Angular
  frontend:
    build:
      context: ./Frontend
      dockerfile: Dockerfile
    container_name: taskflow-frontend
    ports:
      - "4200:80"
    depends_on:
      - backend
    networks:
      - taskflow-network
    restart: unless-stopped

networks:
  taskflow-network:
    driver: bridge

volumes:
  sqldata:
```

### Comandos Docker Compose

```bash
# Construir y levantar todos los servicios
docker-compose up -d --build

# Ver logs en tiempo real
docker-compose logs -f

# Detener todos los servicios
docker-compose down

# Detener y eliminar volúmenes
docker-compose down -v

# Reconstruir solo un servicio
docker-compose up -d --build backend

# Ver estado de servicios
docker-compose ps
```

---

## 🗺️ Roadmap

## 📌 Roadmap del Proyecto

### ✅ Fase 1: Planeación y Setup Inicial (Completado)
- [x] Definición de la idea: tablero Kanban para organizar tareas de equipos pequeños.
- [x] Elección del stack: **Backend .NET 10 (API REST)** + **Frontend Angular**.
- [x] Creación del repositorio en GitHub y organización de ramas (`main`, ramas de desarrollo).
- [x] Inicialización de los proyectos **Backend** y **Frontend**.
- [x] Configuración básica del entorno de desarrollo y control de versiones.

---

### ✅ Fase 2: Backend – API y Modelo de Dominio (Completado)
- [x] Definición del modelo de dominio: `User`, `TaskItem`, `Column`, `Category`, `Role`.
- [x] Configuración de **Entity Framework Core**, `AppDbContext` y migraciones iniciales.
- [x] Creación de endpoints REST para:
  - [x] Gestión de columnas (crear, listar, actualizar, eliminar).
  - [x] Gestión de tareas (crear, mover, actualizar, eliminar).
- [x] Implementación de autenticación con **JWT** (login, registro básico).
- [x] Manejo centralizado de errores con *middleware* personalizado.
- [x] Exposición y prueba de la API mediante **Swagger**.

---

### ✅ Fase 3: Frontend – Interfaz y Experiencia de Usuario (Completado)
- [x] Configuración de rutas principales en Angular (login, registro, tablero).
- [x] Implementación de **guards** para proteger las rutas autenticadas.
- [x] Creación de servicios Angular para consumo de API (`Auth`, `Tasks`, `Columns`).
- [x] Construcción del tablero **Kanban**:
  - [x] Listado de columnas y tareas por estado.
  - [x] Creación y edición de tareas desde el frontend.
  - [x] Movimiento de tareas entre columnas mediante **drag & drop**.
- [x] Diseño responsive básico y mejoras de UX (feedback visual, estados de carga y error).

---

### ✅ Fase 4: Integración, Pruebas Manuales y Despliegue (Completado)
- [x] Integración completa **Frontend ↔ Backend** con manejo de tokens JWT.
- [x] Pruebas funcionales del flujo principal:
  - [x] Registro / login de usuario.
  - [x] Acceso al tablero Kanban.
  - [x] Creación, edición, movimiento y eliminación de tareas.
- [x] Configuración de imágenes Docker para frontend y backend.
- [x] Despliegue del frontend en servidor (**CapRover / Grupolimon**):
  - 🌐 Demo: `http://taskflow-frontend.grupolimon.online/login`
- [x] Grabación de **video explicativo** del flujo de la aplicación y arquitectura.
- [x] Documentación básica inicial en el `README`.


---

## 👥 Contribuidores

Este proyecto fue desarrollado por un equipo de estudiantes de la **Facultad de Ingeniería** de la **FESC**, como proyecto final para la asignatura **DAAR (Desarrollo de Aplicaciones Avanzadas en Red)**.

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/Erickpe8">
        <img src="https://github.com/Erickpe8.png" width="100px;" alt="Erick Pérez"/><br />
        <sub><b>Erick Sebastián Pérez Carvajal</b></sub>
      </a><br />
      <sub>Propietario & Full-Stack Developer</sub><br />
      🏗️ 💻 📖 🎨 🔧
    </td>
    <td align="center">
      <a href="https://github.com/Santiago-Rueda-Q">
        <img src="https://github.com/Santiago-Rueda-Q.png" width="100px;" alt="Santiago Rueda"/><br />
        <sub><b>Santiago Rueda Quintero</b></sub>
      </a><br />
      <sub>Backend Developer</sub><br />
      ⚙️ 📝 🔍
    </td>
    <td align="center">
      <a href="https://github.com/BrayanG2004">
        <img src="https://github.com/BrayanG2004.png" width="100px;" alt="Brayan Arley"/><br />
        <sub><b>Brayan Arley</b></sub>
      </a><br />
      <sub>Frontend Developer</sub><br />
      🎨 💅 🖼️
    </td>
    <td align="center">
      <a href="https://github.com/TIC0o">
        <img src="https://github.com/TIC0o.png" width="100px;" alt="Eliécer Guevara"/><br />
        <sub><b>Eliécer Guevara</b></sub>
      </a><br />
      <sub>QA & Testing</sub><br />
      🐛 ✅ 📊
    </td>
  </tr>
</table>


### 🤝 Cómo Contribuir

Aunque este es un proyecto académico, agradecemos cualquier contribución. Si deseas colaborar:

1. **Fork** el repositorio
2. Crea una **rama** para tu feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. **Push** a la rama (`git push origin feature/AmazingFeature`)
5. Abre un **Pull Request**

### 📝 Lineamientos de Contribución

- Sigue las convenciones de código establecidas
- Escribe tests para nuevas funcionalidades
- Documenta tus cambios
- Asegúrate de que el build pase correctamente
- Actualiza el README si es necesario

---

## 📄 Licencia

Este proyecto está bajo la licencia **MIT**. Consulta el archivo [LICENSE](LICENSE) para más detalles.

```
MIT License

Copyright (c) 2024 Erick Pérez, Santiago Rueda, Brayan Arley, Eliécer Guevara

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 📞 Contacto

### Erick Sebastián Pérez Carvajal
- **GitHub**: [@Erickpe8](https://github.com/Erickpe8)
- **Email**: ericksperez@gmail.com
- **LinkedIn**: [Erick Pérez](https://www.linkedin.com/in/erick-sebastian-perez-carvajal-11a2772b6/)

### Repositorio del Proyecto
- **URL**: [https://github.com/Erickpe8/TaskFlow-Manager](https://github.com/Erickpe8/TaskFlow-Manager)
- **Issues**: [Reportar un problema](https://github.com/Erickpe8/TaskFlow-Manager/issues)
- **Discussions**: [Foro de discusión](https://github.com/Erickpe8/TaskFlow-Manager/discussions)

---

## 🙏 Agradecimientos

Queremos agradecer a:

- **Comunidad de .NET y Angular** - Por la documentación y recursos
- **GitHub** - Por proporcionar la plataforma de colaboración
- **Microsoft Learn** - Por los excelentes tutoriales de .NET
- **Angular.io** - Por la documentación completa y ejemplos
- **Todos los colaboradores** - Por su tiempo y dedicación

---

## 📚 Referencias y Recursos

### Documentación Oficial
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Angular Documentation](https://angular.io/docs)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT.io](https://jwt.io/)

### Tutoriales Útiles
- [Microsoft Learn - ASP.NET Core](https://learn.microsoft.com/en-us/training/aspnet-core/)
- [Angular University](https://angular-university.io/)
- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Herramientas
- [Visual Studio Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/)
- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/)

---

## 🎓 Información Académica

**Institución**: FESC 
**Facultad**: Ingeniería  
**Programa**: Ingeniería de Software
**Asignatura**: DAAR (Desarrollo de Aplicaciones Avanzadas en Red)  
**Semestre**: 2025-2  
**Tipo de Proyecto**: Proyecto Final de Asignatura

### 📋 Objetivos Académicos Alcanzados

✅ Implementar una arquitectura cliente-servidor moderna  
✅ Aplicar patrones de diseño y buenas prácticas  
✅ Desarrollar un sistema full-stack funcional  
✅ Integrar tecnologías actuales de la industria  
✅ Implementar seguridad en aplicaciones web  
✅ Gestionar un proyecto de software con control de versiones  
✅ Documentar técnicamente un sistema completo  
✅ Trabajar colaborativamente en equipo  


---

## 🌟 Showcase

### 🔗 Demo en Vivo  
Aplicación desplegada y completamente funcional:  
👉 **http://taskflow-frontend.grupolimon.online/login**

### 🎥 Video Demostrativo  
Explicación del funcionamiento general, arquitectura y flujo completo del sistema:  
👉 **https://youtu.be/1v-fYTbl4bo?si=10lJxwkmuFnNWA2j**

### 📸 Capturas de Pantalla  
<img width="1600" height="781" alt="image" src="https://github.com/user-attachments/assets/844b68bd-8c6d-44c9-bbe5-80623747e173" />
<img width="1600" height="773" alt="image" src="https://github.com/user-attachments/assets/36589366-1a22-416c-a81f-3e454ef25771" />
<img width="1600" height="771" alt="image" src="https://github.com/user-attachments/assets/5d877fd2-2c13-4f08-a163-5ec21d6ae4ff" />
<img width="1600" height="781" alt="image" src="https://github.com/user-attachments/assets/65763043-77f1-474b-89b2-fd7795820e95" />
<img width="1600" height="837" alt="image" src="https://github.com/user-attachments/assets/ed78da17-27af-4c04-876f-85413ef5ade2" />

---

<div align="center">

## ⭐ Si este proyecto te fue útil, ¡dale una estrella!

**Desarrollado con ❤️ por el equipo de TaskFlow**

![Visitors](https://visitor-badge.laobi.icu/badge?page_id=Erickpe8.TaskFlow-Manager)
![GitHub last commit](https://img.shields.io/github/last-commit/Erickpe8/TaskFlow-Manager)
![GitHub issues](https://img.shields.io/github/issues/Erickpe8/TaskFlow-Manager)
![GitHub pull requests](https://img.shields.io/github/issues-pr/Erickpe8/TaskFlow-Manager)

---

**© 2024 TaskFlow Manager - Todos los derechos reservados**

[⬆ Volver arriba](#-taskflow-manager)
