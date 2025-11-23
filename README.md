# TaskFlow Manager – Guía de ejecución (Backend .NET + Frontend Angular)

este proyecto está compuesto por:

- **Backend:** .NET (Minimal API – .NET 10)
- **Frontend:** Angular 17+ (CLI 21)

---

## 🖥️ Backend (.NET) – Cómo levantar el servidor

### Requisitos
- .NET SDK 10  
  ```
  dotnet --version
  ```

### Ubicación
```
/TaskFlow-Manager/Backend
```

### Ejecutar
```
cd Backend
dotnet run
```

El backend escuchará en:
```
http://localhost:5208
```

### Endpoints disponibles
- Weather Forecast:  
  `http://localhost:5208/weatherforecast`

- OpenAPI JSON:  
  `http://localhost:5208/openapi/v1.json`

---

## 💻 Frontend (Angular) – Cómo levantar la aplicación

### Requisitos
- Node.js 18+
- Angular CLI 21  
  ```
  npm install -g @angular/cli
  ```

### Ubicación
```
/TaskFlow-Manager/Frontend
```

### Instalar dependencias
```
cd Frontend
npm install
```

### Ejecutar servidor
```
ng serve
```

Abrir en:
```
http://localhost:4200/
```

---

## 🔧 Backend + Frontend juntos

1. Backend  
   ```
   cd Backend
   dotnet run
   ```
2. Frontend  
   ```
   cd Frontend
   ng serve
   ```

---

## ✔️ Resumen
- Backend → http://localhost:5208  
- Frontend → http://localhost:4200
