using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain;

namespace TaskFlow.Api.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Ejecutar migraciones pendientes
            await context.Database.MigrateAsync();

            // Semillas de seguridad (roles, permisos, usuario)
            await SeedSecurityAsync(context);

            // Semillas del tablero Kanban (columnas y tareas)
            await SeedKanbanAsync(context);
        }

        private static async Task SeedSecurityAsync(AppDbContext context)
        {
            // Si ya existen roles, asumimos que los datos base de seguridad están creados
            if (await context.Roles.AnyAsync())
                return;

            // 1. Roles
            var teacherRole = new Role { Name = "Teacher" };
            var studentRole = new Role { Name = "Student" };

            context.Roles.AddRange(teacherRole, studentRole);
            await context.SaveChangesAsync();

            // 2. Permisos
            var permissions = new List<Permission>
            {
                new Permission { Name = "users.read" },
                new Permission { Name = "users.create" },
                new Permission { Name = "users.update" },
                new Permission { Name = "users.delete" },
                new Permission { Name = "tasks.read" },
                new Permission { Name = "tasks.create" },
                new Permission { Name = "tasks.update" },
                new Permission { Name = "tasks.delete" },
                new Permission { Name = "columns.read" },
                new Permission { Name = "columns.create" },
                new Permission { Name = "columns.update" },
                new Permission { Name = "columns.delete" }
            };

            context.Permissions.AddRange(permissions);
            await context.SaveChangesAsync();

            // 3. Asignar permisos a roles
            var teacherRolePermissions = permissions
                .Select(p => new RolePermission
                {
                    RoleId = teacherRole.Id,
                    PermissionId = p.Id
                })
                .ToList();

            var studentPermissionNames = new[]
           {
                "users.read",
                "tasks.read", "tasks.create", "tasks.update",
                "columns.read", "columns.create", "columns.update"
            };

            var studentRolePermissions = permissions
                .Where(p => studentPermissionNames.Contains(p.Name))
                .Select(p => new RolePermission
                {
                    RoleId = studentRole.Id,
                    PermissionId = p.Id
                })
                .ToList();

            context.RolePermissions.AddRange(teacherRolePermissions);
            context.RolePermissions.AddRange(studentRolePermissions);
            await context.SaveChangesAsync();

            // 4. Usuario por defecto (Teacher)
            var seededUser = new User
            {
                Email = "doc_js_galindo@fesc.edu.co",
                FullName = "Docente Galindo",
                IsActive = true
            };

            var passwordHasher = new PasswordHasher<User>();
            seededUser.PasswordHash = passwordHasher.HashPassword(seededUser, "0123456789");

            context.Users.Add(seededUser);
            await context.SaveChangesAsync();

            // 5. Relación UserRole
            var userRole = new UserRole
            {
                UserId = seededUser.Id,
                RoleId = teacherRole.Id
            };

            context.UserRoles.Add(userRole);
            await context.SaveChangesAsync();
        }

        private static async Task SeedKanbanAsync(AppDbContext context)
        {
            // Si ya hay columnas, asumimos que el tablero ya está sembrado
            if (await context.Columns.AnyAsync())
                return;

            var columns = new List<Column>
            {
                new Column { Name = "Pendiente",   Order = 1 },
                new Column { Name = "En progreso", Order = 2 },
                new Column { Name = "Completada",  Order = 3 }
            };

            context.Columns.AddRange(columns);
            await context.SaveChangesAsync();

            var colPendiente = columns[0].Id;
            var colProgreso = columns[1].Id;
            var colDone = columns[2].Id;

            var tasks = new List<TaskItem>
            {
                new TaskItem {
                    Title = "Configurar estructura Angular",
                    Description = "Crear módulos, servicios y componentes base",
                    ColumnId = colPendiente,
                    Order = 1
                },
                new TaskItem {
                    Title = "Diseñar Layout del Kanban",
                    Description = "Crear columnas y tarjetas mock",
                    ColumnId = colPendiente,
                    Order = 2
                },
                new TaskItem {
                    Title = "Implementar backend CRUD",
                    Description = "Services, controllers y EF Core",
                    ColumnId = colProgreso,
                    Order = 1
                }
            };

            context.TaskItems.AddRange(tasks);
            await context.SaveChangesAsync();
        }
    }
}
